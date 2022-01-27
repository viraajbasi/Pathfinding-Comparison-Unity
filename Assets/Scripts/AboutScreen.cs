using UnityEngine;
using TMPro;
using System.IO;

public class AboutScreen : MonoBehaviour
{
    public TMP_Text algorithmInformation;
    
    private void Start()
    {
        algorithmInformation.text = "Choose an algorithm.";
    }
    
    private void DijkstraInfo()
    {
        ReadInfoAndShow("Dijkstra.txt");
    }
    
    private void AStarInfo()
    {
        ReadInfoAndShow("AStar.txt");
    }
    
    private void BellmanFordInfo()
    {
        ReadInfoAndShow("BellmanFord.txt");
    }
    
    private void RecursiveBacktrackerInfo()
    {
        ReadInfoAndShow("RecursiveBacktracker.txt");
    }
    
    private void KruskalInfo()
    {
        ReadInfoAndShow("Kruskal.txt");
    }
    
    private void ReadInfoAndShow(string filename)
    {
        var streamReader = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
        algorithmInformation.text = streamReader.ReadToEnd();
    }
}
