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
		
		public MazeCell Parent = null;
		
		public Transform MazeNode;
		public Transform Floor;
		
		public Position Coordinates;
		
		public int Cost;
		public int Distance = int.MaxValue;
		
		public int FCost => GCost + HCost;
		public int GCost = int.MaxValue;
		public int HCost;

		public MazeCell(bool top, bool bottom, bool left, bool right, bool visited, int x, int y, int cost)
		{
			Top = top;
			Bottom = bottom;
			Left = left;
			Right = right;
			Visited = visited;
			Coordinates = new Position(x, y);
			Cost = cost;
		}
	}
}
