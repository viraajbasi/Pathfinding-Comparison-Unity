using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public static class BellmanFord
    {
        // Perform Bellman-Ford algorithm.
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var startNode = mazeList.Find(a => a.StartNode);
            var goalNode = mazeList.Find(a => a.GoalNode);

            foreach (var node in mazeList) // Iterate through lists and generate paths.
            {
                var neighbourList = MazeCell.GenerateNeighbourList(mazeList, node);

                foreach (var neighbour in neighbourList)
                {
                    Relax(node, neighbour);
                }
            }

            foreach (var node in mazeList) // Check for negative cycles.
            {
                var neighbourList = MazeCell.GenerateNeighbourList(mazeList, node);

                foreach (var neighbour in neighbourList)
                {
                    if (node.Distance > neighbour.Distance + node.Cost)
                    {
                        throw new Exception("Negative cycles detected");
                    }
                }
            }

            var totalVisitedNodes = MazeCell.GetVisitedNodeCount(mazeList);
            PlayerPrefs.SetInt("BellmanFordTotalVisited", totalVisitedNodes);

            return GetPath(mazeList, startNode, goalNode);
        }

        private static void Relax(MazeCell nodeA, MazeCell nodeB) // Determine which distance is the shortest between two nodes.
        {
            nodeA.Cost = MazeCell.GetManhattanDistance(nodeA, nodeB);
            
            if (nodeA.Distance > nodeB.Distance + nodeA.Cost)
            {
                nodeA.Visited = true;
                nodeA.Distance = nodeB.Distance + nodeA.Cost;
                nodeA.Parent = nodeB;
            }
        }

        private static List<MazeCell> GetPath(List<MazeCell> mazeList, MazeCell startNode, MazeCell goalNode) // Find path and return it.
        {
            var path = new List<MazeCell> {startNode};
            var currentNode = startNode;
            goalNode.Path = true;

            while (!currentNode.GoalNode)
            {
                currentNode.Path = true;
                path.Add(currentNode);

                var neighbourList = MazeCell.GenerateNeighbourList(mazeList, currentNode);
                var nodeWithShortestDistance = MazeCell.GetNodeWithLowestDistance(neighbourList);

                currentNode = nodeWithShortestDistance;
            }
            
            path.Add(goalNode);

            return path;
        }
    }
}
