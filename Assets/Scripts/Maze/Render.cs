using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
	public class Render : MonoBehaviour
	{
		public Transform wallPrefab;
		public Transform floorPrefab;
		public Transform mazeObjectPrefab;
		public static List<MazeCell> SortedMaze;

		private int _width = 50;
		private int _height = 50;

		private void Start()
		{
			SortedMaze = GenerateRandomMaze(_width, _height);
			DrawMaze(SortedMaze);

			for (int i = 0; i < _width; i++)
			{
				for (int j = 0; j < _height; j++)
				{
					var currentIndex = SortedMaze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					SortedMaze[currentIndex].Visited = false;
				}
			}
			
			Debug.Log(PlayerPrefs.GetInt("UserSolves"));
		}

		private void Update()
		{
			if (PlayerPrefs.GetInt("UserSolves") == 1)
			{
				UserSolves.HandleKeyInput();
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
			var _size = 0.5f;
			var topOffset = new Vector3(0, 0, _size);
			var leftOffset = new Vector3(-_size, 0, 0);
			var rightOffset = new Vector3(_size, 0, 0);
			var bottomOffset = new Vector3(0, 0, -_size);

			for (int i = 0; i < _width; i++)
			{
				for (int j = 0; j < _height; j++)
				{
					var currentIndex = maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j);
					var pos = new Vector3(-_width / 2 + i, 0, -_height / 2 + j);

					maze[currentIndex].MazeNode = Instantiate(mazeObjectPrefab, pos + new Vector3(0, _size, 0), Quaternion.identity,transform);
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

					var floor = Instantiate(floorPrefab, pos, Quaternion.identity, transform);
					floor.name = $"Node ({i},{j}) Floor";
					if (maze[currentIndex].Cost < 0)
					{
						floor.GetComponent<Renderer>().material.color = new Color(0, 0, 179);
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
