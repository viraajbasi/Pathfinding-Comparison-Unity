using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using static System.String;

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
		public Material defaultFloorMaterial;
		public GameObject informationPanel;
		public GameObject fileSavedScreen;
		public TMP_Text fileSavedText;
		public GameObject userSolvesNodePanel;
		public TMP_Text userSolvesNodes;
		public AudioSource audioSource;

		private const float Offset = 0.5f;

		private float AverageTimeTaken => TotalTimeTaken / 3f;
		private long TotalTimeTaken => _dijkstraTimeTaken + _aStarTimeTaken + _bellmanFordTimeTaken;
		
		private int _width;
		private int _height;
		private List<MazeCell> _sortedMaze;

		private int _totalNodes;
		private string _currentAlgorithm;
		
		private List<MazeCell> _dijkstraMaze;
		private bool _dijkstraAlreadyDisplayed = true;
		private long _dijkstraTimeTaken;
		private int _dijkstraNodesVisited;
		private int _dijkstraNodesInPath;
		private bool _dijkstraScreenshotTaken;
		
		private List<MazeCell> _aStarMaze;
		private bool _aStarAlreadyDisplayed = true;
		private long _aStarTimeTaken;
		private int _aStarNodesVisited;
		private int _aStarNodesInPath;
		private bool _aStarScreenshotTaken;
		
		private List<MazeCell> _bellmanFordMaze;
		private bool _bellmanFordAlreadyDisplayed = true;
		private long _bellmanFordTimeTaken;
		private int _bellmanFordNodesVisited;
		private int _bellmanFordNodesInPath;
		private bool _bellmanFordScreenshotTaken;

		private void Start()
		{
			Application.targetFrameRate = -1;
			Time.timeScale = 1f;
			PlayerPrefs.DeleteKey("DijkstraTotalVisited");
			PlayerPrefs.DeleteKey("A*TotalVisited");
			PlayerPrefs.DeleteKey("BellmanFordTotalVisited");
			PlayerPrefs.SetString("FileName", DateTime.Now.ToString("HH.mm-yyyy_MM_dd"));

			_width = PlayerPrefs.GetInt("Width");
			_height = PlayerPrefs.GetInt("Height");

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

			if (PlayerPrefs.GetInt("Pathfinding") == 1)
			{
				var fileExtension = PlayerPrefs.GetString("FileName");
				ScreenCapture.CaptureScreenshot($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/Maze-{fileExtension}.png");
				
				// DIJKSTRA
				var (dijkstraMaze, dijkstraTime) = ExecuteAlgorithmAndFindTimeTaken(1);
				_dijkstraMaze = dijkstraMaze;
				_dijkstraTimeTaken = dijkstraTime;

				pathDijkstra.gameObject.SetActive(false);
				
				_dijkstraNodesVisited = PlayerPrefs.GetInt("DijkstraTotalVisited");

				_dijkstraNodesInPath = MazeCell.GetPathNodeCount(_dijkstraMaze);

				// A*
				var (aStarMaze, aStarTime) = ExecuteAlgorithmAndFindTimeTaken(2);
				_aStarMaze = aStarMaze;
				_aStarTimeTaken = aStarTime;

				pathAStar.gameObject.SetActive(false);

				_aStarNodesVisited = PlayerPrefs.GetInt("A*TotalVisited");

				_aStarNodesInPath = MazeCell.GetPathNodeCount(_aStarMaze);

				// BELLMAN-FORD
				var (bellmanFordMaze, bellmanFordTime) = ExecuteAlgorithmAndFindTimeTaken(3);
				_bellmanFordMaze = bellmanFordMaze;
				_bellmanFordTimeTaken = bellmanFordTime;

				pathBellmanFord.gameObject.SetActive(false);
				
				_bellmanFordNodesVisited = PlayerPrefs.GetInt("BellmanFordTotalVisited");

				_bellmanFordNodesInPath = MazeCell.GetPathNodeCount(_bellmanFordMaze);

				// Stats
				print($"Dijkstra Time to Execute = {_dijkstraTimeTaken}ms");
				print($"Dijkstra Total Visited Nodes = {_dijkstraNodesVisited}");
				print($"Dijkstra Total Path Nodes = {_dijkstraNodesInPath}");
				print($"A* Time to Execute = {_aStarTimeTaken}ms");
				print($"A* Total Visited Nodes = {_aStarNodesVisited}");
				print($"A* Total Path Nodes = {_aStarNodesInPath}");
				print($"Bellman-Ford Time to Execute = {_bellmanFordTimeTaken}ms");
				print($"Bellman-Ford Visited Nodes = {_bellmanFordNodesVisited}");
				print($"Bellman-Ford Total Path Nodes = {_bellmanFordNodesInPath}");
				print($"Total Nodes = {_totalNodes}");
				print($"Total Time = {TotalTimeTaken}ms");
				print($"Average Time = {AverageTimeTaken}ms");
				print(Application.persistentDataPath);
				print(Application.dataPath);
				print(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
			}
			
			MeshCombiner.MazeRendered = true;

			if (PlayerPrefs.GetInt("UserSolves") == 1)
			{
				_sortedMaze.Find(a => a.StartNode).Floor.gameObject.GetComponent<Renderer>().material.color = Color.white;
				userSolvesNodePanel.SetActive(true);
				userSolvesNodes.text = $"Total Visited Nodes: {PlayerPrefs.GetInt("0")}";
			}
		}

		private void Update()
		{
			if (PlayerPrefs.GetInt("UserSolves") == 1 && Time.timeScale > 0)
			{
				var defaultFloorColour = defaultFloorMaterial.color;
				UserSolves.HandleKeyInput(_sortedMaze, defaultFloorColour, audioSource, completedScreen, userSolvesNodes);
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
						PathfindingDisplay(vKey);
					}
				}

				if (Input.GetKeyUp(KeyCode.L))
				{
					var dijkstraList = new List<string>()
					{
						$"Time to Execute: {_dijkstraTimeTaken}ms",
						$"Total Visited Nodes: {_dijkstraNodesVisited}",
						$"Total Nodes in Path: {_dijkstraNodesInPath}"
					};
					
					var aStarList = new List<string>()
					{
						$"Time to Execute: {_aStarTimeTaken}ms",
						$"Total Visited Nodes: {_aStarNodesVisited}",
						$"Total Nodes in Path: {_aStarNodesInPath}"
					};
					
					var bellmanFordList = new List<string>()
					{
						$"Time to Execute: {_bellmanFordTimeTaken}ms",
						$"Total Visited Nodes: {_bellmanFordNodesVisited}",
						$"Total Nodes in Path: {_bellmanFordNodesInPath}"
					};

					var generalList = new List<string>()
					{
						$"Total Nodes: {_totalNodes}",
						$"Total Time: {TotalTimeTaken}ms",
						$"Average Time: {AverageTimeTaken}ms"
					};
					
					SaveScript.SaveStatsToFile(dijkstraList, aStarList, bellmanFordList, generalList);

					Time.timeScale = 0f;
					var fileLocation = PlayerPrefs.GetString("FileLocation");
					fileSavedText.text = $"The statistics file was saved to '{fileLocation}'";
					fileSavedScreen.SetActive(true);
				}
			}
		}

		private List<MazeCell> GenerateRandomMaze(int width, int height)
		{
			var mazeList = new List<MazeCell>();

			for (var i = 0; i < width; i++)
			{
				for (var j = 0; j < height; j++)
				{
					mazeList.Add(new MazeCell(true, true, true, true, false, i, j));
				}
			}

			mazeList.Find(a => a.Coordinates.X == 0 && a.Coordinates.Y == 0).StartNode = true;
			mazeList.Find(a => a.Coordinates.X == width - 1 && a.Coordinates.Y == height - 1).GoalNode = true;

			return RecursiveBacktracker.Algorithm(mazeList, width, height);
		}

		private void DrawMaze(List<MazeCell> mazeList)
		{
			var topOffset = new Vector3(0, 0, Offset);
			var leftOffset = new Vector3(-Offset, 0, 0);
			var rightOffset = new Vector3(Offset, 0, 0);
			var bottomOffset = new Vector3(0, 0, -Offset);
			var yOffset = new Vector3(0, Offset, 0);
			var wallRotation = Quaternion.Euler(0, 90, 0);

			for (var i = 0; i < _width; i++)
			{
				for (var j = 0; j < _height; j++)
				{
					var currentIndex = mazeList.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					var pos = new Vector3(-_width / 2 + i, 0, -_height / 2 + j);

					if (mazeList[currentIndex].StartNode)
					{
						var mazeNode = Instantiate(startEndPrefab, pos + yOffset, Quaternion.identity,transform);
						mazeNode.name = $"Node (Start) ({i},{j})";
						mazeNode.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
					}
					else if (mazeList[currentIndex].GoalNode)
					{
						var mazeNode = Instantiate(startEndPrefab, pos + yOffset, Quaternion.identity,transform);
						mazeNode.name = $"Node (Goal) ({i},{j})";
						mazeNode.GetComponent<Renderer>().material.color = new Color(102, 190, 0);
					}

					mazeList[currentIndex].Floor = Instantiate(floorPrefab, pos, Quaternion.identity, floorObject);
					mazeList[currentIndex].Floor.gameObject.name = $"Node ({i},{j}) Floor";

					if (mazeList[currentIndex].Top)
					{
						var topWall = Instantiate(wallPrefab, pos + topOffset, Quaternion.identity, wallObject);
						topWall.name = $"Node ({i},{j}) Top Wall";
					}

					if (mazeList[currentIndex].Left)
					{
						var leftWall = Instantiate(wallPrefab, pos + leftOffset, wallRotation, wallObject);
						leftWall.name = $"Node ({i},{j}) Left Wall";
					}

					if (i == _width - 1)
					{
						if (mazeList[currentIndex].Right)
						{
							var rightWall = Instantiate(wallPrefab, pos + rightOffset, wallRotation, wallObject);
							rightWall.name = $"Node ({i},{j}) Right Wall";
						}
					}

					if (j == 0)
					{
						if (mazeList[currentIndex].Bottom)
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

		private void PathfindingDisplay(KeyCode key)
		{
			Transform parentObject;
			List<MazeCell> maze;
			bool isDisplayed;
			string totalVisitedNodes;
			string totalPathNodes;
			string algorithmTimeTaken;
			var fileName = PlayerPrefs.GetString("FileName");

			switch (key)
			{
				case KeyCode.H:
					parentObject = pathDijkstra;
					maze = _dijkstraMaze;
					isDisplayed = _dijkstraAlreadyDisplayed;
					_currentAlgorithm = "Dijkstra";
					totalVisitedNodes = _dijkstraNodesVisited.ToString();
					totalPathNodes = _dijkstraNodesInPath.ToString();
					algorithmTimeTaken = _dijkstraTimeTaken.ToString();
					InformationPanel.UpdateLabels(_currentAlgorithm, _totalNodes.ToString(), totalVisitedNodes, totalPathNodes, algorithmTimeTaken, TotalTimeTaken.ToString(), AverageTimeTaken.ToString());
					break;
				
				case KeyCode.J:
					parentObject = pathAStar;
					maze = _aStarMaze;
					isDisplayed = _aStarAlreadyDisplayed;
					_currentAlgorithm = "A*";
					totalVisitedNodes = _aStarNodesVisited.ToString();
					totalPathNodes = _aStarNodesInPath.ToString();
					algorithmTimeTaken = _aStarTimeTaken.ToString();
					InformationPanel.UpdateLabels(_currentAlgorithm, _totalNodes.ToString(), totalVisitedNodes, totalPathNodes, algorithmTimeTaken, TotalTimeTaken.ToString(), AverageTimeTaken.ToString());
					break;
				
				case KeyCode.K:
					parentObject = pathBellmanFord;
					maze = _bellmanFordMaze;
					isDisplayed = _bellmanFordAlreadyDisplayed;
					_currentAlgorithm = "Bellman-Ford";
					totalVisitedNodes = _bellmanFordNodesVisited.ToString();
					totalPathNodes = _bellmanFordNodesInPath.ToString();
					algorithmTimeTaken = _bellmanFordTimeTaken.ToString();
					InformationPanel.UpdateLabels(_currentAlgorithm, _totalNodes.ToString(), totalVisitedNodes, totalPathNodes, algorithmTimeTaken, TotalTimeTaken.ToString(), AverageTimeTaken.ToString());
					break;
				
				default:
					_currentAlgorithm = Empty;
					return;
			}

			print($"Current Algorithm = {_currentAlgorithm}");

			ChangeParentOfObjects(isDisplayed ? floorObject : parentObject, maze);

			parentObject.gameObject.SetActive(!isDisplayed);
			informationPanel.SetActive(!isDisplayed);

			if (!_dijkstraScreenshotTaken && _currentAlgorithm == "Dijkstra")
			{
				StartCoroutine(TakeScreenshot(_currentAlgorithm, fileName));
				_dijkstraScreenshotTaken = true;
			}

			if (!_aStarScreenshotTaken && _currentAlgorithm == "A*")
			{
				StartCoroutine(TakeScreenshot("AStar", fileName));
				/*
				 * The '*' character cannot be in file names on Windows.
				 */
				_aStarScreenshotTaken = true;
			}

			if (!_bellmanFordScreenshotTaken && _currentAlgorithm == "Bellman-Ford")
			{
				StartCoroutine(TakeScreenshot(_currentAlgorithm, fileName));
				_bellmanFordScreenshotTaken = true;
			}
		}

		private IEnumerator TakeScreenshot(string currentAlgorithm, string fileExtension)
		{
			yield return new WaitForSeconds(1);
			ScreenCapture.CaptureScreenshot($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/{currentAlgorithm}-{fileExtension}.png");
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
