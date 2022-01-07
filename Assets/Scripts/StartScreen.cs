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

        if (DijkstraChosen | AStarChosen  | BellmanFordChosen | UserSolvesMaze)
        {
            SceneManager.LoadScene(mainProgram);
        }
        else
        {
            // Add code to display error
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
