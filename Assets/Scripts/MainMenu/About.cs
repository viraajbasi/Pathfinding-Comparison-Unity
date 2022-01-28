using UnityEngine;
using TMPro;
using System.IO;

public class About : MonoBehaviour
{
    public TMP_Text AlgorithmInformation;
    public GameObject AboutScreen;
    
    public void OpenAbout()
	{
		AboutScreen.SetActive(true);
	}

	public	void CloseAbout()
	{
		AboutScreen.SetActive(false);
	}
    
    private void Start()
    {
        AlgorithmInformation.text = "Choose an algorithm.";
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
        AlgorithmInformation.text = streamReader.ReadToEnd();
    }
}
