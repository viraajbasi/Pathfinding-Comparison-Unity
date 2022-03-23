using System.Collections.Generic;
using System.Diagnostics;
using MainMenu;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Maze
{
	public class Render : MonoBehaviour
	{
		public static int DijkstraStartNodeIndex;
		public Transform wallPrefab;
		public Transform floorPrefab;
		public Transform mazeObjectPrefab;
		public GameObject completedScreen;

		private static Position _startPosition = new(0, 0);
		private int _width = 100;
		private int _height = 100;
		private List<MazeCell> _sortedMaze;
		private List<MazeCell> _dijkstraMaze;
		private Stopwatch _stopwatch = new();

		private void Awake()
		{
			PlayerPrefs.DeleteKey("MazeSolved");
			PauseMenu.GameCompleted = false;
		}

		private void Start()
		{
			if (PlayerPrefs.GetInt("UserSolves") == 1)
			{
				UserSolves.StartPosition = _startPosition;
				_width = 20;
				_height = 20;
			}
			
			_sortedMaze = GenerateRandomMaze(_width, _height);
			DrawMaze(_sortedMaze);

			for (int i = 0; i < _width; i++)
			{
				for (int j = 0; j < _height; j++)
				{
					var currentIndex = _sortedMaze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					_sortedMaze[currentIndex].Visited = false;
				}
			}

			if (PlayerPrefs.GetInt("Pathfinding") == 1)
			{
				_stopwatch.Start();
				_dijkstraMaze = Dijkstra.Algorithm(_sortedMaze);
				_stopwatch.Stop();
				DijkstraStartNodeIndex = _dijkstraMaze.FindIndex(a => a.StartNode);
				Debug.Log($"Elapsed Milliseconds = {_stopwatch.ElapsedMilliseconds}");
			}
		}

		private void Update()
		{
			if (PlayerPrefs.GetInt("UserSolves") == 1)
			{
				UserSolves.HandleKeyInput(_sortedMaze);

				if (PlayerPrefs.GetInt("MazeSolved") == 1)
				{
					PauseMenu.GameCompleted = true;
					completedScreen.SetActive(true);
				}
			}

			if (Input.GetKeyDown(KeyCode.H))
			{
				if (PlayerPrefs.GetInt("Pathfinding") == 1)
				{
					var endNodeIndex = _dijkstraMaze.FindIndex(a => a.GoalNode);
					
					while (DijkstraStartNodeIndex != endNodeIndex)
					{
						Dijkstra.GeneratePathToNode(_dijkstraMaze, DijkstraStartNodeIndex);
					}

					_dijkstraMaze[endNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
				}
			}
		}

		private List<MazeCell> GenerateRandomMaze(int w, int h)
		{
			var maze = new List<MazeCell>();

			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					maze.Add(PlayerPrefs.GetInt("BellmanFord") == 0
						? new MazeCell(true, true, true, true, false, i, j, 1)
						: new MazeCell(true, true, true, true, false, i, j, ReturnCost()));
				}
			}

			maze[maze.FindIndex(a => a.Coordinates.X == 0 && a.Coordinates.Y == 0)].StartNode = true;
			maze[maze.FindIndex(a => a.Coordinates.X == w - 1 && a.Coordinates.Y == h - 1)].GoalNode = true;

			var newMaze = PlayerPrefs.GetInt("Kruskal") == 1 ? Kruskal.Algorithm(maze, w, h) : RecursiveBacktracker.Algorithm(maze, w, h);
			return newMaze;
		}

		private void DrawMaze(List<MazeCell> maze)
		{
			var size = 0.5f;
			var topOffset = new Vector3(0, 0, size);
			var leftOffset = new Vector3(-size, 0, 0);
			var rightOffset = new Vector3(size, 0, 0);
			var bottomOffset = new Vector3(0, 0, -size);

			for (int i = 0; i < _width; i++)
			{
				for (int j = 0; j < _height; j++)
				{
					var currentIndex = maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					var pos = new Vector3(-_width / 2 + i, 0, -_height / 2 + j);

					maze[currentIndex].MazeNode = Instantiate(mazeObjectPrefab, pos + new Vector3(0, size, 0), Quaternion.identity,transform);
					maze[currentIndex].MazeNode.name = $"Node ({i},{j})";
					maze[currentIndex].MazeNode.gameObject.SetActive(false);

					if (maze[currentIndex].StartNode)
					{
						maze[currentIndex].MazeNode.name = $"Node (Start) ({i},{j})";
						maze[currentIndex].MazeNode.gameObject.SetActive(true);
						maze[currentIndex].MazeNode.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
					}
					if (maze[currentIndex].GoalNode)
					{
						maze[currentIndex].MazeNode.name = $"Node (Goal) ({i},{j})";
						maze[currentIndex].MazeNode.gameObject.SetActive(true);
						maze[currentIndex].MazeNode.GetComponent<Renderer>().material.color = new Color(102, 190, 0);
					}

					maze[currentIndex].Floor = Instantiate(floorPrefab, pos, Quaternion.identity, transform);
					maze[currentIndex].Floor.gameObject.name = $"Node ({i},{j}) Floor";
					if (maze[currentIndex].Cost < 0)
					{
						maze[currentIndex].Floor.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 179);
					}
					
					if (maze[currentIndex].Top)
					{
						var topWall = Instantiate(wallPrefab, pos + topOffset, Quaternion.identity, transform);
						topWall.name = $"Node ({i},{j}) Top Wall";
					}

					if (maze[currentIndex].Left)
					{
						var leftWall = Instantiate(wallPrefab, pos + leftOffset, Quaternion.Euler(0, 90, 0), transform);
						leftWall.name = $"Node ({i},{j}) Left Wall";
					}

					if (i == _width - 1)
					{
						if (maze[currentIndex].Right)
						{
							var rightWall = Instantiate(wallPrefab, pos + rightOffset, Quaternion.Euler(0, 90, 0), transform);
							rightWall.name = $"Node ({i},{j}) Right Wall";
						}
					}

					if (j == 0)
					{
						if (maze[currentIndex].Bottom)
						{
							var bottomWall = Instantiate(wallPrefab, pos + bottomOffset, Quaternion.identity, transform);
							bottomWall.name = $"Node ({i},{j}) Bottom Wall";
						}
					}
				}
			}
		}

		private int ReturnCost()
		{
			int rndIndex = Random.Range(-10, 10);

			if (rndIndex < 0)
			{
				return -1;
			}

			return 1;
		}
	}
}
