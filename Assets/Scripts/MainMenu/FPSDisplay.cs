using UnityEngine;
using TMPro;

namespace MainMenu
{
    public class FPSDisplay : MonoBehaviour
    {
        public TMP_Text fpsText;
        public GameObject fpsObject;

        private float _pollingTime = 1f;
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

            if (_time >= _pollingTime)
            {
                var frameRate = Mathf.RoundToInt(_frameCount / _time);
                fpsText.text = $"{frameRate} FPS";

                _time -= _pollingTime;
                _frameCount = 0;
            }
        }
    }
}
