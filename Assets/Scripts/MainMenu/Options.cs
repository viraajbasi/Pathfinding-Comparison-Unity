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
	
	private Resolution[] resolutions;
	private int selectedResolution;

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
       resolutions = Screen.resolutions;
       
	   for (int i = 0; i < resolutions.Length; i++)
	   {
		   if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
		   {
			   selectedResolution = i;
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

	    Screen.SetResolution(resolutions[selectedResolution].width, resolutions[selectedResolution].height, FullscreenToggle.isOn);
    }

	private void ResLeft()
	{
		selectedResolution--;

		if (selectedResolution < 0)
		{
			selectedResolution = resolutions.Length - 1;
		}

		UpdateResLabel();
	}

	private void ResRight()
	{
		selectedResolution++;

		if (selectedResolution > resolutions.Length - 1)
		{
			selectedResolution = 0;
		}

		UpdateResLabel();
	}

	private void UpdateResLabel()
	{
		ResolutionLabel.text = resolutions[selectedResolution].width + " x " + resolutions[selectedResolution].height;
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
