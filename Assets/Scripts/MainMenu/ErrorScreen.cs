using UnityEngine;
using TMPro;

public class ErrorScreen : MonoBehaviour
{
    public GameObject errorScreen;
    public TMP_Text errorMessage;
    
    public void OpenErrorScreen(string message)
    {
        errorMessage.text = message;
        errorScreen.SetActive(true);
    }

    private void CloseErrorScreen()
    {
        errorScreen.SetActive(false);
    }
}
