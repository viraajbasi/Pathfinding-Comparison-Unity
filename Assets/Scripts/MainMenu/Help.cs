using UnityEngine;

public class Help : MonoBehaviour
{
    public GameObject HelpScreen;
    
    public void OpenHelpScreen()
    {
        HelpScreen.SetActive(true);
    }

    public void CloseHelpScreen()
    {
        HelpScreen.SetActive(false);
    }
}
