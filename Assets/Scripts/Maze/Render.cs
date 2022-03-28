using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MainMenu;
using UnityEditor;
using UnityEngine;
using static System.String;
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

		private const float Offset = 0.5f;

		private static readonly Position StartPosition = new(0, 0);

		private int _width = 100;
		private int _height = 100;
		private List<MazeCell> _sortedMaze;

		private int _totalNodes;
		private long _averageTimeTaken;
		private string _currentAlgorithm;
		
		private List<MazeCell> _dijkstraMaze;
		private bool _dijkstraAlreadyDisplayed = true;
		private long _dijkstraTimeTaken;
		private int _dijkstraNodesVisited;
		
		private List<MazeCell> _aStarMaze;
		private bool _aStarAlreadyDisplayed = true;
		private long _aStarTimeTaken;
		private int _aStarNodesVisited;
		
		private List<MazeCell> _bellmanFordMaze;
		private bool _bellmanFordAlreadyDisplayed = true;
		private long _bellmanFordTimeTaken;
		private int _bellmanFordNodesVisited;

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

			_totalNodes = _sortedMaze.Count;
			Debug.Log($"Total Nodes = {_totalNodes}");

			if (PlayerPrefs.GetInt("Pathfinding") == 1)
			{
				var (dijkstraMaze, dijkstraTime) = ExecuteAlgorithmAndFindTimeTaken(1);
				_dijkstraMaze = dijkstraMaze;
				_dijkstraTimeTaken = dijkstraTime;
				Debug.Log($"Elapsed Milliseconds = {_dijkstraTimeTaken}");
				pathDijkstra.gameObject.SetActive(false);
				_dijkstraNodesVisited = FindTotalVisitedNodes(_dijkstraMaze);
				Debug.Log($"Dijkstra Total Visited Nodes = {_dijkstraNodesVisited}");
				
				var (aStarMaze, aStarTime) = ExecuteAlgorithmAndFindTimeTaken(2);
				_aStarMaze = aStarMaze;
				_aStarTimeTaken = aStarTime;
				Debug.Log($"Elapsed milliseconds = {_aStarTimeTaken}");
				pathAStar.gameObject.SetActive(false);
				_aStarNodesVisited = FindTotalVisitedNodes(_aStarMaze);
				Debug.Log($"A* Total Visited Nodes = {_aStarNodesVisited}");
				
				var (bellmanFordMaze, bellmanFordTime) = ExecuteAlgorithmAndFindTimeTaken(3);
				_bellmanFordMaze = bellmanFordMaze;
				_bellmanFordTimeTaken = bellmanFordTime;
				Debug.Log($"Elapsed Milliseconds = {_bellmanFordTimeTaken}");
				pathBellmanFord.gameObject.SetActive(false);
				_bellmanFordNodesVisited = FindTotalVisitedNodes(_bellmanFordMaze);
				Debug.Log($"Bellman-Ford Visited Nodes = {_bellmanFordNodesVisited}");

				_averageTimeTaken = _aStarTimeTaken + _dijkstraTimeTaken + _bellmanFordTimeTaken / 3;
				Debug.Log($"Average Time = {_averageTimeTaken}");
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
				foreach (KeyCode vKey in Enum.GetValues(typeof(KeyCode)))
				{
					if (Input.GetKeyUp(vKey))
					{
						_dijkstraAlreadyDisplayed = !_dijkstraAlreadyDisplayed;
						_aStarAlreadyDisplayed = !_aStarAlreadyDisplayed;
						_bellmanFordAlreadyDisplayed = !_bellmanFordAlreadyDisplayed;
						HandleKeyInput(vKey);
					}
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
					maze.Add(new MazeCell(true, true, true, true, false, i, j));
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

		private void ChangeParentOfObjects(Transform parent, List<MazeCell> mazeList)
		{
			foreach (var node in mazeList.Where(node => node.Path))
			{
				node.Floor.parent = parent;
				node.Floor.gameObject.GetComponent<Renderer>().material = parent.gameObject.GetComponent<Renderer>().material;
			}
		}

		private void HandleKeyInput(KeyCode key)
		{
			Transform parentObject;
			List<MazeCell> maze;
			bool isDisplayed;
			
			switch (key)
			{
				case KeyCode.H:
					parentObject = pathDijkstra;
					maze = _dijkstraMaze;
					isDisplayed = _dijkstraAlreadyDisplayed;
					_currentAlgorithm = "Dijkstra";
					break;
				
				case KeyCode.J:
					parentObject = pathAStar;
					maze = _aStarMaze;
					isDisplayed = _aStarAlreadyDisplayed;
					_currentAlgorithm = "A*";
					break;
				
				case KeyCode.K:
					parentObject = pathBellmanFord;
					maze = _bellmanFordMaze;
					isDisplayed = _bellmanFordAlreadyDisplayed;
					_currentAlgorithm = "Bellman-Ford";
					break;
				
				default:
					_currentAlgorithm = Empty;
					return;
			}

			Debug.Log($"Current Algorithm = {_currentAlgorithm}");

			ChangeParentOfObjects(isDisplayed ? floorObject : parentObject, maze);

			parentObject.gameObject.SetActive(!isDisplayed);
		}

		private int FindTotalVisitedNodes(List<MazeCell> mazeList)
		{
			var count = 0;
			
			foreach (var node in mazeList.Where(node => node.Visited))
			{
				count++;
			}

			return count;
		}

		private (List<MazeCell> mazeList, long timeTaken) ExecuteAlgorithmAndFindTimeTaken(int algorithmToExecute)
		{
			Stopwatch sw = new();
			long timeTaken;
			List<MazeCell> mazeList;

			switch (algorithmToExecute)
			{
				case 1:
					sw.Start();
					mazeList = Dijkstra.Algorithm(_sortedMaze);
					sw.Stop();
					timeTaken = sw.ElapsedMilliseconds;
					break;
				
				case 2:
					sw.Start();
					mazeList = AStar.Algorithm(_sortedMaze);
					sw.Stop();
					timeTaken = sw.ElapsedMilliseconds;
					break;
				
				case 3:
					sw.Start();
					mazeList = BellmanFord.Algorithm(_sortedMaze);
					sw.Stop();
					timeTaken = sw.ElapsedMilliseconds;
					break;
				
				default:
					throw new ArgumentException();
			}
			
			sw.Reset();
			
			return (mazeList, timeTaken);
		}
	}
}
