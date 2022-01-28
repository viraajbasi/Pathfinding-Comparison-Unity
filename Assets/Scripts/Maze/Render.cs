using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
	public int width = 10;
	public int height = 10;
	public float size = 1f;
	public Transform wallPrefab;
	public Transform floorPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        Draw(Generate(width, height));
    }

	private List<WallStateBool> Generate(int width, int height)
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
    
    private void Draw(List<WallStateBool> maze)
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
					var topWall = Instantiate(wallPrefab, transform) as Transform;
					topWall.position = pos + new Vector3(0, 0, size / 2);
					topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
				}

				if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Left)
				{
					var leftWall = Instantiate(wallPrefab, transform) as Transform;
					leftWall.position = pos + new Vector3(-size / 2, 0, 0);
					leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
					leftWall.eulerAngles = new Vector3(0, 90, 0);
				}

				if (i == width - 1)
				{
					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Right)
					{
						var rightWall = Instantiate(wallPrefab, transform) as Transform;
						rightWall.position = pos + new Vector3(+size / 2, 0, 0);
						rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
						rightWall.eulerAngles = new Vector3(0, 90, 0);
					}
				}

				if (j == 0)
				{
					if (maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)].Bottom)
					{
						var bottomWall = Instantiate(wallPrefab, transform) as Transform;
						bottomWall.position = pos + new Vector3(0, 0, -size / 2);
						bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
					}
				}
			}
		}
	}
}
