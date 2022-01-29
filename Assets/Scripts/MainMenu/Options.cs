using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public Toggle FullscreenToggle;
	public Toggle VSyncToggle;
	public TMP_Text ResolutionLabel;
	public AudioMixer Mixer;
	public TMP_Text MasterLabel;
	public TMP_Text MusicLabel;
	public TMP_Text SFXLabel;
	public Slider MasterSlider;
	public Slider MusicSlider;
	public Slider SFXSlider;
	public GameObject OptionsScreen;
	
	private Resolution[] _resolutions;
	private int _selectedResolution;

	public void OpenOptions()
	{
		OptionsScreen.SetActive(true);
	}

	public void CloseOptions()
	{
		OptionsScreen.SetActive(false);
	}
	
	private void Start()
    {
       FullscreenToggle.isOn = Screen.fullScreen;
       VSyncToggle.isOn = QualitySettings.vSyncCount != 0;
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
	   
	   Mixer.GetFloat("MasterVol", out float volume);
	   MasterSlider.value = volume;
	   MasterLabel.text = Mathf.RoundToInt(MasterSlider.value + 80).ToString();
	   Mixer.GetFloat("MusicVol", out volume);
	   MusicSlider.value = volume;
	   MusicLabel.text = Mathf.RoundToInt(MusicSlider.value + 80).ToString();
	   Mixer.GetFloat("SFXVol", out volume);
	   SFXSlider.value = volume;
	   SFXLabel.text = Mathf.RoundToInt(SFXSlider.value + 80).ToString();
    }

    private void ApplyGraphics()
    {
	    QualitySettings.vSyncCount = VSyncToggle.isOn ? 1 : 0;

	    Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, FullscreenToggle.isOn);
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
		ResolutionLabel.text = _resolutions[_selectedResolution].width + " x " + _resolutions[_selectedResolution].height;
	}

	private void SetMasterVolume()
	{
		MasterLabel.text = Mathf.RoundToInt(MasterSlider.value + 80).ToString();
		Mixer.SetFloat("MasterVol", MasterSlider.value);
		
		PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
	}
	
	private void SetMusicVolume()
	{
		MusicLabel.text = Mathf.RoundToInt(MusicSlider.value + 80).ToString();
		Mixer.SetFloat("MusicVol", MusicSlider.value);
		
		PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
	}
	
	private void SetSFXVolume()
	{
		SFXLabel.text = Mathf.RoundToInt(SFXSlider.value + 80).ToString();
		Mixer.SetFloat("SFXVol", SFXSlider.value);
		
		PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
	}
}
