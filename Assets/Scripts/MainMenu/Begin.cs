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
        public GameObject startScreen;
        public GameObject loadingScreen;
        public GameObject errorScreen;
        public Slider loadingBar;
        public TMP_Text errorMessage;
        public TMP_InputField widthInput;
        public TMP_InputField heightInput;
        public TMP_Text widthText;
        public TMP_Text heightText;

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
            if ((pathfindingToggle.isOn || userSolvesToggle.isOn))
            {
                StoreToggleState(pathfindingToggle.isOn);
                StartCoroutine(LoadAsync("Game"));
            }
            else
            {
                OpenErrorScreen("Ensure a pathfinding algorithm is chosen.");
            }
        }

        public void SetWidthAndHeight()
        {
            widthText.text = $"Current Maze Width: {widthInput.text}";
            heightText.text = $"Current Maze Width: {heightInput.text}";
        }
        
        public void CloseErrorScreen()
        {
            errorScreen.SetActive(false);
        }

        private void Start()
        {
            PlayerPrefs.DeleteKey("Pathfinding");
            PlayerPrefs.DeleteKey("UserSolves");
        }

        private static void StoreToggleState(bool pathfinding)
        {
            PlayerPrefs.SetInt("Pathfinding", pathfinding ? 1 : 0);
            PlayerPrefs.SetInt("UserSolves", !pathfinding ? 1 : 0);
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
