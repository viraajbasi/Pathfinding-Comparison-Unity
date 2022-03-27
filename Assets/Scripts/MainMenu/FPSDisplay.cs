using UnityEngine;
using TMPro;

namespace MainMenu
{
    public class FPSDisplay : MonoBehaviour
    {
        private const float PollingTime = 1f;
        
        public TMP_Text fpsText;
        public GameObject fpsObject;
        
        private float _time;
        private int _frameCount;

        private void Update()
        {
            if (PlayerPrefs.GetInt("FPS") == 0)
            {
                fpsObject.SetActive(false);
            }

            _time += Time.deltaTime;
           
            _frameCount++;

            if (_time >= PollingTime)
            {
                var frameRate = Mathf.RoundToInt(_frameCount / _time);
                fpsText.text = $"{frameRate} FPS";

                _time -= PollingTime;
                _frameCount = 0;
            }
        }
    }
}
