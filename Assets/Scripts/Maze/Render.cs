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
        var maze = Generator.Generate(width, height);
        Draw(maze);
    }
    
    private void Draw(List<WallStateBool> maze)
	{
		var floor = Instantiate(floorPrefab, transform);
		floor.localScale = new Vector3(width, 1, height);

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var cell = maze[maze.FindIndex(a => a.Coordinates.X == i && a.Coordinates.Y == j)];
				var pos = new Vector3(-width / 2 + i, 0, -height / 2 + j);

				if (cell.Top)
				{
					var topWall = Instantiate(wallPrefab, transform) as Transform;
					topWall.position = pos + new Vector3(0, 0, size / 2);
					topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
				}

				if (cell.Left)
				{
					var leftWall = Instantiate(wallPrefab, transform) as Transform;
					leftWall.position = pos + new Vector3(-size / 2, 0, 0);
					leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
					leftWall.eulerAngles = new Vector3(0, 90, 0);
				}

				if (i == width - 1)
				{
					if (cell.Right)
					{
						var rightWall = Instantiate(wallPrefab, transform) as Transform;
						rightWall.position = pos + new Vector3(+size / 2, 0, 0);
						rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
						rightWall.eulerAngles = new Vector3(0, 90, 0);
					}
				}

				if (j == 0)
				{
					if (cell.Bottom)
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
