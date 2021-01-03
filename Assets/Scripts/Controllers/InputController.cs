using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Labyrinth
{
    public class InputController : MonoBehaviour, IExecute
    {
        private Character _player;
        private KeyCode _savePlayer = KeyCode.F5;
        private KeyCode _loadPlayer = KeyCode.F8;
        private KeyCode _pause = KeyCode.Escape;
        private Canvas _pauseCanvas;

        private List<GameObject> _saveObjects;
        private SaveDataRepository _saveDataRepository;
        private Vector3 _cameraForward;
        private Vector3 _direction;
        private Transform _camera;
        private bool _isPaused = false;

        public Seed seed;
        public int seedInt;
        public bool isSeedChanged;
        public bool isSaved;



        public InputController(Character player, Transform camera, ref List<GameObject> saveObjects, Canvas pauseUI, ref int seed)
        {
            _player = player;
            _camera = camera;
            _saveDataRepository = new SaveDataRepository();
            _saveObjects = saveObjects;
            _pauseCanvas = pauseUI;
            this.seed = new Seed(seed);
            isSaved = false;
        }

        public void Execute()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            _cameraForward = Vector3.Scale(_camera.forward, new Vector3(1, 0, 1)).normalized;
            _direction = moveZ * _cameraForward + moveX * _camera.right;

            _player.Move(_direction);

            if (Input.GetKeyDown(_savePlayer))
            {
                isSaved = true;
                //_saveDataRepository.SaveSeed(seed);
                //_saveDataRepository.Save(_saveObjects);
                //_saveDataRepository.Save(_player);
            }
            if (Input.GetKeyDown(_loadPlayer))
            {
                seed = _saveDataRepository.LoadSeed();
                _saveDataRepository.Load(ref _saveObjects);
                //_saveDataRepository.Load(_player);
                seedInt = seed.seed;
                isSeedChanged = true;

            }
            if (Input.GetKeyDown(_pause))
            {
                if (!_isPaused)
                {
                    Time.timeScale = 0;
                    _pauseCanvas.gameObject.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    _pauseCanvas.gameObject.SetActive(false);
                }
                _isPaused = !_isPaused;
            }
        }


    }
}
