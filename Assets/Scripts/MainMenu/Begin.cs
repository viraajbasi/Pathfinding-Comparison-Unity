using System.Collections;
using TMPro;
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
        public GameObject loadingScreen;
        public GameObject errorScreen;
        public Slider loadingBar;
        public TMP_Text errorMessage;

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
                StartCoroutine(LoadAsync("Game"));
            }
            else
            {
                if (recursiveBacktrackerToggle.isOn == false && kruskalToggle.isOn == false)
                {
                    OpenErrorScreen("Ensure a maze generation algorithm is chosen.");
                }
                else
                {
                    OpenErrorScreen("Ensure a pathfinding algorithm is chosen.");
                }
            }
        }
        
        public void CloseErrorScreen()
        {
            errorScreen.SetActive(false);
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

        private IEnumerator LoadAsync(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            loadingScreen.SetActive(true);
        
            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBar.value = progress;
            
                yield return null;
            }
        }
        
        private void OpenErrorScreen(string message)
        {
            errorMessage.text = message;
            errorScreen.SetActive(true);
        }
    }
}
