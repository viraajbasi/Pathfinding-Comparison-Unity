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
			Draw(Generate(width, height));
		}

		private List<MazeCell> Generate(int w, int h)
		{
			var maze = new List<MazeCell>();

			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					maze.Add(new MazeCell
					{
						Top = true,
						Bottom = true,
						Left = true,
						Right = true,
						Visited = false,
						Coordinates = new Position
						{
							X = i,
							Y = j
						},
						Cost = 1,
						MazeNode = new GameObject()
					});
				}
			}

			maze[maze.FindIndex(a => a.Coordinates.X == 0 && a.Coordinates.Y == 0)].StartNode = true;
			maze[maze.FindIndex(a => a.Coordinates.X == w - 1 && a.Coordinates.Y == h - 1)].GoalNode = true;
			
			SortedMaze = PlayerPrefs.GetInt("Kruskal") == 1 ? Kruskal.Algorithm(maze, w, h) : RecursiveBacktracker.Algorithm(maze, w, h);
			return SortedMaze;
		}

		private void Draw(List<MazeCell> maze)
		{
			var floor = Instantiate(floorPrefab, transform);
			floor.localScale = new Vector3(width, 1, height);
			var position = floor.transform.position;
			floor.position = new Vector3(position.x - _size, 0, position.z - _size);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					var pos = new Vector3(-width / 2 + i, 0, -height / 2 + j);

					var mazeObject = Instantiate(mazeObjectPrefab, transform);
					mazeObject.localPosition = pos + new Vector3(0, -_size, 0);

					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Top)
					{
						var topWall = Instantiate(wallPrefab, transform);
						topWall.position = pos + new Vector3(0, 0, _size);
					}

					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Left)
					{
						var leftWall = Instantiate(wallPrefab, transform);
						leftWall.position = pos + new Vector3(-_size, 0, 0);
						leftWall.eulerAngles = new Vector3(0, 90, 0);
					}

					if (i == width - 1)
					{
						if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Right)
						{
							var rightWall = Instantiate(wallPrefab, transform);
							rightWall.position = pos + new Vector3(_size, 0, 0);
							rightWall.eulerAngles = new Vector3(0, 90, 0);
						}
					}

					if (j == 0)
					{
						if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Bottom)
						{
							var bottomWall = Instantiate(wallPrefab, transform);
							bottomWall.position = pos + new Vector3(0, 0, -_size);
						}
					}
				}
			}
		}
	}
}
