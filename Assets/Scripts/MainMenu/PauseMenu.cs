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
            Time.timeScale = 1f;
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
        
        private void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        private void Start()
        {
            Resume();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
