using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Position
{
	public int X;
	public int Y;
}

public struct Neighbour
{
	public Position Position;
	public SharedWall Wall;
}

public enum SharedWall
{
	Top,
	Bottom,
	Left,
	Right
}

public class WallStateBool
{
	public bool Top { get; set; }
	public bool Bottom { get; set; }
	public bool Left { get; set; }
	public bool Right { get; set; }
	public bool Visited { get; set; }
	public int X { get; set; }
	public int Y { get; set; }
}

public static class MazeGenerator
{
	private static List<WallStateBool> RecursiveBacktracker(List<WallStateBool> maze, int width, int height)
	{
		var rng = new Random();
		var posStack = new Stack<Position>();
		var pos = new Position
		{
			X = rng.Next(0, width),
			Y = rng.Next(0, height)
		};
		
		maze[maze.FindIndex(a => a.X == pos.X && a.Y == pos.Y)].Visited = true;
		posStack.Push(pos);

		while (posStack.Count > 0)
		{
			var current = posStack.Pop();
			var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

			if (neighbours.Count > 0)
			{
				posStack.Push(current);

				var rndIndex = rng.Next(0, neighbours.Count);
				var rndNeigbour = neighbours[rndIndex];
				var nPos = rndNeigbour.Position;

				switch (rndNeigbour.Wall)
				{
					// Determine the shared wall from current cell and remove it.
					case SharedWall.Top:
						maze[maze.FindIndex(a => a.X == current.X && a.Y == current.Y)].Top = false;
						break;
					case SharedWall.Bottom:
						maze[maze.FindIndex(a => a.X == current.X && a.Y == current.Y)].Bottom = false;
						break;
					case SharedWall.Left:
						maze[maze.FindIndex(a => a.X == current.X && a.Y == current.Y)].Left = false;
						break;
					case SharedWall.Right:
						maze[maze.FindIndex(a => a.X == current.X && a.Y == current.Y)].Right = false;
						break;
				}
				
				switch (rndNeigbour.Wall)
				{
					// Determine the shared wall from neighbouring cell and remove it .
					case SharedWall.Top:
						maze[maze.FindIndex(a => a.X == nPos.X && a.Y == nPos.Y)].Bottom = false;
						break;
					case SharedWall.Bottom:
						maze[maze.FindIndex(a => a.X == nPos.X && a.Y == nPos.Y)].Top = false;
						break;
					case SharedWall.Left:
						maze[maze.FindIndex(a => a.X == nPos.X && a.Y == nPos.Y)].Right = false;
						break;
					case SharedWall.Right:
						maze[maze.FindIndex(a => a.X == nPos.X && a.Y == nPos.Y)].Left = false;
						break;
				}

				posStack.Push(nPos);
			}
		}
		
		Debug.Log("Reached end.");
		return maze;
	}

	private static List<Neighbour> GetUnvisitedNeighbours(Position p, List<WallStateBool> maze, int width, int height)
	{
		var list = new List<Neighbour>();

		if (p.X > 0) // Left Wall
		{
			if (!maze[maze.FindIndex(a => a.X == p.X - 1 && a.Y == p.Y)].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
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
			if (!maze[maze.FindIndex(a => a.X == p.X && a.Y == p.Y - 1)].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
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
			if (!maze[maze.FindIndex(a => a.X == p.X && a.Y == p.Y + 1)].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
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
			if (!maze[maze.FindIndex(a => a.X == p.X + 1 && a.Y == p.Y)].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
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

	public static List<WallStateBool> Generate(int width, int height)
	{
		var maze = new List<WallStateBool>();
		
		// Provide each cell with the initial wall state
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				maze.Add(new WallStateBool
				{
					Top = true,
					Bottom = true,
					Left = true,
					Right = true,
					Visited = false,
					X = i,
					Y = j
				});
			}
		}

		return RecursiveBacktracker(maze, width, height);
		//return maze;
	}
}
