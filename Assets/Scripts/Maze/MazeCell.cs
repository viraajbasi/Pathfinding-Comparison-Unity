using UnityEngine;

namespace Maze
{
	public class MazeCell
	{
		public bool Top;
		public bool Bottom;
		public bool Left;
		public bool Right;
		public bool Visited;
		public bool StartNode;
		public bool GoalNode;
		public Position Coordinates;
		public int Cost;
		public GameObject MazeNode;

		public MazeCell(bool top, bool bottom, bool left, bool right, bool visited, int x, int y, int cost, GameObject mazeNode)
		{
			Top = top;
			Bottom = bottom;
			Left = left;
			Right = right;
			Visited = visited;
			Coordinates = new Position(x, y);
			Cost = cost;
			MazeNode = mazeNode;
		}
	}
}
