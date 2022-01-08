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

    public void BeginProgram()
    {
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

    public void OpenHelp()
    {
        helpScreen.SetActive(true);
    }

    public void CloseHelp()
    {
        helpScreen.SetActive(false);
    }
}
