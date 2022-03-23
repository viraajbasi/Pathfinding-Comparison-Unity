using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameCompleted;
        public GameObject pauseMenu;
        private bool _isPaused;
        
        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
        }

        public void ReturnToMainMenu()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
        
        private void Pause(bool gameCompleted)
        {
            if (!gameCompleted)
            {
                pauseMenu.SetActive(true);
            }

            Time.timeScale = 0f;
            _isPaused = true;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause(GameCompleted);
                }
            }

            if (GameCompleted)
            {
                Pause(GameCompleted);
            }
        }
    }
}
