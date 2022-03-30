using TMPro;
using UnityEngine;

namespace Maze
{
    public class FileSaved : MonoBehaviour
    {
        public GameObject fileSavedScreen;
        public TMP_Text message;
        
        public void CloseFileSavedScreen()
        {
            Time.timeScale = 1f;
            fileSavedScreen.SetActive(false);
        }

        private void ChangeMessage()
        {
            var fileLocation = PlayerPrefs.GetString("FileLocation");
            message.text = $"The statistics file was saved to '{fileLocation}'";
        }
    }
}