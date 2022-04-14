using System;
using UnityEngine;

namespace Maze
{
    public class CameraControls : MonoBehaviour
    {
        private const float PanBorderThickness = 10f; // Sets the border for the user to move their mouse to pan the camera.
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
            // Assign main camera to variable.
            _mainCamera = Camera.main;

            if (_mainCamera == null) // If the camera is not found for whatever reason prevent the program from executing.
            {
                throw new Exception("Camera not found");
            }

            // Determine limits from width and height.
            var avgWidthAndHeight = (PlayerPrefs.GetInt("Width") + PlayerPrefs.GetInt("Height")) / 2;
            _maxY = avgWidthAndHeight;
            _minY = _maxY / 10;
            _xLimit = _maxY / 2;
            _zLimit = _maxY / 2;
            _initialPosition = new Vector3(0, _maxY, 0);
            
            // Position the camera correctly over the maze.
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
                // Speed up movement.
                _panSpeed = 40f;
            }

            /*
             * _panSpeed * Time.deltaTime increases or decreases values relative to the time between frames and the pan speed.
             * Ensures smooth movement.
             */
            
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

            if (Input.GetKey(KeyCode.R)) // Returns to initial position.
            {
                pos = _initialPosition;
            }

            // Allows zoom with the mouse wheel relative to time and scroll speed.
            pos.y -= scroll * ScrollSpeed * Time.deltaTime * 100f;
            
            // Prevents the camera from going too far in each direction.
            pos.x = Mathf.Clamp(pos.x, -_xLimit, _xLimit);
            pos.z = Mathf.Clamp(pos.z, -_zLimit, _zLimit);
            pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
            
            // Linearly interpolates to the new position.
            // Smoother movement.
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 100f);
        }
    }
}
