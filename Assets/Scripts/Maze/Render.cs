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
		public Transform startEndPrefab;
		public Transform wallObject;
		public Transform floorObject;
		public GameObject completedScreen;

		private static Position _startPosition = new(0, 0);
		private int _width = 100;
		private int _height = 100;
		private bool _alreadyDisplayedPath;
		private List<MazeCell> _sortedMaze;
		private List<MazeCell> _dijkstraMaze;
		private Stopwatch _stopwatch = new();
		
		private void Start()
		{
			PauseMenu.GameCompleted = false;
			if (PlayerPrefs.GetInt("UserSolves") == 1)
			{
				UserSolves.StartPosition = _startPosition;
				_width = 20;
				_height = 20;
				PlayerPrefs.DeleteKey("MazeSolved");
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
			
			if (PlayerPrefs.GetInt("Pathfinding") == 1)
			{
				if (Input.GetKeyDown(KeyCode.H))
				{
					_alreadyDisplayedPath = !_alreadyDisplayedPath;
					_stopwatch.Start();
					_dijkstraMaze = Dijkstra.Algorithm(_sortedMaze);
					_stopwatch.Stop();
					DijkstraStartNodeIndex = _dijkstraMaze.FindIndex(a => a.StartNode);
					Debug.Log($"Elapsed Milliseconds = {_stopwatch.ElapsedMilliseconds}");

					var endNodeIndex = _dijkstraMaze.FindIndex(a => a.GoalNode);

					if (_alreadyDisplayedPath)
					{
						while (DijkstraStartNodeIndex != endNodeIndex)
						{
							Dijkstra.GeneratePathToNode(_dijkstraMaze, DijkstraStartNodeIndex);
						}

						_dijkstraMaze[endNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color =
							Color.black;
						_dijkstraMaze[endNodeIndex].Floor.gameObject.SetActive(true);

						foreach (var n in _dijkstraMaze)
						{
							if (n.Floor.gameObject.GetComponent<Renderer>().material.color != Color.black)
							{
								n.Floor.gameObject.SetActive(false);
							}
						}
					}
					else
					{
						foreach (var n in _dijkstraMaze)
						{
							n.Floor.gameObject.SetActive(false);
						}
					}
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

			var bigFloor = Instantiate(floorPrefab, new Vector3(-size, -size, -size), Quaternion.identity, transform);
			bigFloor.localScale = new Vector3(_width / 10, 1, _height / 10);
			bigFloor.name = "Big floor";

			for (int i = 0; i < _width; i++)
			{
				for (int j = 0; j < _height; j++)
				{
					var currentIndex = maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					var pos = new Vector3(-_width / 2 + i, 0, -_height / 2 + j);

					if (maze[currentIndex].StartNode)
					{
						maze[currentIndex].MazeNode = Instantiate(startEndPrefab, pos + new Vector3(0, size, 0), Quaternion.identity,transform);
						maze[currentIndex].MazeNode.name = $"Node (Start) ({i},{j})";
						maze[currentIndex].MazeNode.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
					}
					else if (maze[currentIndex].GoalNode)
					{
						maze[currentIndex].MazeNode = Instantiate(startEndPrefab, pos + new Vector3(0, size, 0), Quaternion.identity,transform);
						maze[currentIndex].MazeNode.name = $"Node (Goal) ({i},{j})";
						maze[currentIndex].MazeNode.GetComponent<Renderer>().material.color = new Color(102, 190, 0);
					}

					maze[currentIndex].Floor = Instantiate(floorPrefab, pos, Quaternion.identity, floorObject);
					maze[currentIndex].Floor.gameObject.name = $"Node ({i},{j}) Floor";
					maze[currentIndex].Floor.gameObject.SetActive(false);
					if (maze[currentIndex].Cost < 0)
					{
						maze[currentIndex].Floor.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 179);
					}
					
					if (maze[currentIndex].Top)
					{
						var topWall = Instantiate(wallPrefab, pos + topOffset, Quaternion.identity, wallObject);
						topWall.name = $"Node ({i},{j}) Top Wall";
					}

					if (maze[currentIndex].Left)
					{
						var leftWall = Instantiate(wallPrefab, pos + leftOffset, Quaternion.Euler(0, 90, 0), wallObject);
						leftWall.name = $"Node ({i},{j}) Left Wall";
					}

					if (i == _width - 1)
					{
						if (maze[currentIndex].Right)
						{
							var rightWall = Instantiate(wallPrefab, pos + rightOffset, Quaternion.Euler(0, 90, 0), wallObject);
							rightWall.name = $"Node ({i},{j}) Right Wall";
						}
					}

					if (j == 0)
					{
						if (maze[currentIndex].Bottom)
						{
							var bottomWall = Instantiate(wallPrefab, pos + bottomOffset, Quaternion.identity, wallObject);
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
