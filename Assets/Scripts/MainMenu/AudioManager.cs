using UnityEngine;
using UnityEngine.Audio;

namespace MainMenu
{
    public class AudioManager : MonoBehaviour
    {
        /*
         * Sets the volume to the value stored in PlayerPrefs.
         * Ensures that user options are preserved when the program is restarted.
         */
        
        public AudioMixer mixer;

        private void Start()
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVolume"));
            }

            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolume"));
            }

            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVolume"));
            }
        }
    }
}
