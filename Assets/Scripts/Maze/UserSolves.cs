using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        public static Position StartPosition;

        public static void HandleKeyInput(List<MazeCell> mazeList)
        {
            var currentNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == StartPosition.X && a.Coordinates.Y == StartPosition.Y);
            //var currentNodeIndex = mazeList.FindIndex(a => a.StartNode);
            if (mazeList[currentNodeIndex].GoalNode)
            {
                PlayerPrefs.SetInt("MazeSolved", 1);
            }
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
            {
                var topNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == StartPosition.X && a.Coordinates.Y == StartPosition.Y + 1);
                var topOffset = new Position(StartPosition.X, StartPosition.Y + 1);
                if (!mazeList[currentNodeIndex].Top)
                {
                    VisitAndColour(currentNodeIndex, topNodeIndex, topOffset, mazeList);
                }
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.A))
            {
                if (StartPosition.X > 0)
                {
                    var leftNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == StartPosition.X - 1 && a.Coordinates.Y == StartPosition.Y);
                    var leftOffset = new Position(StartPosition.X - 1, StartPosition.Y);
                    if (!mazeList[currentNodeIndex].Left && StartPosition.X >= 0)
                    {
                        VisitAndColour(currentNodeIndex, leftNodeIndex, leftOffset, mazeList);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D))
            {
                var rightNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == StartPosition.X + 1 && a.Coordinates.Y == StartPosition.Y);
                var rightOffset = new Position(StartPosition.X + 1, StartPosition.Y);
                if (!mazeList[currentNodeIndex].Right)
                {
                    VisitAndColour(currentNodeIndex, rightNodeIndex, rightOffset, mazeList);
                }
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.S))
            {
                if (StartPosition.Y > 0)
                {
                    var bottomNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == StartPosition.X && a.Coordinates.Y == StartPosition.Y - 1);
                    var bottomOffset = new Position(StartPosition.X, StartPosition.Y - 1);
                    if (!mazeList[currentNodeIndex].Bottom && StartPosition.Y >= 0)
                    {
                        VisitAndColour(currentNodeIndex, bottomNodeIndex, bottomOffset, mazeList);
                    }
                }
            }
        }
        
        private static void VisitAndColour(int currentIndex, int nextIndex, Position pos, List<MazeCell> mazeList)
        {
            mazeList[nextIndex].Visited = true;
            mazeList[currentIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
            mazeList[nextIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.white;
            StartPosition = pos;
        }
    }
}
