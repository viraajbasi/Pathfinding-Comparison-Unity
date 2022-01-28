using System.Collections.Generic;
using UnityEngine;

public static class Generator
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
					Coordinates = new Position
					{
						X = i,
						Y = j
					}
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
