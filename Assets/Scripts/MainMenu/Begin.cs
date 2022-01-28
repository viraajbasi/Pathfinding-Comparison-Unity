using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Begin : MonoBehaviour
{
    public Toggle DijkstraToggle;
    public Toggle AStarToggle;
    public Toggle BellmanFordToggle;
    public Toggle RecursiveBacktrackerToggle;
    public Toggle KruskalToggle;
    public Toggle UserSolvesToggle;
    public GameObject HelpScreen;
    public GameObject StartScreen;
    public Error ErrorScript;

	public void OpenStart()
	{
		StartScreen.SetActive(true);
	}

	public void CloseStart()
	{
		StartScreen.SetActive(false);
	}

    private void Start()
    {
        PlayerPrefs.DeleteKey("Dijkstra");
        PlayerPrefs.DeleteKey("A*");
        PlayerPrefs.DeleteKey("BellmanFord");
        PlayerPrefs.DeleteKey("RecursiveBacktracker");
        PlayerPrefs.DeleteKey("Kruskal");
    }

    private void BeginProgram()
    {
        StoreToggleState(DijkstraToggle.isOn, AStarToggle.isOn, BellmanFordToggle.isOn, RecursiveBacktrackerToggle.isOn, KruskalToggle.isOn);
        if (DijkstraToggle.isOn | AStarToggle.isOn  | BellmanFordToggle.isOn | UserSolvesToggle.isOn && RecursiveBacktrackerToggle.isOn | KruskalToggle.isOn)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            if (RecursiveBacktrackerToggle.isOn == false && KruskalToggle.isOn == false)
            {
                ErrorScript.OpenErrorScreen("Ensure a maze generation algorithm is chosen.");
            }
            else
            {
                ErrorScript.OpenErrorScreen("Ensure a pathfinding algorithm is chosen.");
            }
        }
    }

    private void OpenHelp()
    {
        HelpScreen.SetActive(true);
    }

    private void CloseHelp()
    {
        HelpScreen.SetActive(false);
    }

    private void StoreToggleState(bool d, bool a, bool bf, bool rb, bool k)
    {
        PlayerPrefs.SetInt("Dijkstra", (d ? 1 : 0));
        PlayerPrefs.SetInt("A*", (a ? 1 : 0));
        PlayerPrefs.SetInt("BellmanFord", (bf ? 1 : 0));
        PlayerPrefs.SetInt("RecursiveBacktracker", (rb ? 1 : 0));
        PlayerPrefs.SetInt("Kruskal", (k ? 1 : 0));
    }
}
