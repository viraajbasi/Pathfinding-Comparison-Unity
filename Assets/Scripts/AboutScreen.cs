using UnityEngine;
using TMPro;
using System.IO;

public class AboutScreen : MonoBehaviour
{
    public TMP_Text algorithmInformation;
    public string DataPath;
    
    private void Start()
    {
        algorithmInformation.text = "Choose an algorithm.";
        // Find data path
        DataPath = Application.dataPath;
        // Output the Game data path to the console
        Debug.Log("dataPath : " + DataPath);
    }
    
    private void DijkstraInfo()
    {
        ReadInfoAndShow("Dijkstra");
    }
    
    private void AStarInfo()
    {
        ReadInfoAndShow("AStar");
    }
    
    private void BellmanFordInfo()
    {
        ReadInfoAndShow("BellmanFord");
    }
    
    private void RecursiveBacktrackerInfo()
    {
        ReadInfoAndShow("RecursiveBacktracker");
    }
    
    private void KruskalInfo()
    {
        ReadInfoAndShow("Kruskal");
    }
    
    private void ReadInfoAndShow(string filename)
    {
        string path = $"{DataPath}/Resources/{filename}.txt";
        StreamReader streamReader = new StreamReader(path);

        algorithmInformation.text = streamReader.ReadToEnd();
    }
}
