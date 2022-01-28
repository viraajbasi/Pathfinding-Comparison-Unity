using UnityEngine;
using TMPro;

public class Error : MonoBehaviour
{
    public GameObject ErrorScreen;
    public TMP_Text ErrorMessage;
    
    public void OpenErrorScreen(string message)
    {
        ErrorMessage.text = message;
        ErrorScreen.SetActive(true);
    }

    public void CloseErrorScreen()
    {
        ErrorScreen.SetActive(false);
    }
}
