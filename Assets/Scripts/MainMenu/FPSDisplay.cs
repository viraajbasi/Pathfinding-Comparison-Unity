using System.Net;
using UnityEngine;
using TMPro;

namespace MainMenu
{
    public class FPSDisplay : MonoBehaviour
    {
        private const float PollingTime = 1f; // Determines speed at which FPS is updated.
        
        public TMP_Text fpsText;
        public GameObject fpsObject;
        
        private float _time;
        private int _frameCount;

        private void Update()
        {
            if (PlayerPrefs.GetInt("FPS") == 0)
            {
                // If FPS is disabled then turn off the counter.
                fpsObject.SetActive(false);
                
                // Prevent rest of Update() from being executed unnecessarily.
                return;
            }

            // Determines time (in seconds) between the last frame and the current one. 
            _time += Time.deltaTime;
           
            // Increments the frame count.
            _frameCount++;

            if (_time >= PollingTime) // Only update FPS if it reaches the required threshold.
            {
                // Round to integer since we do not want to provide a fractional framerate.
                var frameRate = Mathf.RoundToInt(_frameCount / _time);
                
                // Update counter text.
                fpsText.text = $"{frameRate} FPS";
                
                _time -= PollingTime;
                _frameCount = 0;
            }
        }
    }
}
