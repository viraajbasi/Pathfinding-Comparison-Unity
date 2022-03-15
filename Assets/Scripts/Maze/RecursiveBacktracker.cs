using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Maze
{
	public static class RecursiveBacktracker
	{
		public static List<MazeCell> Algorithm(List<MazeCell> maze, int width, int height)
		{
			var positionStack = new Stack<Position>();
			var position = new Position(Random.Range(0, width), Random.Range(0, height));
			var positionIndex = maze.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y);

			maze[positionIndex].Visited = true;
			positionStack.Push(position);

			while (positionStack.Count > 0)
			{
				var current = positionStack.Pop();
				var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

				if (neighbours.Count > 0)
				{
					positionStack.Push(current);
					var rndNeighbour = neighbours[Random.Range(0, neighbours.Count)];
					var neighbourPosition = rndNeighbour.Coordinates;
					var currentIndex = maze.FindIndex(a => a.Coordinates.X == current.X && a.Coordinates.Y == current.Y);
					var neighbourIndex = maze.FindIndex(a => a.Coordinates.X == neighbourPosition.X && a.Coordinates.Y == neighbourPosition.Y);

					switch (rndNeighbour.Wall)
					{
						case SharedWall.Top:
							maze[currentIndex].Top = false;
							maze[neighbourIndex].Bottom = false;
							break;

						case SharedWall.Bottom:
							maze[currentIndex].Bottom = false;
							maze[neighbourIndex].Top = false;
							break;

						case SharedWall.Left:
							maze[currentIndex].Left = false;
							maze[neighbourIndex].Right = false;
							break;

						case SharedWall.Right:
							maze[currentIndex].Right = false;
							maze[neighbourIndex].Left = false;
							break;
					}

					maze[neighbourIndex].Visited = true;
					positionStack.Push(neighbourPosition);
				}
			}

			return maze;
		}

		private static List<Neighbour> GetUnvisitedNeighbours(Position position, List<MazeCell> maze, int width, int height)
		{
			var list = new List<Neighbour>();
			var topWallIndex = maze.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y + 1);
			var leftWallIndex = maze.FindIndex(a => a.Coordinates.X == position.X - 1 && a.Coordinates.Y == position.Y);
			var rightWallIndex = maze.FindIndex(a => a.Coordinates.X == position.X + 1 && a.Coordinates.Y == position.Y);
			var bottomWallIndex = maze.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y - 1);

			if (position.X > 0) // Left Wall
			{
				if (!maze[leftWallIndex].Visited)
				{
					list.Add(new Neighbour(position.X - 1, position.Y, SharedWall.Left));
				}
			}

			if (position.Y > 0) // Bottom Wall
			{
				if (!maze[bottomWallIndex].Visited)
				{
					list.Add(new Neighbour(position.X, position.Y - 1, SharedWall.Bottom));
				}
			}

			if (position.Y < height - 1) // Top Wall
			{
				if (!maze[topWallIndex].Visited)
				{
					list.Add(new Neighbour(position.X, position.Y + 1, SharedWall.Top));
				}
			}

			if (position.X < width - 1) // Right Wall
			{
				if (!maze[rightWallIndex].Visited)
				{
					list.Add(new Neighbour(position.X + 1, position.Y, SharedWall.Right));
				}
			}

			return list;
		}
	}
}
