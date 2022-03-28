using System;
using System.Collections.Generic;

namespace Maze
{
    public static class BellmanFord
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var startNode = mazeList.Find(a => a.StartNode);
            var goalNode = mazeList.Find(a => a.GoalNode);

            foreach (var node in mazeList)
            {
                var neighbourList = MazeCell.GenerateNeighbourList(mazeList, node);

                foreach (var neighbour in neighbourList)
                {
                    Relax(node, neighbour);
                }
            }

            foreach (var node in mazeList)
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

            return GetPath(mazeList, startNode, goalNode);
        }

        private static void Relax(MazeCell nodeA, MazeCell nodeB)
        {
            nodeA.Cost = MazeCell.GetManhattanDistance(nodeA, nodeB);
            
            if (nodeA.Distance > nodeB.Distance + nodeA.Cost)
            {
                nodeA.Visited = true;
                nodeA.Distance = nodeB.Distance + nodeA.Cost;
                nodeA.Parent = nodeB;
            }
        }

        private static List<MazeCell> GetPath(List<MazeCell> mazeList, MazeCell startNode, MazeCell goalNode)
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
