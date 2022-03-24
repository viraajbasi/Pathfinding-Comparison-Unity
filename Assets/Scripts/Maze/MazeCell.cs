using UnityEngine;
using System.Collections.Generic;

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
		public Transform MazeNode;
		public Transform Floor;
		public float Distance = float.MaxValue;
		public int GCost;
		public int HCost;
		public int FCost => GCost + HCost;
		public MazeCell Parent;
		public bool Path;

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
