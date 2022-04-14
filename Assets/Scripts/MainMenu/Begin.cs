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
                
                // If executed as a coroutine, the progress can be displayed in the loading screen.
                StartCoroutine(LoadAsync("Game"));
            }
            else
            {
                // Ensures that one of the options is chosen.
                OpenErrorScreen("Choose at least one mode.");
            }
        }

        public void SetWidthAndHeight()
        {
            int width;
            int height;
            int.TryParse(widthInput.text, out width);
            int.TryParse(heightInput.text, out height);
            
            if (width is <= 100 and >= 10) // Makes sure that the width is within range.
            {
                PlayerPrefs.SetInt("Width", width);
            }
            else
            {
                PlayerPrefs.SetInt("Width", 20);
            }

            if (height is <= 100 and >= 10) // Makes sure that the height is within range.
            {
                PlayerPrefs.SetInt("Height", height);
            }
            else
            {
                PlayerPrefs.SetInt("Height", 20);
            }
            
            widthText.text = $"Current Maze Width: {PlayerPrefs.GetInt("Width")}";
            heightText.text = $"Current Maze Height: {PlayerPrefs.GetInt("Height")}";
        }
        
        public void CloseErrorScreen()
        {
            errorScreen.SetActive(false);
        }

        private void Start()
        {
            // Deletes any values in PlayerPrefs to prevent errors.
            // Could arise from the user quitting and relaunching.
            PlayerPrefs.DeleteKey("Pathfinding");
            PlayerPrefs.DeleteKey("UserSolves");
            
            // Sets the default width and height to 20.
            // Also resets width and height to prevent any possible errors.
            PlayerPrefs.SetInt("Width", 20);
            PlayerPrefs.SetInt("Height", 20);

            // Prevents user from entering anything other than an integer in the width and height field.
            heightInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
            widthInput.characterValidation = TMP_InputField.CharacterValidation.Integer;

            widthText.text = $"Current Maze Width: {PlayerPrefs.GetInt("Width")}";
            heightText.text = $"Current Maze Height: {PlayerPrefs.GetInt("Height")}";
        }

        private static void StoreToggleState(bool pathfinding)
        {
            // Stores the correct state of the toggle for comparisons later on.
            PlayerPrefs.SetInt("Pathfinding", pathfinding ? 1 : 0);
            PlayerPrefs.SetInt("UserSolves", !pathfinding ? 1 : 0);
        }

        private IEnumerator LoadAsync(string sceneName) // Uses an IEnumerator to update the loading bar as the scene loads.
        {
            // LoadSceneAsync() has a progress value between 0 and 1.
            var operation = SceneManager.LoadSceneAsync(sceneName);
            loadingScreen.SetActive(true);
        
            while (!operation.isDone)
            {
                loadingBar.value = operation.progress;
            
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
