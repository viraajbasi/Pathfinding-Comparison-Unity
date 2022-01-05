using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum WallState
{
	LEFT = 1, // 0001
	RIGHT = 2, // 0010
	UP = 4, // 0100
	DOWN = 8, // 1000
	VISITED = 128, // 1000 0000
}

public class Position
{
	public int X;
	public int Y;
}

public struct Neighbour
{
	public Position Position;
	public WallState SharedWall;
}

public static class MazeGenerator
{
	private static WallState GetOppositeWall(WallState wall)
	{
		switch (wall)
		{
			case WallState.RIGHT: return WallState.LEFT;
			case WallState.LEFT: return WallState.RIGHT;
			case WallState.UP: return WallState.DOWN;
			case WallState.DOWN: return WallState.UP;
			default: return WallState.LEFT;
		}
	}

	private static WallState[,] RecursiveBacktracker(WallState[,] maze, int width, int height)
	{
		var rng = new System.Random();
		var posStack = new Stack<Position>();
		var pos = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };
		
		maze[pos.X, pos.Y] |= WallState.VISITED; // 1000 1111
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

				maze[current.X, current.Y] &= ~rndNeigbour.SharedWall;
				maze[nPos.X, nPos.Y] &= ~GetOppositeWall(rndNeigbour.SharedWall);
				
				maze[nPos.X, nPos.Y] |= WallState.VISITED;

				posStack.Push(nPos);
			}
		}

		return maze;
	}

	private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
	{
		var list = new List<Neighbour>();
		
		if (p.X > 0) // left
		{
			if (!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED))
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X - 1,
								Y = p.Y
							},
							SharedWall = WallState.LEFT
						});
			}
		}

		if (p.Y > 0) // bottom
		{
			if (!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED))
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X,
								Y = p.Y - 1
							},
							SharedWall = WallState.DOWN
						});
			}
		}

		if (p.Y < height - 1) // up
		{
			if (!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED))
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X,
								Y = p.Y + 1
							},
							SharedWall = WallState.UP
						});
			}
		}

		if (p.X < width - 1) // right
		{
			if (!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED))
			{
				list.Add(new Neighbour
						{
							Position = new Position
							{
								X = p.X + 1,
								Y = p.Y
							},
							SharedWall = WallState.RIGHT
						});
			}
		}

		return list;
	}

	public static WallState[,] Generate(int width, int height)
	{
		WallState[,] maze = new WallState[width, height];
		WallState initial = WallState.RIGHT | WallState.LEFT | WallState.DOWN | WallState.UP; // Add wall states using pipe 
		
		// Provide each cell with the initial wall state
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				maze[i,j] = initial; // state = 1111
			}
		}

		return RecursiveBacktracker(maze, width, height);
	}
}
