using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer Mixer;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
        Mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVolume"));
        }
        
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            Mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolume"));
        }
        
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            Mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVolume"));
        }
    }
}
