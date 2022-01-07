using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public GameObject errorScreen;
    public TMP_Text errorBody;

    public static bool DijkstraChosen;
    public static bool AStarChosen;
    public static bool BellmanFordChosen;
    public static bool UserSolvesMaze;
    public static bool RecursiveBackrackerChosen;

    public void BeginProgram()
    {
        DijkstraChosen = dijkstraToggle.isOn;
        AStarChosen = astarToggle.isOn;
        BellmanFordChosen = bellmanfordToggle.isOn;
        UserSolvesMaze = userSolvesToggle.isOn;
        RecursiveBackrackerChosen = recursiveBacktrackerToggle.isOn;

        if (DijkstraChosen | AStarChosen  | BellmanFordChosen | UserSolvesMaze && recursiveBacktrackerToggle.isOn | kruskalToggle.isOn)
        {
            SceneManager.LoadScene(mainProgram);
        }
        else
        {
            if (recursiveBacktrackerToggle.isOn == false && kruskalToggle.isOn == false)
            {
                DisplayError("Ensure a maze generation algorithm is chosen.");
            }
            else
            {
                DisplayError("Ensure a pathfinding algorithm is chosen.");
            }
        }
    }

    public void DisplayError(string message)
    {
        errorBody.text = message;
        errorScreen.SetActive(true);
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
