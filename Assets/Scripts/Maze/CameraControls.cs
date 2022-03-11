using UnityEngine;

namespace Maze
{
    public class CameraControls : MonoBehaviour
    {
        private float _panSpeed = 20f;
        private float _panBorderThickness = 10f;
        private float _xLimit = 20f;
        private float _yLimit = 20f;
        private float _scrollSpeed = 20f;
        private float _minY = 20f;
        private float _maxY = 50f;

        private void Update()
        {
            var pos = transform.position;
            var scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - _panBorderThickness)
            {
                pos.z += _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= _panBorderThickness)
            {
                pos.z -= _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - _panBorderThickness)
            {
                pos.x += _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= _panBorderThickness)
            {
                pos.x -= _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Equals))
            {
                pos.y -= _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Minus))
            {
                pos.y += _panSpeed * Time.deltaTime;
            }

            pos.y -= scroll * _scrollSpeed * Time.deltaTime * 100f;

            pos.x = Mathf.Clamp(pos.x, -_xLimit, _xLimit);
            pos.z = Mathf.Clamp(pos.z, -_yLimit, _yLimit);
            pos.y = Mathf.Clamp(pos.y, _minY, _maxY);

            transform.position = pos;
        }
    }
}
