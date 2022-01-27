using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public GameObject optionsScreen;
	public GameObject aboutScreen;
	public GameObject startScreen;

	private void OpenStart()
	{
		startScreen.SetActive(true);
	}

	private void CloseStart()
	{
		startScreen.SetActive(false);
	}
	
	private void OpenOptions()
	{
		optionsScreen.SetActive(true);
	}

	private void CloseOptions()
	{
		optionsScreen.SetActive(false);
	}

	private void OpenAbout()
	{
		aboutScreen.SetActive(true);
	}

	private	void CloseAbout()
	{
		aboutScreen.SetActive(false);
	}

	private void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quitting...");
	}
}
