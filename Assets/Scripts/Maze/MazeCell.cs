using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
	public class MazeCell
	{
		// Related to maze.
		public readonly Position Coordinates;
		public SharedWall SharedWall;
		
		// Booleans relating to walls and other properties of the cell.
		public bool Top;
		public bool Bottom;
		public bool Left;
		public bool Right;
		public bool StartNode;
		public bool GoalNode;

		// Boolean to determine if cell is visited.
		public bool Visited;
		
		// Weighting and distance variables for A*.
		public int FCost => GCost + HCost;
		public int GCost = int.MaxValue;
		public int HCost;

		// Weighting and distance variables for Dijkstra and Bellman-Ford..
		public int Cost;
		public int Distance = int.MaxValue;
		
		// Parent cell for backtracking purposes.
		public MazeCell Parent = null;
		
		// Unity floor object.
		public Transform Floor;

		// Boolean to determine whether cell is in the optimal path.
		public bool Path;

		public MazeCell(bool top, bool bottom, bool left, bool right, bool visited, int x, int y)
		{
			Top = top;
			Bottom = bottom;
			Left = left;
			Right = right;
			Visited = visited;
			Coordinates = new Position(x, y);
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
		
		public static int GetManhattanDistance(MazeCell nodeA, MazeCell nodeB)
		{
			var distX = Mathf.Abs(nodeA.Coordinates.X - nodeB.Coordinates.X);
			var distY = Mathf.Abs(nodeA.Coordinates.Y - nodeB.Coordinates.Y);

			return Mathf.Abs(distX + distY);
		}
		
		public static MazeCell GetNodeWithLowestDistance(List<MazeCell> neighbourList)
		{
			var nodeWithLowestDistance = neighbourList[0];

			foreach (var neighbour in neighbourList.Where(neighbour => neighbour.Distance < nodeWithLowestDistance.Distance))
			{
				nodeWithLowestDistance = neighbour;
			}

			return nodeWithLowestDistance;
		}

		public static int GetVisitedNodeCount(List<MazeCell> mazeList)
		{
			return mazeList.Count(node => node.Visited);
		}

		public static int GetPathNodeCount(List<MazeCell> mazeList)
		{
			return mazeList.Count(node => node.Path);
		}
	}
}
