using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public static class AStar
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var startNode = mazeList.Find(a => a.StartNode);
            var goalNode = mazeList.Find(a => a.GoalNode);
            var openList = new List<MazeCell> {startNode};
            var closedList = new List<MazeCell>();

            startNode.GCost = 0;
            startNode.HCost = MazeCell.GetManhattanDistance(startNode, goalNode);

            while (openList.Count > 0)
            {
                var currentNode = GetNodeWithLowestFCost(openList);
                currentNode.Visited = true;

                if (currentNode.GoalNode)
                {
                    var totalVisitedNodes = MazeCell.GetVisitedNodeCount(closedList);
                    PlayerPrefs.SetInt("A*TotalVisited", totalVisitedNodes);
                    return GetPath(startNode, goalNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                var neighbourList = MazeCell.GenerateNeighbourList(mazeList, currentNode);

                foreach (var neighbour in neighbourList)
                {
                    if (closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    var provisionalGCost = currentNode.GCost + MazeCell.GetManhattanDistance(currentNode, neighbour);

                    if (provisionalGCost < neighbour.GCost)
                    {
                        neighbour.Parent = currentNode;
                        neighbour.GCost = provisionalGCost;
                        neighbour.HCost = MazeCell.GetManhattanDistance(neighbour, goalNode);

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
            
            return mazeList;
        }

        private static MazeCell GetNodeWithLowestFCost(List<MazeCell> mazeList)
        {
            var lowestFCostNode = mazeList[0];
            
            foreach (var node in mazeList.Where(node => node.FCost < lowestFCostNode.FCost))
            {
                lowestFCostNode = node;
            }

            return lowestFCostNode;
        }

        private static List<MazeCell> GetPath(MazeCell startNode, MazeCell goalNode)
        {
            var path = new List<MazeCell> {goalNode};
            var currentNode = goalNode;

            while (currentNode.Parent != null)
            {
                currentNode.Path = true;
                path.Add(currentNode.Parent);
                currentNode = currentNode.Parent;
            }

            startNode.Path = true;
            path.Add(startNode);
            
            path.Reverse();

            return path;
        }
    }
}
