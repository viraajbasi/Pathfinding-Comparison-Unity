using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
	public class MazeCell
	{
		// Related to maze generation.
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

		// Generates a list of all the neighbouring nodes, relative to the supplied current node.
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
		
		// Finds the Manhattan Distance between two supplied nodes.
		public static int GetManhattanDistance(MazeCell nodeA, MazeCell nodeB)
		{
			var distX = Mathf.Abs(nodeA.Coordinates.X - nodeB.Coordinates.X);
			var distY = Mathf.Abs(nodeA.Coordinates.Y - nodeB.Coordinates.Y);

			return Mathf.Abs(distX + distY);
		}
		
		// Finds the node with the lowest distance in a list.
		public static MazeCell GetNodeWithLowestDistance(List<MazeCell> mazeList)
		{
			var nodeWithLowestDistance = mazeList[0];

			foreach (var neighbour in mazeList.Where(neighbour => neighbour.Distance < nodeWithLowestDistance.Distance))
			{
				nodeWithLowestDistance = neighbour;
			}

			return nodeWithLowestDistance;
		}

		// Returns the number of visited nodes in a supplied list.
		public static int GetVisitedNodeCount(List<MazeCell> mazeList)
		{
			return mazeList.Count(node => node.Visited);
		}

		// Returns the number of nodes in the path in a supplied list.
		public static int GetPathNodeCount(List<MazeCell> mazeList)
		{
			return mazeList.Count(node => node.Path);
		}
	}
}
