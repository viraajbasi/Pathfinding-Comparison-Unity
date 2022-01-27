using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog;
	public Toggle vsyncTog;
	public TMP_Text resolutionLabel;
	public AudioMixer theMixer;
	public TMP_Text masterLabel;
	public TMP_Text musicLabel;
	public TMP_Text sfxLabel;
	public Slider masterSlider;
	public Slider musicSlider;
	public Slider sfxSlider;
	
	private Resolution[] _resolutions;
	private int _selectedResolution;

	// Start is called before the first frame update
    private void Start()
    {
       fullscreenTog.isOn = Screen.fullScreen;
       vsyncTog.isOn = QualitySettings.vSyncCount != 0;
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
	   
	   theMixer.GetFloat("MasterVol", out float volume);
	   masterSlider.value = volume;
	   masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
	   theMixer.GetFloat("MusicVol", out volume);
	   musicSlider.value = volume;
	   musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
	   theMixer.GetFloat("SFXVol", out volume);
	   sfxSlider.value = volume;
	   sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    private void ApplyGraphics()
    {
	    QualitySettings.vSyncCount = vsyncTog.isOn ? 1 : 0;

	    Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, fullscreenTog.isOn);
    }

	private void ResLeft()
	{
		_selectedResolution--;

		if (_selectedResolution < 0)
		{
			_selectedResolution = _resolutions.Length - 1;
		}

		UpdateResLabel();
	}

	private void ResRight()
	{
		_selectedResolution++;

		if (_selectedResolution > _resolutions.Length - 1)
		{
			_selectedResolution = 0;
		}

		UpdateResLabel();
	}

	private void UpdateResLabel()
	{
		resolutionLabel.text = _resolutions[_selectedResolution].width + " x " + _resolutions[_selectedResolution].height;
	}

	private void SetMasterVolume()
	{
		masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
		theMixer.SetFloat("MasterVol", masterSlider.value);
		
		PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
	}
	
	private void SetMusicVolume()
	{
		musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
		theMixer.SetFloat("MusicVol", musicSlider.value);
		
		PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
	}
	
	private void SetSFXVolume()
	{
		sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
		theMixer.SetFloat("SFXVol", sfxSlider.value);
		
		PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
	}
}
