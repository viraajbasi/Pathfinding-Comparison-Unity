using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        private static Position _startPosition = new(0, 0);

        public static void HandleKeyInput()
        {
            var currentNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y);
            var topOffset = new Position(_startPosition.X, _startPosition.Y + 1);
            var leftOffset = new Position(_startPosition.X - 1, _startPosition.Y);
            var rightOffset = new Position(_startPosition.X + 1, _startPosition.Y);
            var bottomOffset = new Position(_startPosition.X, _startPosition.Y - 1);

            if (Input.GetKey(KeyCode.W))
            {
                var topNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y + 1);
                if (!Render.SortedMaze[currentNodeIndex].Top)
                {
                    Render.SortedMaze[currentNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    Render.SortedMaze[topNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                    _startPosition = topOffset;
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (_startPosition.X > 0)
                {
                    var leftNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X - 1 && a.Coordinates.Y == _startPosition.Y);
                    if (!Render.SortedMaze[currentNodeIndex].Left && _startPosition.X >= 0)
                    {
                        Render.SortedMaze[currentNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.red;
                        Render.SortedMaze[leftNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                        _startPosition = leftOffset;
                    }
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                var rightNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X + 1 && a.Coordinates.Y == _startPosition.Y);
                if (!Render.SortedMaze[currentNodeIndex].Right)
                {
                    Render.SortedMaze[currentNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    Render.SortedMaze[rightNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                    _startPosition = rightOffset;
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (_startPosition.Y > 0)
                {
                    var bottomNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y - 1);
                    if (!Render.SortedMaze[currentNodeIndex].Bottom && _startPosition.Y >= 0)
                    {
                        Render.SortedMaze[currentNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.red;
                        Render.SortedMaze[bottomNodeIndex].Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
                        _startPosition = bottomOffset;
                    }
                }
            }
        }
    }
}
