using UnityEngine;

namespace MainMenu
{
	public class Quit : MonoBehaviour
	{
		public void QuitGame()
		{
			Application.Quit();
			Debug.Log("Quitting...");
		}
	}
}
