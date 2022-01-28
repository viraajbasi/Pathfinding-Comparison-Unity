using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public string mainProgram;
    public Toggle dijkstraToggle;
    public Toggle astarToggle;
    public Toggle bellmanfordToggle;
    public Toggle recursiveBacktrackerToggle;
    public Toggle kruskalToggle;
    public Toggle userSolvesToggle;
    public GameObject helpScreen;
    public ErrorScreen errorScript;


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
        StoreToggleState(dijkstraToggle.isOn, astarToggle.isOn, bellmanfordToggle.isOn, recursiveBacktrackerToggle.isOn, kruskalToggle.isOn);
        if (dijkstraToggle.isOn | astarToggle.isOn  | bellmanfordToggle.isOn | userSolvesToggle.isOn && recursiveBacktrackerToggle.isOn | kruskalToggle.isOn)
        {
            SceneManager.LoadScene(mainProgram);
        }
        else
        {
            if (recursiveBacktrackerToggle.isOn == false && kruskalToggle.isOn == false)
            {
                errorScript.OpenErrorScreen("Ensure a maze generation algorithm is chosen.");
            }
            else
            {
                errorScript.OpenErrorScreen("Ensure a pathfinding algorithm is chosen.");
            }
        }
    }

    private void OpenHelp()
    {
        helpScreen.SetActive(true);
    }

    private void CloseHelp()
    {
        helpScreen.SetActive(false);
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
