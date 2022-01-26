using System.Collections.Generic;
using System;

/*[Flags]
public enum WallState
{
	LEFT = 1, // 0001
	RIGHT = 2, // 0010
	UP = 4, // 0100
	DOWN = 8, // 1000
	VISITED = 128, // 1000 0000
}*/

public class Position
{
	public int X;
	public int Y;
}

public struct Neighbour
{
	public Position Position;
	public SharedWall Wall;
	//public WallState SharedWall;
	//public SharedWallState SharedWall;
}

public enum SharedWall
{
	Top,
	Bottom,
	Left,
	Right
}

/*public class SharedWallState
{
	public bool TopShared;
	public bool BottomShared;
	public bool LeftShared;
	public bool RightShared;
}*/

public struct WallStateBool
{
	public bool Top;
	public bool Bottom;
	public bool Left;
	public bool Right;
	public bool Visited;
	//public SharedWall wall;
}

public static class MazeGenerator
{
	//public WallState SharedWall;
	/*private static WallState GetOppositeWall(WallStateBool wall)
	{
		switch (wall)
		{
			case WallState.RIGHT: return WallState.LEFT;
			case WallState.LEFT: return WallState.RIGHT;
			case WallState.UP: return WallState.DOWN;
			case WallState.DOWN: return WallState.UP;
			default: return WallState.LEFT;
		}
	}*/

	private static WallStateBool[,] RecursiveBacktracker(WallStateBool[,] maze, int width, int height)
	{
		var rng = new Random();
		var posStack = new Stack<Position>();
		var pos = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };
		
		//maze[pos.X, pos.Y] |= WallState.VISITED; // 1000 1111
		maze[pos.X, pos.Y].Visited = true;
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

				/*maze[current.X, current.Y] &= ~rndNeigbour.SharedWall;
				maze[nPos.X, nPos.Y] &= ~GetOppositeWall(rndNeigbour.SharedWall);
				maze[nPos.X, nPos.Y] |= WallState.VISITED;*/

				switch (rndNeigbour.Wall)
				{
					// Determine the shared wall from current cell and remove it.
					case SharedWall.Top:
						maze[current.X, current.Y].Top = false;
						break;
					case SharedWall.Bottom:
						maze[current.X, current.Y].Bottom = false;
						break;
					case SharedWall.Left:
						maze[current.X, current.Y].Left = false;
						break;
					case SharedWall.Right:
						maze[current.X, current.Y].Right = false;
						break;
				}
				
				switch (rndNeigbour.Wall)
				{
					// Determine the shared wall from neighbouring cell and remove it .
					case SharedWall.Top:
						maze[nPos.X, nPos.Y].Bottom = false;
						break;
					case SharedWall.Bottom:
						maze[nPos.X, nPos.Y].Top = false;
						break;
					case SharedWall.Left:
						maze[nPos.X, nPos.Y].Right = false;
						break;
					case SharedWall.Right:
						maze[nPos.X, nPos.Y].Left = false;
						break;
				}

				posStack.Push(nPos);
			}
		}

		return maze;
	}

	private static WallStateBool[,] Kruskal(WallStateBool[,] maze, int width, int height)
	{
		return maze;
	}

	private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallStateBool[,] maze, int width, int height)
	{
		var list = new List<Neighbour>();

		if (p.X > 0) // Left Wall
		{
			if (/*!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED)*/ !maze[p.X - 1, p.Y].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X - 1,
								Y = p.Y,
							},
							//SharedWall = WallState.LEFT
							Wall = SharedWall.Left
						});
			}
		}

		if (p.Y > 0) // Bottom Wall
		{
			if (/*!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED)*/ !maze[p.X, p.Y - 1].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X,
								Y = p.Y - 1
							},
							//SharedWall = WallState.DOWN
							Wall = SharedWall.Bottom
						});
			}
		}

		if (p.Y < height - 1) // Top Wall
		{
			if (/*!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED)*/ !maze[p.X, p.Y + 1].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X,
								Y = p.Y + 1
							},
							//SharedWall = WallState.UP
							Wall = SharedWall.Top
						});
			}
		}

		if (p.X < width - 1) // Right Wall
		{
			if (/*!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED)*/ !maze[p.X + 1, p.Y].Visited)
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X + 1,
								Y = p.Y
							},
							//SharedWall = WallState.RIGHT
							Wall = SharedWall.Right
						});
			}
		}

		return list;
	}

	public static WallStateBool[,] Generate(int width, int height)
	{
		WallStateBool[,] maze = new WallStateBool[width, height];
		//WallState initial = WallState.RIGHT | WallState.LEFT | WallState.DOWN | WallState.UP; // Add wall states using pipe 
		
		// Provide each cell with the initial wall state
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				maze[i, j].Top = true;
				maze[i, j].Bottom = true;
				maze[i, j].Left = true;
				maze[i, j].Right = true;
			}
		}

		return RecursiveBacktracker(maze, width, height);
	}
}
