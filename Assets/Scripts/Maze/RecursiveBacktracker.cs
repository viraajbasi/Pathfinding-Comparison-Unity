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

			maze[maze.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y)].Visited = true;
			positionStack.Push(position);

			while (positionStack.Count > 0)
			{
				var current = positionStack.Pop();
				var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

				if (neighbours.Count > 0)
				{
					positionStack.Push(current);
					var rndNeigbour = neighbours[Random.Range(0, neighbours.Count)];
					var neighbourPosition = rndNeigbour.Coordinates;

					switch (rndNeigbour.Wall)
					{
						case SharedWall.Top:
							maze[maze.FindIndex(a => a.Coordinates.X == current.X && a.Coordinates.Y == current.Y)].Top = false;
							break;
						case SharedWall.Bottom:
							maze[maze.FindIndex(a => a.Coordinates.X == current.X && a.Coordinates.Y == current.Y)].Bottom = false;
							break;
						case SharedWall.Left:
							maze[maze.FindIndex(a => a.Coordinates.X == current.X && a.Coordinates.Y == current.Y)].Left = false;
							break;
						case SharedWall.Right:
							maze[maze.FindIndex(a => a.Coordinates.X == current.X && a.Coordinates.Y == current.Y)].Right = false;
							break;
					}

					switch (rndNeigbour.Wall)
					{
						case SharedWall.Top:
							maze[maze.FindIndex(a => a.Coordinates.X == neighbourPosition.X && a.Coordinates.Y == neighbourPosition.Y)].Bottom = false;
							break;
						case SharedWall.Bottom:
							maze[maze.FindIndex(a => a.Coordinates.X == neighbourPosition.X && a.Coordinates.Y == neighbourPosition.Y)].Top = false;
							break;
						case SharedWall.Left:
							maze[maze.FindIndex(a => a.Coordinates.X == neighbourPosition.X && a.Coordinates.Y == neighbourPosition.Y)].Right = false;
							break;
						case SharedWall.Right:
							maze[maze.FindIndex(a => a.Coordinates.X == neighbourPosition.X && a.Coordinates.Y == neighbourPosition.Y)].Left = false;
							break;
					}

					maze[maze.FindIndex(a => a.Coordinates.X == neighbourPosition.X && a.Coordinates.Y == neighbourPosition.Y)].Visited = true;
					positionStack.Push(neighbourPosition);
				}
			}

			return maze;
		}

		private static List<Neighbour> GetUnvisitedNeighbours(Position position, List<MazeCell> maze, int width, int height)
		{
			var list = new List<Neighbour>();

			if (position.X > 0) // Left Wall
			{
				if (!maze[maze.FindIndex(a => a.Coordinates.X == position.X - 1 && a.Coordinates.Y == position.Y)].Visited)
				{
					list.Add(new Neighbour(position.X - 1, position.Y, SharedWall.Left));
				}
			}

			if (position.Y > 0) // Bottom Wall
			{
				if (!maze[maze.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y - 1)].Visited)
				{
					list.Add(new Neighbour(position.X, position.Y - 1, SharedWall.Bottom));
				}
			}

			if (position.Y < height - 1) // Top Wall
			{
				if (!maze[maze.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y + 1)].Visited)
				{
					list.Add(new Neighbour(position.X, position.Y + 1, SharedWall.Top));
				}
			}

			if (position.X < width - 1) // Right Wall
			{
				if (!maze[maze.FindIndex(a => a.Coordinates.X == position.X + 1 && a.Coordinates.Y == position.Y)].Visited)
				{
					list.Add(new Neighbour(position.X + 1, position.Y, SharedWall.Right));
				}
			}

			return list;
		}
	}
}
