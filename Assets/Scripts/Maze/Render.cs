using System.Collections.Generic;
using System.Diagnostics;
using MainMenu;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Maze
{
	public class Render : MonoBehaviour
	{
		public Transform wallPrefab;
		public Transform floorPrefab;
		public Transform startEndPrefab;
		public Transform wallObject;
		public Transform floorObject;
		public Transform pathDijkstra;
		public Transform pathAStar;
		public Transform pathBellmanFord;
		public GameObject completedScreen;
		public Material pathMaterial;
		public Material defaultFloorMaterial;
		
		private const float Offset = 0.5f;

		private static readonly Position StartPosition = new(0, 0);
		
		private readonly Stopwatch _stopwatch = new();
		
		private int _width = 70;
		private int _height = 70;
		private List<MazeCell> _sortedMaze;
		private List<MazeCell> _dijkstraMaze;
		private bool _dijkstraAlreadyDisplayed;
		private List<MazeCell> _aStarMaze;
		private bool _aStarAlreadyDisplayed;

		private void Start()
		{
			Application.targetFrameRate = -1;
			PauseMenu.GameCompleted = false;
			if (PlayerPrefs.GetInt("UserSolves") == 1)
			{
				UserSolves.StartPosition = StartPosition;
				_width = 20;
				_height = 20;
				PlayerPrefs.DeleteKey("MazeSolved");
			}
			
			_sortedMaze = GenerateRandomMaze(_width, _height);
			DrawMaze(_sortedMaze);
			for (var i = 0; i < _width; i++)
			{
				for (var j = 0; j < _height; j++)
				{
					_sortedMaze.Find(a => a.Coordinates.X == i && a.Coordinates.Y == j).Visited = false;
				}
			}

			if (PlayerPrefs.GetInt("Pathfinding") == 1)
			{
				_stopwatch.Start();
				_dijkstraMaze = Dijkstra.Algorithm(_sortedMaze);
				_stopwatch.Stop();
				Debug.Log($"Elapsed Milliseconds = {_stopwatch.ElapsedMilliseconds}");
				_stopwatch.Reset();
				pathDijkstra.gameObject.SetActive(false);

				_stopwatch.Start();
				_aStarMaze = AStar.Algorithm(_sortedMaze);
				_stopwatch.Stop();
				Debug.Log($"Elapsed milliseconds = {_stopwatch.ElapsedMilliseconds}");
				_stopwatch.Reset();
				pathAStar.gameObject.SetActive(false);
			}
			
			MeshCombiner.MazeRendered = true;
		}

		private void Update()
		{
			if (PlayerPrefs.GetInt("UserSolves") == 1 && Time.timeScale > 0)
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
				if (Input.GetKeyUp(KeyCode.H))
				{
					_dijkstraAlreadyDisplayed = !_dijkstraAlreadyDisplayed;

					if (_dijkstraAlreadyDisplayed)
					{
						ChangeParentOfObjects(pathDijkstra, pathMaterial, _dijkstraMaze);
					}
					else
					{
						ChangeParentOfObjects(floorObject, defaultFloorMaterial, _dijkstraMaze);
					}
					
					pathDijkstra.gameObject.SetActive(_dijkstraAlreadyDisplayed);
				}

				if (Input.GetKeyUp(KeyCode.J))
				{
					_aStarAlreadyDisplayed = !_aStarAlreadyDisplayed;

					if (_aStarAlreadyDisplayed)
					{
						ChangeParentOfObjects(pathAStar, pathMaterial, _aStarMaze);
					}
					else
					{
						ChangeParentOfObjects(floorObject, defaultFloorMaterial, _aStarMaze);
					}
					
					pathAStar.gameObject.SetActive(_aStarAlreadyDisplayed);
				}

				if (Input.GetKeyUp(KeyCode.K))
				{
				}
			}
		}

		private List<MazeCell> GenerateRandomMaze(int w, int h)
		{
			var maze = new List<MazeCell>();

			for (var i = 0; i < w; i++)
			{
				for (var j = 0; j < h; j++)
				{
					maze.Add(new MazeCell(true, true, true, true, false, i, j, 1));
				}
			}

			maze.Find(a => a.Coordinates.X == 0 && a.Coordinates.Y == 0).StartNode = true;
			maze.Find(a => a.Coordinates.X == w - 1 && a.Coordinates.Y == h - 1).GoalNode = true;

			return PlayerPrefs.GetInt("Kruskal") == 1 ? Kruskal.Algorithm(maze, w, h) : RecursiveBacktracker.Algorithm(maze, w, h);
		}

		private void DrawMaze(List<MazeCell> maze)
		{
			var topOffset = new Vector3(0, 0, Offset);
			var leftOffset = new Vector3(-Offset, 0, 0);
			var rightOffset = new Vector3(Offset, 0, 0);
			var bottomOffset = new Vector3(0, 0, -Offset);
			
			for (var i = 0; i < _width; i++)
			{
				for (var j = 0; j < _height; j++)
				{
					var currentIndex = maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					var pos = new Vector3(-_width / 2 + i, 0, -_height / 2 + j);

					if (maze[currentIndex].StartNode)
					{
						maze[currentIndex].MazeNode = Instantiate(startEndPrefab, pos + new Vector3(0, Offset, 0), Quaternion.identity,transform);
						maze[currentIndex].MazeNode.name = $"Node (Start) ({i},{j})";
						maze[currentIndex].MazeNode.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
					}
					else if (maze[currentIndex].GoalNode)
					{
						maze[currentIndex].MazeNode = Instantiate(startEndPrefab, pos + new Vector3(0, Offset, 0), Quaternion.identity,transform);
						maze[currentIndex].MazeNode.name = $"Node (Goal) ({i},{j})";
						maze[currentIndex].MazeNode.GetComponent<Renderer>().material.color = new Color(102, 190, 0);
					}

					maze[currentIndex].Floor = Instantiate(floorPrefab, pos, Quaternion.identity, floorObject);
					maze[currentIndex].Floor.gameObject.name = $"Node ({i},{j}) Floor";

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

		private void ChangeParentOfObjects(Transform parent, Material newMaterial, List<MazeCell> mazeList)
		{
			foreach (var node in mazeList)
			{
				node.Floor.gameObject.GetComponent<Renderer>().material = newMaterial;
				node.Floor.parent = parent;
			}
		}
	}
}
