using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

namespace MainMenu
{
	public class Options : MonoBehaviour
	{
		public Toggle fullscreenToggle;
		public Toggle vSyncToggle;
		public Toggle fpsToggle;
		public TMP_Text resolutionLabel;
		public AudioMixer mixer;
		public TMP_Text masterLabel;
		public TMP_Text musicLabel;
		public TMP_Text sfxLabel;
		public Slider masterSlider;
		public Slider musicSlider;
		public Slider sfxSlider;
		public GameObject optionsScreen;

		// Contains all compatible resolutions.
		private Resolution[] _resolutions;
		private int _selectedResolution;

		public void OpenOptions()
		{
			optionsScreen.SetActive(true);
		}

		public void CloseOptions()
		{
			optionsScreen.SetActive(false);
		}

		public void ApplyGraphics()
		{
			// Determines VSync option based on the toggle state.
			// If vSyncCount = 1, then VSync is on.
			// If vSyncCount = 0, then VSync is off.
			QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;

			// Finds the selected resolution in the _resolutions[] array.
			Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, fullscreenToggle.isOn);
			
			// Determines whether to show or hide FPS based on the toggle state.
			PlayerPrefs.SetInt("FPS", fpsToggle.isOn ? 1 : 0);
		}

		public void ResLeft()
		{
			_selectedResolution--;

			if (_selectedResolution < 0) // Wraps navigation around.
			{
				_selectedResolution = _resolutions.Length - 1;
			}

			// Display correct resolution in label.
			UpdateResLabel();
		}

		public void ResRight()
		{
			_selectedResolution++;

			if (_selectedResolution > _resolutions.Length - 1) // Wraps navigation around.
			{
				_selectedResolution = 0;
			}

			// Displays correct resolution in label.
			UpdateResLabel();
		}

		/*
		 * The volume value returned by the mixer is offset by a value of -80.
		 * Therefore, we need to add 80 to it to ensure that the value is always between 0 and 100.
		 * The actual value is stored in PlayerPrefs.
		 */
		
		public void SetMasterVolume()
		{
			masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
			mixer.SetFloat("MasterVol", masterSlider.value);

			PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
		}

		public void SetMusicVolume()
		{
			musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
			mixer.SetFloat("MusicVol", musicSlider.value);

			PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
		}

		public void SetSfxVolume()
		{
			sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
			mixer.SetFloat("SFXVol", sfxSlider.value);

			PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
		}

		public void ResetSound()
		{
			masterSlider.value = 0;
			musicSlider.value = 0;
			sfxSlider.value = 0;
			
			SetMasterVolume();
			SetMusicVolume();
			SetSfxVolume();
		}
		
		private void UpdateResLabel()
		{
			resolutionLabel.text = _resolutions[_selectedResolution].width + " x " + _resolutions[_selectedResolution].height;
		}
		
		private void Start()
		{
			// Ensures that the main menu is restricted to 60fps.
			// There is no need for the framerate to be unlocked on the main menu.
			Application.targetFrameRate = 60;
			
			// Checks whether the screen is currently fullscreen and assigns the toggle accordingly.
			fullscreenToggle.isOn = Screen.fullScreen;
			
			// Checks whether VSync is enabled and updates the toggle accordingly.
			vSyncToggle.isOn = QualitySettings.vSyncCount != 0;
			
			// Checks PlayerPrefs to see whether the user has enabled FPS counter previously.
			fpsToggle.isOn = PlayerPrefs.GetInt("FPS") == 1;
			
			// Gathers all compatible resolutions.
			_resolutions = Screen.resolutions;

			// Finds current resolution in _resolutions[] and updates label and _selectedResolution accordingly.
			for (var i = 0; i < _resolutions.Length; i++)
			{
				if (Screen.width == _resolutions[i].width && Screen.height == _resolutions[i].height)
				{
					_selectedResolution = i;
					UpdateResLabel();
					break;
				}
			}
			
			// Gets slider values from the mixer and updates the label.
			mixer.GetFloat("MasterVol", out var volume);
			masterSlider.value = volume;
			masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
			mixer.GetFloat("MusicVol", out volume);
			musicSlider.value = volume;
			musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
			mixer.GetFloat("SFXVol", out volume);
			sfxSlider.value = volume;
			sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
		}
	}
}
