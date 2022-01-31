using UnityEngine;
using TMPro;

namespace MainMenu
{
    public class Error : MonoBehaviour
    {
        public GameObject errorScreen;
        public TMP_Text errorMessage;

        public void OpenErrorScreen(string message)
        {
            errorMessage.text = message;
            errorScreen.SetActive(true);
        }

        public void CloseErrorScreen()
        {
            errorScreen.SetActive(false);
        }
    }
}
