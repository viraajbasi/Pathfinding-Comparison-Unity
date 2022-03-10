using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool IsPaused = false;
        public GameObject pauseMenu;


        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        }

        public void ReturnToMainMenu()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
        
        private void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }
}
