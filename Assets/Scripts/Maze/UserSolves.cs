using System.ComponentModel;
using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        private static Position _startPosition = new(0, 0);

        public static void HandleKeyInput()
        {
            var currentNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y);
            var currentNode = Render.SortedMaze[currentNodeIndex].MazeNode;

            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("W");
                var _topOffset = new Position(_startPosition.X, _startPosition.Y + 1);
                var topNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y + 1);
                if (!Render.SortedMaze[topNodeIndex].Bottom)
                {
                    DrawLine(currentNode, Render.SortedMaze[topNodeIndex].MazeNode);
                    _startPosition = _topOffset;
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (_startPosition.X > 0)
                {
                    Debug.Log("A");
                    var _leftOffset = new Position(_startPosition.X - 1, _startPosition.Y);
                    var leftNodeIndex = Render.SortedMaze.FindIndex(a =>
                        a.Coordinates.X == _startPosition.X - 1 && a.Coordinates.Y == _startPosition.Y);
                    if (!Render.SortedMaze[leftNodeIndex].Right && _startPosition.X >= 0)
                    {
                        DrawLine(currentNode, Render.SortedMaze[leftNodeIndex].MazeNode);
                        _startPosition = _leftOffset;
                    }
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                Debug.Log("D");
                var _rightOffset = new Position(_startPosition.X + 1, _startPosition.Y);
                var rightNodeIndex = Render.SortedMaze.FindIndex(a => a.Coordinates.X == _startPosition.X + 1 && a.Coordinates.Y == _startPosition.Y);
                if (!Render.SortedMaze[rightNodeIndex].Left)
                {
                    DrawLine(currentNode, Render.SortedMaze[rightNodeIndex].MazeNode);
                    _startPosition = _rightOffset;
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (_startPosition.Y > 0)
                {
                    Debug.Log("S");
                    var _bottomOffset = new Position(_startPosition.X, _startPosition.Y - 1);
                    var bottomNodeIndex = Render.SortedMaze.FindIndex(a =>
                        a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y - 1);
                    if (!Render.SortedMaze[bottomNodeIndex].Top && _startPosition.Y >= 0)
                    {
                        DrawLine(currentNode, Render.SortedMaze[bottomNodeIndex].MazeNode);
                        _startPosition = _bottomOffset;
                    }
                }
            }
        }

        private static void DrawLine(Transform original, Transform target)
        {
            Debug.Log("Moving...");
            var lineRenderer = new LineRenderer
            {
                startColor = Color.red,
                endColor = Color.red,
                startWidth = 0.3f,
                endWidth = 0.3f
            };
            
            lineRenderer.SetPosition(0, original.position);
            lineRenderer.SetPosition(1, target.position);
        }
    }
}
