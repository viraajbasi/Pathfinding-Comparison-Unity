using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class Begin : MonoBehaviour
    {
        public Toggle pathfindingToggle;
        public Toggle userSolvesToggle;
        public Toggle kruskalToggle;
        public Toggle recursiveBacktrackerToggle;
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
            if (pathfindingToggle.isOn | userSolvesToggle.isOn && kruskalToggle.isOn | recursiveBacktrackerToggle.isOn)
            {
                StoreToggleState(pathfindingToggle.isOn, recursiveBacktrackerToggle.isOn);
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
            PlayerPrefs.DeleteKey("Pathfinding");
            PlayerPrefs.DeleteKey("UserSolves");
            PlayerPrefs.DeleteKey("RecursiveBacktracker");
            PlayerPrefs.DeleteKey("Kruskal");
        }

        private static void StoreToggleState(bool pathfinding, bool recursiveBacktracker)
        {
            PlayerPrefs.SetInt("Pathfinding", pathfinding ? 1 : 0);
            PlayerPrefs.SetInt("UserSolves", !pathfinding ? 1 : 0);
            PlayerPrefs.SetInt("RecursiveBacktraker", recursiveBacktracker ? 1 : 0);
            PlayerPrefs.SetInt("Kruskal", !recursiveBacktracker ? 1 : 0);
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
