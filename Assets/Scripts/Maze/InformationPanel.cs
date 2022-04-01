using TMPro;
using UnityEngine;

namespace Maze
{
    public class InformationPanel : MonoBehaviour
    {
        public TMP_Text algorithmNameObject;
        public TMP_Text totalNodesObject;
        public TMP_Text totalVisitedNodesObject;
        public TMP_Text totalPathNodesObject;
        public TMP_Text algorithmTimeTakenObject;
        public TMP_Text totalTimeTakenObject;
        public TMP_Text averageTimeTakenObject;
        
        private static string _algorithmName;
        private static string _totalNodes;
        private static string _totalVisitedNodes;
        private static string _totalPathNodes;
        private static string _algorithmTimeTaken;
        private static string _totalTimeTaken;
        private static string _averageTimeTaken;

        public static void UpdateLabels(string algName, string totNodes, string totVisNodes, string totPathNodes, string algTimeTaken, string totTimeTaken, string avgTimeTaken)
        {
            _algorithmName = $"Current Algorithm: {algName}";
            _totalNodes = $"Total Nodes: {totNodes}";
            _totalVisitedNodes = $"Total Visited Nodes: {totVisNodes}";
            _totalPathNodes = $"Total Nodes in Path {totPathNodes}";
            _algorithmTimeTaken = $"Time Taken to Find Path: {algTimeTaken}ms";
            _totalTimeTaken = $"Total Time Taken for All Algorithms: {totTimeTaken}ms";
            _averageTimeTaken = $"Average Time Taken for All Algorithms: {avgTimeTaken}ms";
        }
        
        private void Update()
        {
            algorithmNameObject.text = _algorithmName;
            totalNodesObject.text = _totalNodes;
            totalVisitedNodesObject.text = _totalVisitedNodes;
            totalPathNodesObject.text = _totalPathNodes;
            algorithmTimeTakenObject.text = _algorithmTimeTaken;
            totalTimeTakenObject.text = _totalTimeTaken;
            averageTimeTakenObject.text = _averageTimeTaken;
        }
    }
}
