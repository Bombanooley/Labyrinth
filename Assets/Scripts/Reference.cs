using UnityEngine;


namespace Labyrinth
{
    public class Reference
    {
        private PlayerBall _playerBall;
        private Camera _mainCamera;
        private Canvas _canvas;

        public PlayerBall PlayerBall
        {
            get
            {
                if (_playerBall == null)
                {
                    var gameObject = Resources.Load<PlayerBall>("Prefabs/Player");
                    _playerBall = Object.Instantiate(gameObject);
                }
                else
                    _playerBall = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBall>();
                return _playerBall;
            }
        }

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
        }

        public Canvas Canvas
        {
            get
            {
                if (_canvas == null) _canvas = Object.FindObjectOfType<Canvas>();
                return _canvas;
            }
        }


    }
}
