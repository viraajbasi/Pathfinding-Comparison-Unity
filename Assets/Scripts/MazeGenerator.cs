using System.Collections.Generic;
using UnityEngine;

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
	public bool Top;
	public bool Bottom;
	public bool Left;
	public bool Right;
	public bool Visited;
	public int X;
	public int Y;
}

public static class MazeGenerator
{
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

		if (PlayerPrefs.GetInt("Kruskal") == 1)
		{
			return Kruskal.Algorithm(maze, width, height);
		}

		return RecursiveBacktracker.Algorithm(maze, width, height);
	}
}
