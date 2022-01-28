using System.Collections.Generic;

public static class RecursiveBacktracker
{
    public static List<WallStateBool> Algorithm(List<WallStateBool> maze, int width, int height)
	{
		var rng = new System.Random();
		var posStack = new Stack<Position>();
		var pos = new Position
		{
			X = rng.Next(0, width),
			Y = rng.Next(0, height)
		};
		
		maze[maze.FindIndex(a => a.Coordinates.X == pos.X && a.Coordinates.Y == pos.Y)].Visited = true;
		posStack.Push(pos);

		while (posStack.Count > 0)
		{
			var current = posStack.Pop();
			var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

			if (neighbours.Count > 0)
			{
				posStack.Push(current);
				var rndNeigbour = neighbours[rng.Next(0, neighbours.Count)];
				var nPos = rndNeigbour.Coordinates;

				switch (rndNeigbour.Wall)
				{
					// Determine the shared wall from current cell and remove it.
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
					// Determine the shared wall from neighbouring cell and remove it .
					case SharedWall.Top:
						maze[maze.FindIndex(a => a.Coordinates.X == nPos.X && a.Coordinates.Y == nPos.Y)].Bottom = false;
						break;
					case SharedWall.Bottom:
						maze[maze.FindIndex(a => a.Coordinates.X == nPos.X && a.Coordinates.Y == nPos.Y)].Top = false;
						break;
					case SharedWall.Left:
						maze[maze.FindIndex(a => a.Coordinates.X == nPos.X && a.Coordinates.Y == nPos.Y)].Right = false;
						break;
					case SharedWall.Right:
						maze[maze.FindIndex(a => a.Coordinates.X == nPos.X && a.Coordinates.Y == nPos.Y)].Left = false;
						break;
				}

				maze[maze.FindIndex(a => a.Coordinates.X == nPos.X && a.Coordinates.Y == nPos.Y)].Visited = true;
				posStack.Push(nPos);
			}
		}
		
		return maze;
	}

	private static List<Neighbour> GetUnvisitedNeighbours(Position p, List<WallStateBool> maze, int width, int height)
	{
		var list = new List<Neighbour>();

		if (p.X > 0) // Left Wall
		{
			if (!maze[maze.FindIndex(a => a.Coordinates.X == p.X - 1 && a.Coordinates.Y == p.Y)].Visited)
			{
				list.Add(new Neighbour
						{
							Coordinates = new Position
							{
								X = p.X - 1,
								Y = p.Y,
							},
							Wall = SharedWall.Left
						});
			}
		}

		if (p.Y > 0) // Bottom Wall
		{
			if (!maze[maze.FindIndex(a => a.Coordinates.X == p.X && a.Coordinates.Y == p.Y - 1)].Visited)
			{
				list.Add(new Neighbour
						{
							Coordinates = new Position
							{
								X = p.X,
								Y = p.Y - 1
							},
							Wall = SharedWall.Bottom
						});
			}
		}

		if (p.Y < height - 1) // Top Wall
		{
			if (!maze[maze.FindIndex(a => a.Coordinates.X == p.X && a.Coordinates.Y == p.Y + 1)].Visited)
			{
				list.Add(new Neighbour
						{
							Coordinates = new Position
							{
								X = p.X,
								Y = p.Y + 1
							},
							Wall = SharedWall.Top
						});
			}
		}

		if (p.X < width - 1) // Right Wall
		{
			if (!maze[maze.FindIndex(a => a.Coordinates.X == p.X + 1 && a.Coordinates.Y == p.Y)].Visited)
			{
				list.Add(new Neighbour
						{
							Coordinates = new Position
							{
								X = p.X + 1,
								Y = p.Y
							},
							Wall = SharedWall.Right
						});
			}
		}

		return list;
	}
}
