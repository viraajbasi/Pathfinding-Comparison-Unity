using UnityEngine;
using TMPro;
using System.IO;

public class AboutScreen : MonoBehaviour
{
    public TMP_Text algorithmInformation;

    public void Start()
    {
        algorithmInformation.text = "Choose an algorithm.";
    }
    
    public void DijkstraInfo()
    {
        ReadInfoAndShow("Dijkstra");
    }
    
    public void AStarInfo()
    {
        ReadInfoAndShow("AStar");
    }
    
    public void BellmanFordInfo()
    {
        ReadInfoAndShow("BellmanFord");
    }
    
    public void RecursiveBacktrackerInfo()
    {
        ReadInfoAndShow("RecursiveBacktracker");
    }
    
    public void KruskalInfo()
    {
        ReadInfoAndShow("Kruskal");
    }
    
    private void ReadInfoAndShow(string filename)
    {
        string path = $"Assets/Resources/{filename}.txt";
        StreamReader streamReader = new StreamReader(path);

        algorithmInformation.text = streamReader.ReadToEnd();
    }
}
