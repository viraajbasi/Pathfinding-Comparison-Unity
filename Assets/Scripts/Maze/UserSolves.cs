using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        public static Position StartPosition;

        private static int _totalVisitedNodes;

        public static void HandleKeyInput(List<MazeCell> mazeList, Color defaultFloorColour)
        {
            var currentNode = mazeList.Find(a => a.Coordinates.X == StartPosition.X && a.Coordinates.Y == StartPosition.Y);
            currentNode.Visited = true;

            if (currentNode.GoalNode)
            {
                PlayerPrefs.SetInt("MazeSolved", 1);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                StartPosition = ResetToStart(mazeList, defaultFloorColour);
            } 
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W))
            {
                var topOffset = new Position(StartPosition.X, StartPosition.Y + 1);
                var topNode = mazeList.Find(a => a.Coordinates.X == topOffset.X && a.Coordinates.Y == topOffset.Y);
                
                if (!currentNode.Top)
                {
                    VisitAndColour(topOffset, currentNode, topNode);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.A))
            {
                if (StartPosition.X > 0)
                {
                    var leftOffset = new Position(StartPosition.X - 1, StartPosition.Y);
                    var leftNode = mazeList.Find(a => a.Coordinates.X == leftOffset.X && a.Coordinates.Y == leftOffset.Y);

                    if (!currentNode.Left && StartPosition.X >= 0)
                    {
                        VisitAndColour(leftOffset, currentNode, leftNode);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.D))
            {
                var rightOffset = new Position(StartPosition.X + 1, StartPosition.Y);
                var rightNode = mazeList.Find(a => a.Coordinates.X == rightOffset.X && a.Coordinates.Y == rightOffset.Y);
                
                if (!currentNode.Right)
                {
                    VisitAndColour(rightOffset, currentNode, rightNode);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.S))
            {
                if (StartPosition.Y > 0)
                {
                    var bottomOffset = new Position(StartPosition.X, StartPosition.Y - 1);
                    var bottomNode = mazeList.Find(a => a.Coordinates.X == bottomOffset.X && a.Coordinates.Y == bottomOffset.Y);
                    
                    if (!currentNode.Bottom && StartPosition.Y >= 0)
                    {
                        VisitAndColour(bottomOffset, currentNode, bottomNode);
                    }
                }
            }
        }
        
        private static void VisitAndColour(Position pos, MazeCell currentNode, MazeCell nextNode)
        {
            currentNode.Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
            nextNode.Floor.gameObject.GetComponent<Renderer>().material.color = Color.white;
            
            currentNode.Floor.gameObject.SetActive(true);
            nextNode.Floor.gameObject.SetActive(true);

            StartPosition = pos;
        }

        private static Position ResetToStart(List<MazeCell> mazeList, Color defaultColor)
        {
            foreach (var node in mazeList.Where(node =>  node.Floor.gameObject.GetComponent<Renderer>().material.color == Color.black || node.Floor.gameObject.GetComponent<Renderer>().material.color == Color.white))
            {
                node.Floor.gameObject.GetComponent<Renderer>().material.color = defaultColor;
            }

            var currentNode = mazeList.Find(a => a.StartNode);
            var currentNodePosition = currentNode.Coordinates;
            currentNode.Floor.gameObject.GetComponent<Renderer>().material.color = Color.white;

            return currentNodePosition;
        }
    }
}
