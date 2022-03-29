using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public static class Dijkstra
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var nodesToVisitQueue = new Queue<MazeCell>();
            var startNode = mazeList.Find(a => a.StartNode);
            var targetNode = mazeList.Find(a => a.GoalNode);
            targetNode.Distance = 0;
            nodesToVisitQueue.Enqueue(targetNode);

            while (nodesToVisitQueue.Count > 0)
            {
                var currentNode = nodesToVisitQueue.Dequeue();
                var unvisitedNeighbourNodes = MazeCell.GenerateNeighbourList(mazeList, currentNode).Where(node => !node.Visited).ToList();

                for (var i = 0; i < unvisitedNeighbourNodes.Count; i++)
                {
                    var nodeInList = unvisitedNeighbourNodes[i];
                    nodeInList.Cost = MazeCell.GetManhattanDistance(currentNode, nodeInList);
                    var newDistance = currentNode.Distance + nodeInList.Cost;
                    nodeInList.Distance = Mathf.Min(nodeInList.Distance, newDistance);

                    nodesToVisitQueue.Enqueue(nodeInList);
                }

                currentNode.Visited = true;
            }

            var totalVisitedNodes = MazeCell.GetVisitedNodeCount(mazeList);
            PlayerPrefs.SetInt("DijkstraTotalVisited", totalVisitedNodes);

            return GetPath(mazeList, startNode, targetNode);
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
