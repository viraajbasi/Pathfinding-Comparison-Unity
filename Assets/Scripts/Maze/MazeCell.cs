using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
	public class MazeCell
	{
		public readonly Position Coordinates;
		public readonly int Cost;
		
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

		public bool Path;
		
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

		public static List<MazeCell> GenerateNeighbourList(List<MazeCell> mazeList, MazeCell currentNode)
		{
			var list = new List<MazeCell>();
			var currentPosition = new Position(currentNode.Coordinates.X, currentNode.Coordinates.Y);

			if (!currentNode.Top)
			{
				list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y + 1));
			}

			if (!currentNode.Left)
			{
				list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X - 1 && a.Coordinates.Y == currentPosition.Y));
			}

			if (!currentNode.Right)
			{
				list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X + 1 && a.Coordinates.Y == currentPosition.Y));
			}

			if (!currentNode.Bottom)
			{
				list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y - 1));
			}

			return list;
		}
	}
}
