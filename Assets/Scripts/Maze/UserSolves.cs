using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        private static Position _startPosition = new(0, 0);

        public static void HandleKeyInput()
        {
            var currentNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y);
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
            {
                var topNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y + 1);
                var topOffset = new Position(_startPosition.X, _startPosition.Y + 1);
                if (!Render.SortedMaze[currentNodeIndex].Top)
                {
                    VisitAndColour(currentNodeIndex, topNodeIndex, topOffset);
                }
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.A))
            {
                if (_startPosition.X > 0)
                {
                    var leftNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X - 1 && a.Coordinates.Y == _startPosition.Y);
                    var leftOffset = new Position(_startPosition.X - 1, _startPosition.Y);
                    if (!Render.SortedMaze[currentNodeIndex].Left && _startPosition.X >= 0)
                    {
                        VisitAndColour(currentNodeIndex, leftNodeIndex, leftOffset);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D))
            {
                var rightNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X + 1 && a.Coordinates.Y == _startPosition.Y);
                var rightOffset = new Position(_startPosition.X + 1, _startPosition.Y);
                if (!Render.SortedMaze[currentNodeIndex].Right)
                {
                    VisitAndColour(currentNodeIndex, rightNodeIndex, rightOffset);
                }
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.S))
            {
                if (_startPosition.Y > 0)
                {
                    var bottomNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y - 1);
                    var bottomOffset = new Position(_startPosition.X, _startPosition.Y - 1);
                    if (!Render.SortedMaze[currentNodeIndex].Bottom && _startPosition.Y >= 0)
                    {
                        VisitAndColour(currentNodeIndex, bottomNodeIndex, bottomOffset);
                    }
                }
            }
        }
        
        private static void VisitAndColour(int currentIndex, int nextIndex, Position pos)
        {
            Render.SortedMaze[nextIndex].Visited = true;
            Render.SortedMaze[currentIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
            Render.SortedMaze[nextIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.white;
            _startPosition = pos;
        }
    }
}
