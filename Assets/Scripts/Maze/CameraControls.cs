using System;
using UnityEngine;

namespace Maze
{
    public class CameraControls : MonoBehaviour
    {
        private const float PanBorderThickness = 10f;
        private const float ScrollSpeed = 20f;

        private float _maxY;
        private float _minY;
        private float _xLimit;
        private float _zLimit;
        private float _panSpeed;
        private Vector3 _initialPosition;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;

            if (_mainCamera == null)
            {
                throw new Exception("Camera not found");
            }

            var avgWidthAndHeight = (PlayerPrefs.GetInt("Width") + PlayerPrefs.GetInt("Height")) / 2;
            _maxY = avgWidthAndHeight;
            _minY = _maxY / 10;
            _xLimit = _maxY / 2;
            _zLimit = _maxY / 2;
            _initialPosition = new Vector3(0, _maxY + 10, 0);
            
            var camTransform = _mainCamera.transform;

            camTransform.position = _initialPosition;
            camTransform.eulerAngles = new Vector3(90, 0, 0);
        }
        
        private void Update()
        {
            var pos = transform.position;
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            _panSpeed = 20f;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                _panSpeed = 40f;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - PanBorderThickness)
            {
                pos.z += _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= PanBorderThickness)
            {
                pos.z -= _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - PanBorderThickness)
            {
                pos.x += _panSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= PanBorderThickness)
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

            if (Input.GetKey(KeyCode.R))
            {
                pos = _initialPosition;
            }

            pos.y -= scroll * ScrollSpeed * Time.deltaTime * 100f;

            pos.x = Mathf.Clamp(pos.x, -_xLimit, _xLimit);
            pos.z = Mathf.Clamp(pos.z, -_zLimit, _zLimit);
            pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
            
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 100f);
        }
    }
}
