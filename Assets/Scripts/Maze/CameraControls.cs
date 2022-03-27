using UnityEngine;

namespace Maze
{
    public class CameraControls : MonoBehaviour
    {
        private float _panSpeed;
        private float _panBorderThickness = 10f;
        private float _xLimit = 40f;
        private float _yLimit = 40f;
        private float _scrollSpeed = 20f;
        private float _minY = 20f;
        private float _maxY = 70f;
        private Vector3 _initialPosition;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;

            if (PlayerPrefs.GetInt("UserSolves") == 1)
            {
                _initialPosition = new Vector3(0, 25, 0);
            }
            else
            {
                _initialPosition = new Vector3(0, 100, 0);
            }
            
            _mainCamera.transform.position = _initialPosition;
            _mainCamera.transform.eulerAngles = new Vector3(90, 0, 0);
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

            if (Input.GetKey(KeyCode.R))
            {
                pos = _initialPosition;
            }

            pos.y -= scroll * _scrollSpeed * Time.deltaTime * 100f;

            pos.x = Mathf.Clamp(pos.x, -_xLimit, _xLimit);
            pos.z = Mathf.Clamp(pos.z, -_yLimit, _yLimit);
            pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
            
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 100f);
        }
    }
}
