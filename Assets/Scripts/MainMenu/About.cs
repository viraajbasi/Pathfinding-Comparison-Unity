using UnityEngine;
using TMPro;
using System.IO;

namespace MainMenu
{
    public class About : MonoBehaviour
    {
        public TMP_Text algorithmInformation;
        public GameObject aboutScreen;

        public void OpenAbout()
        {
            aboutScreen.SetActive(true);
        }

        public void CloseAbout()
        {
            aboutScreen.SetActive(false);
            algorithmInformation.text = "Choose an algorithm...";
        }

        public void DijkstraInfo()
        {
            ReadInfoAndShow("Dijkstra.txt");
        }

        public void AStarInfo()
        {
            ReadInfoAndShow("AStar.txt");
        }

        public void BellmanFordInfo()
        {
            ReadInfoAndShow("BellmanFord.txt");
        }

        public void RecursiveBacktrackerInfo()
        {
            ReadInfoAndShow("RecursiveBacktracker.txt");
        }

        public void KruskalInfo()
        {
            ReadInfoAndShow("Kruskal.txt");
        }

        private void ReadInfoAndShow(string filename)
        {
            var streamReader = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
            algorithmInformation.text = streamReader.ReadToEnd();
        }
    }
}
