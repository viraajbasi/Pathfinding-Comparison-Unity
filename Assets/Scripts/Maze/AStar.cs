using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class AStar
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var openList = new List<MazeCell>();
            var closedList = new List<MazeCell>();
            var startNodeIndex = mazeList.FindIndex(a => a.StartNode);
            var endNodeIndex = mazeList.FindIndex(a => a.GoalNode);
            
            openList.Add(mazeList[startNodeIndex]);

            foreach (var node in mazeList)
            {
                node.GCost = int.MaxValue;
                node.Parent = null;
            }

            mazeList[startNodeIndex].GCost = 0;
            mazeList[startNodeIndex].HCost = GetManhattanDistance(mazeList[startNodeIndex], mazeList[endNodeIndex]);
            mazeList[startNodeIndex].FCost = CalculateFCost(mazeList[startNodeIndex]);

            while (openList.Count > 0)
            {
                var currentNodeIndex = GetIndexOfLowestNode(openList);
                var currentNode = mazeList[currentNodeIndex];

                if (currentNode.GoalNode)
                {
                    mazeList = GetFinalPath(mazeList[endNodeIndex]);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                var neighbourList = GenerateNeighbourList(mazeList, currentNodeIndex);

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
                        neighbour.HCost = GetManhattanDistance(neighbour, mazeList[endNodeIndex]);
                        neighbour.FCost = CalculateFCost(neighbour);

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

        private static int CalculateFCost(MazeCell node)
        {
            return node.GCost + node.HCost;
        }

        private static int GetIndexOfLowestNode(List<MazeCell> mazeList)
        {
            var listFCost = mazeList.Select(a => a.FCost).ToList();
            var node = mazeList.Find(a => a.FCost == listFCost.Min());
            var index = mazeList.FindIndex(a => a == node);

            return index;
        }
        
        private static List<MazeCell> GetFinalPath(MazeCell endNode)
        {
            var path = new List<MazeCell> {endNode};
            var currentNode = endNode;

            while (currentNode.Parent != null)
            {
                path.Add(currentNode.Parent);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            return path;
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
    }
}
