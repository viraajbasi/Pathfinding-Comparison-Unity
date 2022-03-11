using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
	public class Render : MonoBehaviour
	{
		public int width = 10;
		public int height = 10;
		public Transform wallPrefab;
		public Transform floorPrefab;
		public Transform mazeObjectPrefab;
		public static List<MazeCell> SortedMaze = new List<MazeCell>();
		private float _size = 0.5f;

		private void Start()
		{
			GameObject.Find("Main Camera").transform.position = new Vector3(0, width, 0);
			GameObject.Find("Main Camera").transform.eulerAngles = new Vector3(90, 0, 0);
			Draw(Generate(width, height));
		}

		private List<MazeCell> Generate(int w, int h)
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
			
			SortedMaze = PlayerPrefs.GetInt("Kruskal") == 1 ? Kruskal.Algorithm(maze, w, h) : RecursiveBacktracker.Algorithm(maze, w, h);
			return SortedMaze;
		}

		private void Draw(List<MazeCell> maze)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					var pos = new Vector3(-width / 2 + i, 0, -height / 2 + j);

					maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode = Instantiate(mazeObjectPrefab, transform);
					maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.name = $"Node ({i},{j})";
					maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.position = pos + new Vector3(0, _size, 0);
					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].StartNode)
					{
						maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.name = $"Node (Start) ({i},{j})";
						maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.GetComponent<MeshRenderer>().enabled = true;
						maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
					}
					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].GoalNode)
					{
						maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.name = $"Node (Goal) ({i},{j})";
						maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.GetComponent<MeshRenderer>().enabled = true;
						maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].MazeNode.GetComponent<Renderer>().material.color = new Color(102, 190, 0);
					}

					var floor = Instantiate(floorPrefab, transform);
					floor.name = $"Node ({i},{j}) Floor";
					floor.position = pos;
					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Cost < 0)
					{
						floor.GetComponent<Renderer>().material.color = new Color(0, 0, 179);
					}
					
					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Top)
					{
						var topWall = Instantiate(wallPrefab, transform);
						topWall.name = $"Node ({i},{j}) Top Wall";
						topWall.position = pos + new Vector3(0, 0, _size);
					}

					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Left)
					{
						var leftWall = Instantiate(wallPrefab, transform);
						leftWall.name = $"Node ({i},{j}) Left Wall";
						leftWall.position = pos + new Vector3(-_size, 0, 0);
						leftWall.eulerAngles = new Vector3(0, 90, 0);
					}

					if (i == width - 1)
					{
						if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Right)
						{
							var rightWall = Instantiate(wallPrefab, transform);
							rightWall.name = $"Node ({i},{j}) Right Wall";
							rightWall.position = pos + new Vector3(_size, 0, 0);
							rightWall.eulerAngles = new Vector3(0, 90, 0);
						}
					}

					if (j == 0)
					{
						if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Bottom)
						{
							var bottomWall = Instantiate(wallPrefab, transform);
							bottomWall.name = $"Node ({i},{j}) Bottom Wall";
							bottomWall.position = pos + new Vector3(0, 0, -_size);
						}
					}
				}
			}
		}

		private int ReturnCost()
		{
			if (Random.Range(-10, 10) < 0)
			{
				return -1;
			}

			return 1;
		}
	}
}
