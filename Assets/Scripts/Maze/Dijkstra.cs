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
                var nextNodes = MazeCell.GenerateNeighbourList(mazeList, currentNode).Where(node => !node.Visited).ToList();
                
                foreach (var node in nextNodes)
                {
                    var nodeInList = mazeList.Find(a => a == node);
                    node.Cost = MazeCell.GetManhattanDistance(currentNode, node);
                    var newDistance = currentNode.Distance + node.Cost;
                    nodeInList.Distance = Mathf.Min(node.Distance, newDistance);

                    nodesToVisitQueue.Enqueue(nodeInList);
                }

                currentNode.Visited = true;
            }

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
