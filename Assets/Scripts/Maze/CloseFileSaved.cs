using UnityEngine;

namespace Maze
{
    public class CloseFileSaved : MonoBehaviour
    {
        public GameObject fileSavedScreen;

        public void CloseFileSavedScreen()
        {
            Time.timeScale = 1f;
            fileSavedScreen.SetActive(false);
        }
    }
}