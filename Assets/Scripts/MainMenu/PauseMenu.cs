using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;

        public void Resume()
        {
            pauseMenu.SetActive(false);
            
            // Resumes time.
            Time.timeScale = 1f;
        }

        public void ReturnToMainMenu()
        {
            // Ensures that time is resumed before returning.
            Time.timeScale = 1f;
            
            SceneManager.LoadScene("Menu");
        }
        
        private void Pause()
        {
            pauseMenu.SetActive(true);
            
            // Stops time.
            Time.timeScale = 0f;
        }

        private void Start()
        {
            // Ensures that time is not paused by default.
            Resume();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // Checks whether escape is pressed and pauses or unpauses accordingly.
            {
                if (Time.timeScale == 0f)
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
