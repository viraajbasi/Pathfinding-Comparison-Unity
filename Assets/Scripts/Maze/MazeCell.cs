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
	}
}
