using UnityEngine;
using TMPro;

namespace MainMenu
{
    public class Help : MonoBehaviour
    {
        public GameObject helpScreen;

        public void OpenHelpScreen()
        {
            helpScreen.SetActive(true);
        }

        public void CloseHelpScreen()
        {
            helpScreen.SetActive(false);
        }
    }
}
