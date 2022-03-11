using UnityEngine;
using TMPro;

namespace MainMenu
{
    public class FPSDisplay : MonoBehaviour
    {
        public TextMeshProUGUI fpsText;

        private float _pollingTime = 1f;
        private float _time;
        private int _frameCount;
        void Update()
        {
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
