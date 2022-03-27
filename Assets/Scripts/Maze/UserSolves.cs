using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        public static Position StartPosition;

        public static void HandleKeyInput(List<MazeCell> mazeList)
        {
            var currentNode = mazeList.Find(a => a.Coordinates.X == StartPosition.X && a.Coordinates.Y == StartPosition.Y);
            
            if (currentNode.GoalNode)
            {
                PlayerPrefs.SetInt("MazeSolved", 1);
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                var topOffset = new Position(StartPosition.X, StartPosition.Y + 1);
                var topNode = mazeList.Find(a => a.Coordinates.X == topOffset.X && a.Coordinates.Y == topOffset.Y);
                
                if (!currentNode.Top)
                {
                    VisitAndColour(topOffset, currentNode, topNode);
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
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

            if (Input.GetKeyDown(KeyCode.D))
            {
                var rightOffset = new Position(StartPosition.X + 1, StartPosition.Y);
                var rightNode = mazeList.Find(a => a.Coordinates.X == rightOffset.X && a.Coordinates.Y == rightOffset.Y);
                
                if (!currentNode.Right)
                {
                    VisitAndColour(rightOffset, currentNode, rightNode);
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
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
    }
}
