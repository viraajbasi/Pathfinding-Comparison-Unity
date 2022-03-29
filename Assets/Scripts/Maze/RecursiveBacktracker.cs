using System.Collections.Generic;
using Random = System.Random;

namespace Maze
{
	public static class RecursiveBacktracker
	{
		public static List<MazeCell> Algorithm(List<MazeCell> mazeList, int width, int height)
		{
			var rng = new Random();
			var positionStack = new Stack<Position>();
			var randomPosition = new Position(rng.Next(0, width), rng.Next(0, height));
			var randomPositionIndex = mazeList.FindIndex(a => a.Coordinates.X == randomPosition.X && a.Coordinates.Y == randomPosition.Y);

			mazeList[randomPositionIndex].Visited = true;
			positionStack.Push(randomPosition);

			while (positionStack.Count > 0)
			{
				var currentPosition = positionStack.Pop();
				var neighbourList = GetUnvisitedNeighbours(currentPosition, mazeList, width, height);

				if (neighbourList.Count > 0)
				{
					positionStack.Push(currentPosition);
					var randomNeighbour = neighbourList[rng.Next(0, neighbourList.Count)];
					var currentNode = mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y);
					var neighbourPosition = randomNeighbour.Coordinates;

					switch (randomNeighbour.SharedWall)
					{
						case SharedWall.Top:
							currentNode.Top = false;
							randomNeighbour.Bottom = false;
							break;

						case SharedWall.Bottom:
							currentNode.Bottom = false;
							randomNeighbour.Top = false;
							break;

						case SharedWall.Left:
							currentNode.Left = false;
							randomNeighbour.Right = false;
							break;

						case SharedWall.Right:
							currentNode.Right = false;
							randomNeighbour.Left = false;
							break;
					}
					
					randomNeighbour.Visited = true;
					positionStack.Push(neighbourPosition);
				}
			}

			return mazeList;
		}

		private static List<MazeCell> GetUnvisitedNeighbours(Position currentPosition, List<MazeCell> mazeList, int width, int height)
		{
			var list = new List<MazeCell>();
			var topNode = mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y + 1);
			var leftNode = mazeList.Find(a => a.Coordinates.X == currentPosition.X - 1 && a.Coordinates.Y == currentPosition.Y);
			var rightNode = mazeList.Find(a => a.Coordinates.X == currentPosition.X + 1 && a.Coordinates.Y == currentPosition.Y);
			var bottomNode = mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y - 1);

			if (currentPosition.X > 0) // Left Wall
			{
				if (!leftNode.Visited)
				{
					leftNode.SharedWall = SharedWall.Left;
					list.Add(leftNode);
				}
			}

			if (currentPosition.Y > 0) // Bottom Wall
			{
				if (!bottomNode.Visited)
				{
					bottomNode.SharedWall = SharedWall.Bottom;
					list.Add(bottomNode);
				}
			}

			if (currentPosition.Y < height - 1) // Top Wall
			{
				if (!topNode.Visited)
				{
					topNode.SharedWall = SharedWall.Top;
					list.Add(topNode);
				}
			}

			if (currentPosition.X < width - 1) // Right Wall
			{
				if (!rightNode.Visited)
				{
					rightNode.SharedWall = SharedWall.Right;
					list.Add(rightNode);
				}
			}

			return list;
		}
	}
}
