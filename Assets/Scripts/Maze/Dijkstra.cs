using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class Dijkstra : MonoBehaviour
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            PriorityQueue<MazeCell> queue = new(true);
            var traverseIndex = 0;
            
            foreach (var node in mazeList)
            {
                node.Distance = int.MaxValue;
            }

            mazeList.Find(a => a.StartNode).Distance = 0;

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                var currentNodeIndex = mazeList.FindIndex(a => a == currentNode);
                var neighbourPositions = GenerateNeighbourPositionList(mazeList, currentNodeIndex);

                foreach (var position in neighbourPositions)
                {
                    var neighbourIndex = mazeList.FindIndex(a => a.Coordinates.X == position.X && a.Coordinates.Y == position.Y);

                    if (mazeList[neighbourIndex].Distance > 0)
                    {
                        if (mazeList[currentNodeIndex].Distance > mazeList[neighbourIndex].Distance + mazeList[neighbourIndex].Cost)
                        {
                            mazeList[neighbourIndex].Distance = mazeList[currentNodeIndex].Distance + mazeList[neighbourIndex].Cost;
                            mazeList[neighbourIndex].Parent = mazeList[currentNodeIndex];
                        }
                        
                        queue.UpdatePriority(mazeList[neighbourIndex], mazeList[neighbourIndex].Distance);
                    }
                }
            }

            return mazeList;
        }
        
        public static void GeneratePathToNode(List<MazeCell> mazeList)
        {
            var traverseIndex = 0;
            mazeList.Find(a => a.StartNode).TraverseIndex = traverseIndex;
            var currentIndex = mazeList.FindIndex(a => a.StartNode);

            while (true)
            {
                if (mazeList[currentIndex].GoalNode)
                {
                    break;
                }

                var neighbours = GenerateNeighbourPositionList(mazeList, currentIndex);
                var distanceList = new List<int>();

                traverseIndex++;

                foreach (var neighbour in neighbours)
                {
                    var neighbourIndex = mazeList.FindIndex(a => a.Coordinates.X == neighbour.X && a.Coordinates.Y == neighbour.Y);
                    distanceList.Add(mazeList[neighbourIndex].Distance);
                }

                var minDistance = distanceList.Min();

                mazeList.Find(a => a.Distance == minDistance).TraverseIndex = traverseIndex;

                currentIndex = mazeList.FindIndex(a => a.TraverseIndex == traverseIndex);

                distanceList.Clear();
            }
        }

        private static List<Position> GenerateNeighbourPositionList(List<MazeCell> mazeList, int currentIndex)
        {
            var list = new List<Position>();
            var currentPosition = new Position(mazeList[currentIndex].Coordinates.X, mazeList[currentIndex].Coordinates.Y);

            if (!mazeList[currentIndex].Top)
            {
                list.Add(new Position(currentPosition.X, currentPosition.Y + 1));
            }

            if (!mazeList[currentIndex].Left)
            {
                list.Add(new Position(currentPosition.X - 1, currentPosition.Y));
            }

            if (!mazeList[currentIndex].Right)
            {
                list.Add(new Position(currentPosition.X + 1, currentPosition.Y));
            }

            if (!mazeList[currentIndex].Bottom)
            {
                list.Add(new Position(currentPosition.X, currentPosition.Y - 1));
            }

            return list;
        }
    }
}
