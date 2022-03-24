using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class AStar
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            var startNode = mazeList.Find(a => a.StartNode);
            var endNode = mazeList.Find(a => a.GoalNode);
            var openList = new List<MazeCell>();
            var closedList = new HashSet<MazeCell>();
            
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                var currentNode = openList.First();

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || openList[i].FCost < currentNode.FCost && openList[i].HCost < currentNode.HCost)
                    {
                        currentNode.Visited = true;
                        currentNode = openList[i];
                    }

                    openList.Remove(currentNode);
                    closedList.Add(currentNode);

                    if (currentNode.GoalNode)
                    {
                        mazeList = GetFinalPath(endNode, mazeList);
                    }

                    var neighbourList = GenerateNeighbourList(mazeList, mazeList.FindIndex(a => a == currentNode));

                    foreach (var neighbour in neighbourList)
                    {
                        if (closedList.Contains(neighbour))
                        {
                            continue;
                        }

                        var moveCost = currentNode.GCost + GetManhattenDistance(currentNode, neighbour);

                        if (moveCost < neighbour.GCost || !openList.Contains(neighbour))
                        {
                            neighbour.GCost = moveCost;
                            neighbour.HCost = GetManhattenDistance(neighbour, endNode);
                            neighbour.Parent = currentNode;

                            if (!openList.Contains(neighbour))
                            {
                                openList.Add(neighbour);
                            }
                        }
                    }
                }
            }

            return mazeList;
        }

        private static int GetManhattenDistance(MazeCell nodeA, MazeCell nodeB)
        {
            var distX = Mathf.Abs(nodeA.Coordinates.X - nodeB.Coordinates.X);
            var distY = Mathf.Abs(nodeA.Coordinates.Y - nodeB.Coordinates.Y);

            return distX + distY;
        }
        
        private static List<MazeCell> GetFinalPath(MazeCell endNode, List<MazeCell> mazeList)
        {
            var currentNode = endNode;

            while (!currentNode.StartNode)
            {
                var currentNodeIndex = mazeList.FindIndex(a => a == currentNode);
                mazeList[currentNodeIndex].Path = true;
                currentNode = mazeList[currentNodeIndex].Parent;
            }

            return mazeList;
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
