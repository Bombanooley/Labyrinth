using UnityEngine;


namespace Labyrinth
{
    public sealed class CameraController : MonoBehaviour, IExecute
    {
        private Vector3 _offset;
        private Quaternion _startRotation;
        private Transform _player;
        private Transform _mainCamera;
        public Vector3 Offset 
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }
        
        public CameraController (Transform player, Transform mainCamera, Vector3 offset)
        {
            _player = player;
            _mainCamera = mainCamera;
            _startRotation = mainCamera.rotation;
            _offset = offset;
        }

        public void Execute()
        {
            _mainCamera.position = _player.position + _offset;
        }

        public void Shake()
        {
            _mainCamera.rotation = Quaternion.Euler(
                _mainCamera.rotation.eulerAngles.x,
                _mainCamera.rotation.eulerAngles.y,
                Mathf.PingPong(Time.time * 1000, 10f) - 5f);
        }

        public void StopShaking()
        {
            _mainCamera.rotation = _startRotation;
        }
    }

}
