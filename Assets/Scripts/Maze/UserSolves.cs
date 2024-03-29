using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Maze
{
    public class UserSolves : MonoBehaviour
    {
        private static readonly List<MazeCell> VisitedList = new List<MazeCell>();
        private static Position _startPosition;
        private static Stopwatch _stopwatch;

        public static void HandleKeyInput(List<MazeCell> mazeList, Color defaultFloorColour, AudioSource audioSource, GameObject completedScreen, TMP_Text userSolvesNodeText)
        {
            var currentNode = mazeList.Find(a => a.Coordinates.X == _startPosition.X && a.Coordinates.Y == _startPosition.Y);
            currentNode.Visited = true;

            if (!VisitedList.Contains(currentNode)) // Adds node to list for counting purposes.
            {
                VisitedList.Add(currentNode);
            }
            
            // Print list count to Unity Editor console.
            print(VisitedList.Count);
            
            userSolvesNodeText.text = $"Total Visited Nodes: {VisitedList.Count}";
            
            // Stops if the current node is the goal node.
            if (currentNode.GoalNode)
            {
                Time.timeScale = 0f;
                completedScreen.SetActive(true);
            }

            // Reset back to start node.
            // Movements are in a single if statement to ensure only one input per frame.
            if (Input.GetKey(KeyCode.Q))
            {
                audioSource.Play();
                _startPosition = ResetToStart(mazeList, defaultFloorColour);
            } 
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W))
            {
                var topOffset = new Position(_startPosition.X, _startPosition.Y + 1);
                var topNode = mazeList.Find(a => a.Coordinates.X == topOffset.X && a.Coordinates.Y == topOffset.Y);
                
                audioSource.Play();
                
                if (!currentNode.Top)
                {
                    VisitAndColour(topOffset, currentNode, topNode);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.A))
            {
                if (_startPosition.X > 0)
                {
                    var leftOffset = new Position(_startPosition.X - 1, _startPosition.Y);
                    var leftNode = mazeList.Find(a => a.Coordinates.X == leftOffset.X && a.Coordinates.Y == leftOffset.Y);

                    audioSource.Play();
                    
                    if (!currentNode.Left && _startPosition.X >= 0)
                    {
                        VisitAndColour(leftOffset, currentNode, leftNode);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.D))
            {
                var rightOffset = new Position(_startPosition.X + 1, _startPosition.Y);
                var rightNode = mazeList.Find(a => a.Coordinates.X == rightOffset.X && a.Coordinates.Y == rightOffset.Y);
                
                audioSource.Play();
                
                if (!currentNode.Right)
                {
                    VisitAndColour(rightOffset, currentNode, rightNode);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.S))
            {
                if (_startPosition.Y > 0)
                {
                    var bottomOffset = new Position(_startPosition.X, _startPosition.Y - 1);
                    var bottomNode = mazeList.Find(a => a.Coordinates.X == bottomOffset.X && a.Coordinates.Y == bottomOffset.Y);
                    
                    audioSource.Play();
                    
                    if (!currentNode.Bottom && _startPosition.Y >= 0)
                    {
                        VisitAndColour(bottomOffset, currentNode, bottomNode);
                    }
                }
            }
        }
        
        private static void VisitAndColour(Position pos, MazeCell currentNode, MazeCell nextNode) // Set node as visited and colour accordingly.
        {
            currentNode.Floor.gameObject.GetComponent<Renderer>().material.color = Color.black;
            nextNode.Floor.gameObject.GetComponent<Renderer>().material.color = Color.white;
            
            currentNode.Floor.gameObject.SetActive(true);
            nextNode.Floor.gameObject.SetActive(true);

            _startPosition = pos;
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

        private void Start()
        {
            // Set start position to the bottom left of the maze.
            _startPosition = new Position(0, 0);
            
            // Ensure visited list is empty.
            VisitedList.Clear();
        }
    }
}
