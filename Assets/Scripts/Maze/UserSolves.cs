using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        private static Position _startPosition = new(0, 0);

        public static void HandleKeyInput(List<MazeCell> mazeList)
        {
            var currentNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y);
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
            {
                var topNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y + 1);
                var topOffset = new Position(_startPosition.X, _startPosition.Y + 1);
                if (!mazeList[currentNodeIndex].Top)
                {
                    VisitAndColour(currentNodeIndex, topNodeIndex, topOffset, mazeList);
                }
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.A))
            {
                if (_startPosition.X > 0)
                {
                    var leftNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == _startPosition.X - 1 && a.Coordinates.Y == _startPosition.Y);
                    var leftOffset = new Position(_startPosition.X - 1, _startPosition.Y);
                    if (!mazeList[currentNodeIndex].Left && _startPosition.X >= 0)
                    {
                        VisitAndColour(currentNodeIndex, leftNodeIndex, leftOffset, mazeList);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D))
            {
                var rightNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == _startPosition.X + 1 && a.Coordinates.Y == _startPosition.Y);
                var rightOffset = new Position(_startPosition.X + 1, _startPosition.Y);
                if (!mazeList[currentNodeIndex].Right)
                {
                    VisitAndColour(currentNodeIndex, rightNodeIndex, rightOffset, mazeList);
                }
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.S))
            {
                if (_startPosition.Y > 0)
                {
                    var bottomNodeIndex = mazeList.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y - 1);
                    var bottomOffset = new Position(_startPosition.X, _startPosition.Y - 1);
                    if (!mazeList[currentNodeIndex].Bottom && _startPosition.Y >= 0)
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
            _startPosition = pos;
        }
    }
}
