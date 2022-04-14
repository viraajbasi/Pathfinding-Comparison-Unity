using UnityEngine;
using TMPro;
using System.IO;

namespace MainMenu
{
    public class About : MonoBehaviour
    {
        /*
         * The various text files are project files.
         * They contain the information that is displayed to the user.
         */
        
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

        private void ReadInfoAndShow(string filename)
        {
            // Reads from the specified text file in the StreamingAssets folder in the project.
            var streamReader = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
            
            // Reads the entire file into the text box.
            algorithmInformation.text = streamReader.ReadToEnd();
        }
    }
}
