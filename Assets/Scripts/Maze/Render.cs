using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
	public class Render : MonoBehaviour
	{
		public int width = 10;
		public int height = 10;
		public float size = 1f;
		public Transform wallPrefab;
		public Transform floorPrefab;
		public static List<MazeCell> SortedMaze = new List<MazeCell>();

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
						Cost = 1
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

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					var pos = new Vector3(-width / 2 + i, 0, -height / 2 + j);

					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Top)
					{
						var topWall = Instantiate(wallPrefab, transform);
						topWall.position = pos + new Vector3(0, 0, size / 2);
						var localScale = topWall.localScale;
						localScale = new Vector3(size, localScale.y, localScale.z);
						topWall.localScale = localScale;
					}

					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Left)
					{
						var leftWall = Instantiate(wallPrefab, transform);
						leftWall.position = pos + new Vector3(-size / 2, 0, 0);
						var localScale = leftWall.localScale;
						localScale = new Vector3(size, localScale.y, localScale.z);
						leftWall.localScale = localScale;
						leftWall.eulerAngles = new Vector3(0, 90, 0);
					}

					if (i == width - 1)
					{
						if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Right)
						{
							var rightWall = Instantiate(wallPrefab, transform);
							rightWall.position = pos + new Vector3(+size / 2, 0, 0);
							var localScale = rightWall.localScale;
							localScale = new Vector3(size, localScale.y, localScale.z);
							rightWall.localScale = localScale;
							rightWall.eulerAngles = new Vector3(0, 90, 0);
						}
					}

					if (j == 0)
					{
						if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Bottom)
						{
							var bottomWall = Instantiate(wallPrefab, transform);
							bottomWall.position = pos + new Vector3(0, 0, -size / 2);
							var localScale = bottomWall.localScale;
							localScale = new Vector3(size, localScale.y, localScale.z);
							bottomWall.localScale = localScale;
						}
					}
				}
			}
		}
	}
}
