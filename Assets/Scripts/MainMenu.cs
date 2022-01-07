using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public GameObject optionsScreen;
	public GameObject aboutScreen;
	public GameObject startScreen;
	public GameObject errorScreen;

	public void OpenStart()
	{
		startScreen.SetActive(true);
	}

	public void CloseStart()
	{
		startScreen.SetActive(false);
	}
	
	public void OpenOptions()
	{
		optionsScreen.SetActive(true);
	}

	public void CloseOptions()
	{
		optionsScreen.SetActive(false);
	}

	public void OpenAbout()
	{
		aboutScreen.SetActive(true);
	}

	public void CloseAbout()
	{
		aboutScreen.SetActive(false);
	}

	public void CloseError()
	{
		errorScreen.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quitting...");
	}
}
