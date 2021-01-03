using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Random;
using Random = UnityEngine.Random;

namespace Labyrinth
{
    public sealed class GameController : MonoBehaviour, IDisposable, ISpawner
    {
        [SerializeField] private CanvasGroup winBackgroundImageCanvasGroup;
        [SerializeField] private CanvasGroup looseBackgroundImageCanvasGroup;
        [SerializeField] private Canvas pauseUI;
        [SerializeField] private float _goodBonusRate;
        [SerializeField] private float _badBonusRate;
        [SerializeField] private float _deadBonusRate;
        [SerializeField] private float _boosterRate;
        public bool isNeedToGenerate;

        private const string _playerName = "Player";
        private const string _goodbonusName = "GoodBonus";
        private const string _badBonusName = "BadBonus";
        private const string _deadBonusName = "DeadBonus";
        private const string _boosterName = "Booster";

        private PlayerBall _player;
        private PlayerBall _playerPrefab;
        private GoodBonus _goodBonusPrefab;
        private BadBonus _badBonusPrefab;
        private DeadBonus _deadBonusPrefab;
        private Booster _boosterPrefab;

        private List<Vector3> _IOPositions;
        private List<GameObject> _saveObjects;
        private ListExecuteObjects _executeObjects;
        private GameEnding _gameEnding;
        private DisplayScore _displayScore;
        private InputController _inputController;
        private CameraController _mainCamera;
        private MiniMapController _minimap;
        private LabyrinthGenerator _labyrinthGenerator;
        private SaveDataRepository _saveDataRepository;

        private int _rows;
        private int _columns;
        private int _score;
        private int _desireScore;
        private int _seed;
        private float _flickCooldown;
        private bool _isShaking;
        private bool _isWin;
        private bool _isLoose;





        public int Score
        {
            get => _score;
            set { _score = value; }
        }
        public int DesireScore
        {
            get => _desireScore;
            set { _desireScore = value; }
        }

        private void Awake()
        {
            var date = DateTime.Now;
            _seed = date.Second + date.Minute * 100 + date.Hour * 10000 + date.Day * 1000000;

            InitState(_seed);

            Reference reference = new Reference();
            _saveDataRepository = new SaveDataRepository();
            _executeObjects = new ListExecuteObjects();
            _IOPositions = new List<Vector3>();
            _saveObjects = new List<GameObject>();
            SpawnPlayer();

            _mainCamera = new CameraController(
                    _player.transform,
                    reference.MainCamera.transform,
                    new Vector3(0f, 6.84f, -4.38f));
            _executeObjects.AddExecuteObject(_mainCamera);

            _minimap = FindObjectOfType<MiniMapController>();
            _executeObjects.AddExecuteObject(_minimap);


            _labyrinthGenerator = FindObjectOfType<LabyrinthGenerator>();
            isNeedToGenerate = _labyrinthGenerator.isNeedToGenerate;

            _gameEnding = GetComponent<GameEnding>();
            //Instantiate(new GameEnding(), transform);

            _displayScore = new DisplayScore(GameObject.Find("TextScore").GetComponent<Text>());

            if (isNeedToGenerate)
            {
                _labyrinthGenerator.Generate();
                _rows = _labyrinthGenerator.Rows;
                _columns = _labyrinthGenerator.Columns;

                SpawnBonuses();
            }

            ExecuteObjectsAddDelegates();

            _inputController = new InputController(_player, reference.MainCamera.transform, ref _saveObjects, pauseUI, ref _seed);
            _executeObjects.AddExecuteObject(_inputController);

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
            }
        }


        private void Update()
        {
            if (_inputController.isSaved)
            {
                _inputController.isSaved = false;
                SaveGame();
            }
            if(_inputController.isSeedChanged)
            {
                _inputController.isSeedChanged = false;
                _seed = _inputController.seedInt;
                Reload(_seed);
            }

            for (int i = 0; i < _executeObjects.Length; i++)
            {
                var interactiveObject = _executeObjects[i];
                if (interactiveObject != null)
                {
                    interactiveObject?.Execute();
                }
            }

            if (_isShaking) _mainCamera.Shake();

            WinLooseCheck();
        }

        private void SaveGame()
        {
            List<IMemento> list = new List<IMemento>();
            list.Add(Save());
            list.Add(_player.Save());
            foreach (var item in _executeObjects)
            {
                if (item is IMemento)
                    list.Add(item as IMemento);
            }
            _saveDataRepository.Save(list);
        }

        private void Reload(int seed)
        {
            Reference reference = new Reference();

            _seed = seed;
            InitState(_seed);
            ClearLevel();
            SpawnPlayer();

            _executeObjects.AddExecuteObject(_mainCamera);
            _executeObjects.AddExecuteObject(_minimap);

            _labyrinthGenerator.Generate();
            _rows = _labyrinthGenerator.Rows;
            _columns = _labyrinthGenerator.Columns;

            SpawnBonuses();

            ExecuteObjectsAddDelegates();

            _inputController = new InputController(_player, reference.MainCamera.transform, ref _saveObjects, pauseUI, ref _seed);
            _executeObjects.AddExecuteObject(_inputController);

        }

        private void ClearLevel()
        {
            ClearList(ref _saveObjects);
            ClearList(ref _IOPositions);
            _executeObjects.Reload();
        }

        private void ClearList<T> (ref List<T> list)
        {
            if (list.Count == 0)
                return;
            foreach (var item in list)
            {
                if (item is GameObject)
                    Destroy(item as GameObject);
            }
            list.Clear();
        }

        private void ExecuteObjectsAddDelegates()
        {
            foreach (var item in _executeObjects)
            {
                if (item is BadBonus badBonus)
                    badBonus.Slow += _player.Slow;
                if (item is DeadBonus deadBonus)
                    deadBonus.GameOver += Loose;
                if (item is GoodBonus goodBonus)
                    goodBonus.OnPointChange += DisplayScore;
                if (item is Booster booster)
                {
                    booster.Shake += ShakeCamera;
                    booster.Shake += _player.GetComponent<PlayerBall>().Boost;
                }
            }
        }

        private void FlickCooldown()
        {
            _flickCooldown += Time.deltaTime;
        }

        public void SpawnPlayer()
        {
            _player?.Destroy();
            _playerPrefab = Resources.Load<PlayerBall>("Prefabs/Player");
            _player = Instantiate(_playerPrefab, new Vector3(0f, 0.05f, 0f), Quaternion.identity);
            _player.name = "Player";
            _saveObjects.Add(_player.gameObject);
        }

        public void SpawnBonuses()
        {
            var mult = _rows * _columns;

            var goodBonusCount = mult * _goodBonusRate;
            var badBonusCount = mult * _badBonusRate;
            var deadBonusCount = mult * _deadBonusRate;
            var boosterCount = mult * _boosterRate;
            _goodBonusPrefab = Resources.Load<GoodBonus>("Prefabs/GoodBonus");
            _badBonusPrefab = Resources.Load<BadBonus>("Prefabs/BadBonus");
            _deadBonusPrefab = Resources.Load<DeadBonus>("Prefabs/DeadBonus");
            _boosterPrefab = Resources.Load<Booster>("Prefabs/Booster");


            for (int i = 0; i < goodBonusCount; i++)
                CreateBonus(_goodBonusPrefab, "GoodBonus");
            for (int i = 0; i < badBonusCount; i++)
                CreateBonus(_badBonusPrefab, "BadBonus");
            for (int i = 0; i < deadBonusCount; i++)
                CreateBonus(_deadBonusPrefab, "DeadBonus");
            for (int i = 0; i < boosterCount; i++)
                CreateBonus(_boosterPrefab, "Booster");
            _desireScore = (int)goodBonusCount;
        }

        private void CreateBonus (InteractiveObject obj, string name)
        {
            bool _isPlaced = false;
            do
            {
                Vector3 pos = new Vector3
                            (
                                Range(0, _rows / 2) * 4,
                                0.05f,
                                Range(0, _columns / 2) * 4
                            );
                if (_IOPositions.Contains(pos) || (pos.x == 0.0f && pos.z == 0.0f)) continue;
                else _isPlaced = true;
                var temp = Instantiate(obj, pos, Quaternion.identity);
                temp.name = name;
                _executeObjects.AddExecuteObject(temp.GetComponent<InteractiveObject>());
                _IOPositions.Add(temp.GetComponent<Transform>().position);
                _saveObjects.Add(temp.gameObject);
            }
            while (!_isPlaced);
        }

        private void LoadPlayer(Transform transform)
        {
            Reference reference = new Reference();
            _player = Instantiate(_playerPrefab, transform.position, _playerPrefab.transform.rotation);
            _player.name = "Player";
            _player.GetComponent<Rigidbody>().useGravity = true;
            var temp = new InputController(_player, reference.MainCamera.transform, ref _saveObjects, pauseUI, ref _seed);
            _inputController = temp;
            _minimap.LoadPlayer(_player.transform);
        }
        private void LoadGoodBonus(Transform transform)
        {
            var temp = Instantiate(_goodBonusPrefab, transform.position, _goodBonusPrefab.transform.rotation);
            _saveObjects.Add(temp.gameObject);
        }
        private void LoadBadBonus(Transform transform)
        {
            var temp = Instantiate(_badBonusPrefab, transform.position, _badBonusPrefab.transform.rotation);
            _saveObjects.Add(temp.gameObject);
        }
        private void LoadDeadBonus(Transform transform)
        {
            var temp = Instantiate(_deadBonusPrefab, transform.position, _deadBonusPrefab.transform.rotation);
            _saveObjects.Add(temp.gameObject);
        }
        private void LoadBooster(Transform transform)
        {
            var temp = Instantiate(_boosterPrefab, transform.position, _boosterPrefab.transform.rotation);
            _saveObjects.Add(temp.gameObject);
        }

        private void LoadSpawnObjects()
        {
            for (int i = 0; i < _saveObjects.Count; i++)
            {
                switch (_saveObjects[i].name)
                {
                    case _playerName:
                        LoadPlayer(_saveObjects[i].transform);
                        break;
                    case _goodbonusName:
                        LoadGoodBonus(_saveObjects[i].transform);
                        break;
                    case _badBonusName:
                        LoadBadBonus(_saveObjects[i].transform);
                        break;
                    case _deadBonusName:
                        LoadDeadBonus(_saveObjects[i].transform);
                        break;
                    case _boosterName:
                        LoadBooster(_saveObjects[i].transform);
                        break;
                    default:
                        break;
                }
            }
            _executeObjects.Reload();
        }

        public MementoGameController Save()
        {
            return new MementoGameController(_seed, _rows, _columns, Score, _goodBonusRate,
                _badBonusRate, _deadBonusRate, _boosterRate);
        }

        private void Restart() => _gameEnding.RestartButton();

        private void ExitGame() => _gameEnding.ExitGame();

        private void DisplayScore(int point)
        {
            Score += point;
            _displayScore.Display(Score, DesireScore);
            if (Score >= DesireScore) _isWin = true; 
        }

        private void Loose() => _isLoose = true;

        private void WinLooseCheck()
        {
            if (_isWin)
            {
                _gameEnding.EndLevel(winBackgroundImageCanvasGroup, false);
            }
            else if (_isLoose)
            {
                _gameEnding.EndLevel(looseBackgroundImageCanvasGroup, true);
            }
        }

        public void Dispose()
        {
            foreach (var item in _saveObjects)
            {
                Destroy(item);
            }
        }

        private void ShakeCamera()
        {
            _isShaking = true;
            Invoke(nameof(StopShaking), 0.5f);
        }

        private void StopShaking()
        {
            _isShaking = false;
            _mainCamera.StopShaking();
        }

    }
}

