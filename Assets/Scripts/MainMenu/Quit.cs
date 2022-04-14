using UnityEngine;

namespace MainMenu
{
	public class Quit : MonoBehaviour
	{
		public void QuitGame()
		{
			Application.Quit();
			
			// In the Unity Editor, Application.Quit() has no effect.
			// A Debug.Log is required to make sure that the expected behaviour is experienced.
			Debug.Log("Quitting...");
		}
	}
}
