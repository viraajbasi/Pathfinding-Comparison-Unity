using System.Collections.Generic; 
using UnityEngine;

namespace Maze
{
    public class AStar
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var startNode = mazeList.Find(a => a.StartNode);
            var goalNode = mazeList.Find(a => a.GoalNode);
            var openList = new List<MazeCell> {startNode};
            var closedList = new List<MazeCell>();

            startNode.GCost = 0;
            startNode.HCost = GetManhattanDistance(startNode, goalNode);

            while (openList.Count > 0)
            {
                var currentNode = GetNodeWithLowestFCost(openList);

                if (currentNode.GoalNode)
                {
                    return GetPath(goalNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                var neighbourList = GenerateNeighbourList(mazeList, mazeList.FindIndex(a => a == currentNode));

                foreach (var neighbour in neighbourList)
                {
                    if (closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    var provisionalGCost = currentNode.GCost + GetManhattanDistance(currentNode, neighbour);

                    if (provisionalGCost < neighbour.GCost)
                    {
                        neighbour.Parent = currentNode;
                        neighbour.GCost = provisionalGCost;
                        neighbour.HCost = GetManhattanDistance(neighbour, goalNode);

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
            
            return mazeList;
        }

        private static int GetManhattanDistance(MazeCell nodeA, MazeCell nodeB)
        {
            var distX = Mathf.Abs(nodeA.Coordinates.X - nodeB.Coordinates.X);
            var distY = Mathf.Abs(nodeA.Coordinates.Y - nodeB.Coordinates.Y);

            return distX + distY;
        }

        private static MazeCell GetNodeWithLowestFCost(List<MazeCell> mazeList)
        {
            var lowestFCostNode = mazeList[0];
            
            foreach (var node in mazeList)
            {
                if (node.FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = node;
                }
            }

            return lowestFCostNode;
        }

        private static List<MazeCell> GenerateNeighbourList(List<MazeCell> mazeList, int currentIndex)
        {
            var list = new List<MazeCell>();
            var currentPosition = new Position(mazeList[currentIndex].Coordinates.X, mazeList[currentIndex].Coordinates.Y);

            if (!mazeList[currentIndex].Top)
            {
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y + 1));
            }

            if (!mazeList[currentIndex].Left)
            {
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X - 1 && a.Coordinates.Y == currentPosition.Y));
            }

            if (!mazeList[currentIndex].Right)
            {
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X + 1 && a.Coordinates.Y == currentPosition.Y));
            }

            if (!mazeList[currentIndex].Bottom)
            {
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y - 1));
            }

            return list;
        }

        private static List<MazeCell> GetPath(MazeCell goalNode)
        {
            var path = new List<MazeCell> {goalNode};
            var currentNode = goalNode;

            while (currentNode.Parent != null)
            {
                path.Add(currentNode.Parent);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            foreach (var n in path)
            {
                n.Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                n.Floor.gameObject.SetActive(true);
            }

            return path;
        }
    }
}
