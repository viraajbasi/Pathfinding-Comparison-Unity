using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Dijkstra : MonoBehaviour
    {
        public static List<MazeCell> Algorithm(List<MazeCell> mazeList)
        {
            InitialiseSingleSource(mazeList);
            PriorityQueue<MazeCell> priorityQueue = new(true);

            foreach (var node in mazeList)
            {
                priorityQueue.Enqueue(node.Distance, node);
            }

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();
                
                /*
                 * TODO
                 * Logic loop here requires a list of nodes based on the current variable.
                 * Create a method to generate this.
                 */

            }

            return mazeList;
        }
        
        private static void InitialiseSingleSource(List<MazeCell> mazeList)
        {
            foreach (var node in mazeList)
            {
                node.Distance = int.MaxValue;
                node.Parent = null;
            }

            mazeList.Find(a => a.StartNode).Distance = 0;
        }

        private static void Relax(int nextNode, int currentNode, List<MazeCell> mazeList)
        {
            if (mazeList[nextNode].Distance > mazeList[currentNode].Distance + mazeList[currentNode].Cost)
            {
                mazeList[nextNode].Distance = mazeList[currentNode].Distance + mazeList[currentNode].Cost;
                mazeList[nextNode].Parent = mazeList[currentNode];
            }
        }
    }
}
