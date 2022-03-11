using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class Begin : MonoBehaviour
    {
        public Toggle dijkstraToggle;
        public Toggle aStarToggle;
        public Toggle bellmanFordToggle;
        public Toggle recursiveBacktrackerToggle;
        public Toggle kruskalToggle;
        public Toggle userSolvesToggle;
        public GameObject startScreen;
        public Error errorScript;

        public void OpenStart()
        {
            startScreen.SetActive(true);
        }

        public void CloseStart()
        {
            startScreen.SetActive(false);
        }
        
        public void BeginProgram()
        {
            if (dijkstraToggle.isOn | aStarToggle.isOn | bellmanFordToggle.isOn | userSolvesToggle.isOn && recursiveBacktrackerToggle.isOn | kruskalToggle.isOn)
            {
                StoreToggleState(dijkstraToggle.isOn, aStarToggle.isOn, bellmanFordToggle.isOn,recursiveBacktrackerToggle.isOn, kruskalToggle.isOn);
                SceneManager.LoadScene("Game");
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
        
        private void Start()
        {
            PlayerPrefs.DeleteKey("Dijkstra");
            PlayerPrefs.DeleteKey("A*");
            PlayerPrefs.DeleteKey("BellmanFord");
            PlayerPrefs.DeleteKey("RecursiveBacktracker");
            PlayerPrefs.DeleteKey("Kruskal");
        }

        private static void StoreToggleState(bool d, bool a, bool bf, bool rb, bool k)
        {
            PlayerPrefs.SetInt("Dijkstra", d ? 1 : 0);
            PlayerPrefs.SetInt("A*", a ? 1 : 0);
            PlayerPrefs.SetInt("BellmanFord", bf ? 1 : 0);
            PlayerPrefs.SetInt("RecursiveBacktracker", rb ? 1 : 0);
            PlayerPrefs.SetInt("Kruskal", k ? 1 : 0);
        }
    }
}
