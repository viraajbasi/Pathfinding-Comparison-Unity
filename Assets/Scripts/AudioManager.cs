using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer theMixer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            theMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVolume"));
        }
        
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolume"));
        }
        
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            theMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVolume"));
        }
    }
}
