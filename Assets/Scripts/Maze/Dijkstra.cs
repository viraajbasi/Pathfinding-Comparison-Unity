using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class Dijkstra : MonoBehaviour
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var nodesToVisitQueue = new Queue<MazeCell>();
            var targetNode = mazeList.Find(a => a.GoalNode);
            nodesToVisitQueue.Enqueue(targetNode);

            while (nodesToVisitQueue.Count > 0)
            {
                var currentNode = nodesToVisitQueue.Dequeue();
                var currentNodeIndex = mazeList.FindIndex(a => a == currentNode);
                
                if (currentNode == targetNode)
                {
                    currentNode.Distance = 0;
                }

                var nextNodes = GenerateNeighbourList(mazeList, currentNodeIndex).Where(node => !node.Visited).ToList();
                
                foreach (var node in nextNodes)
                {
                    var index = mazeList.FindIndex(a => a == node);
                    var newDistance = currentNode.Distance + node.Cost;
                    mazeList[index].Distance = Mathf.Min(node.Distance, newDistance);
                    
                    nodesToVisitQueue.Enqueue(node);
                }

                mazeList[currentNodeIndex].Visited = true;
            }

            return mazeList;
        }
        
        public static void GeneratePathToNode(List<MazeCell> mazeList, int startNodeIndex)
        {
            while (true)
            {
                if (mazeList[startNodeIndex].GoalNode)
                {
                    mazeList[startNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                    break;
                }
                
                mazeList[startNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                Debug.Log($"Index: {startNodeIndex}, Distance: {mazeList[startNodeIndex]}.Distance");

                var neighbourList = GenerateNeighbourList(mazeList, startNodeIndex);
                var nodeWithShortestDistance = FindShortestNode(neighbourList);
                var nextNodeIndex = mazeList.FindIndex(a => a == nodeWithShortestDistance);

                startNodeIndex = nextNodeIndex;
            }
        }

        private static MazeCell FindShortestNode(List<MazeCell> neighbourList)
        {
            var distanceList = neighbourList.Select(n => n.Distance).ToList();

            //return distanceList.Min();

            var node = neighbourList.Find(a => a.Distance == distanceList.Min());

            return node;
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
