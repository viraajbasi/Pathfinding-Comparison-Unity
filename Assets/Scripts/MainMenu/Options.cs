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
			QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;

			Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, fullscreenToggle.isOn);
			
			PlayerPrefs.SetInt("FPS", fpsToggle.isOn ? 1 : 0);
		}

		public void ResLeft()
		{
			_selectedResolution--;

			if (_selectedResolution < 0)
			{
				_selectedResolution = _resolutions.Length - 1;
			}

			UpdateResLabel();
		}

		public void ResRight()
		{
			_selectedResolution++;

			if (_selectedResolution > _resolutions.Length - 1)
			{
				_selectedResolution = 0;
			}

			UpdateResLabel();
		}

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
			fullscreenToggle.isOn = Screen.fullScreen;
			vSyncToggle.isOn = QualitySettings.vSyncCount != 0;
			fpsToggle.isOn = PlayerPrefs.GetInt("FPS") == 1;
			_resolutions = Screen.resolutions;

			for (int i = 0; i < _resolutions.Length; i++)
			{
				if (Screen.width == _resolutions[i].width && Screen.height == _resolutions[i].height)
				{
					_selectedResolution = i;
					UpdateResLabel();
					break;
				}
			}

			mixer.GetFloat("MasterVol", out float volume);
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
