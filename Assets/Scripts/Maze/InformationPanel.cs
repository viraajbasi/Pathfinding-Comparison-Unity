using TMPro;
using TMPro.EditorUtilities;
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
            _algorithmName = algName;
            _totalNodes = totNodes;
            _totalVisitedNodes = totVisNodes;
            _totalPathNodes = totPathNodes;
            _algorithmTimeTaken = algTimeTaken;
            _totalTimeTaken = totTimeTaken;
            _averageTimeTaken = avgTimeTaken;
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
