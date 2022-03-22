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
        
        public static void GeneratePathToNode(List<MazeCell> mazeList, int startNodeIndex) // TODO: FIX THE INFINITE LOOP
        {
            if (mazeList.Count == 0 || mazeList[startNodeIndex].GoalNode)
            {
                return;
            }
            
            mazeList[startNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;

            var minDistance = GenerateNeighbourList(mazeList, startNodeIndex).Min(a => a.Distance);
            var nextNodeIndex = mazeList.FindIndex(a => a.Distance == minDistance);
            
            mazeList[nextNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
            
            GeneratePathToNode(mazeList, nextNodeIndex);

            foreach (var n in mazeList)
            {
                Debug.Log($"{n.Coordinates.X},{n.Coordinates.Y} Distance = {n.Distance}");
            }
            
        }

        private float CalculateNodeDistance(MazeCell currentNode, MazeCell targetNode)
        {
            return Mathf.Pow(currentNode.Coordinates.X + currentNode.Coordinates.Y, 2) - Mathf.Pow(targetNode.Coordinates.X + targetNode.Coordinates.Y, 2);
        }

        private static List<MazeCell> GenerateNeighbourList(List<MazeCell> mazeList, int currentIndex)
        {
            var list = new List<MazeCell>();
            var currentPosition = new Position(mazeList[currentIndex].Coordinates.X, mazeList[currentIndex].Coordinates.Y);

            if (!mazeList[currentIndex].Top)
            {
                //list.Add(new Position(currentPosition.X, currentPosition.Y + 1));
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y + 1));
            }

            if (!mazeList[currentIndex].Left)
            {
                //list.Add(new Position(currentPosition.X - 1, currentPosition.Y));
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X - 1 && a.Coordinates.Y == currentPosition.Y));
            }

            if (!mazeList[currentIndex].Right)
            {
                //list.Add(new Position(currentPosition.X + 1, currentPosition.Y));
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X + 1 && a.Coordinates.Y == currentPosition.Y));
            }

            if (!mazeList[currentIndex].Bottom)
            {
                //list.Add(new Position(currentPosition.X, currentPosition.Y - 1));
                list.Add(mazeList.Find(a => a.Coordinates.X == currentPosition.X && a.Coordinates.Y == currentPosition.Y - 1));
            }

            return list;
        }
    }
}
