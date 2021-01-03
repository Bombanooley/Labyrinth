using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Labyrinth
{ 
    public sealed class SaveDataRepository
    {
        private readonly IData<SavedData> _data;
        private readonly IData<Seed> _seedData;
        private readonly IData<List<SavedData>> _dataList;

        private const string _folderName = "dataSave";
        private const string _fileName = "data.txt";
        private const string _seedFileName = "seed.txt";
        private const string _playerName = "Player";
        private const string _goodbonusName = "GoodBonus";
        private const string _badBonusName = "BadBonus";
        private const string _deadBonusName = "DeadBonus";
        private const string _boosterName = "Booster";
        private readonly string _path;

        public SaveDataRepository()
        {
            _seedData = new JsonData<Seed>();
            _data = new JsonData<IMemento>();
            _dataList = new JsonData<List<SavedData>>();

            _path = Path.Combine(Application.dataPath, _folderName);
        }

        //public void Save(Character player)
        //{
        //    if (!Directory.Exists(Path.Combine(_path)))
        //        Directory.CreateDirectory(_path);
        //    var savePlayer = new SavedData
        //    {
        //        Position = player.transform.position,
        //        Name = "Name",
        //        IsEnabled = true
        //    };
        //    //_data.Save(savePlayer, Path.Combine(_path, _fileName));
        //}

        public void Save (List<IMemento> list)
        {
            if (!Directory.Exists(Path.Combine(_path)))
                Directory.CreateDirectory(_path);

            List<SavedData> save = new List<SavedData>();

            foreach (var item in list)
            {
                save.Add(new SavedData
                {
                    Position = item.transform.position,
                    Name = item.name,
                    IsEnabled = true
                });
            }
            _dataList.Save(save, Path.Combine(_path, _fileName));
        }
        public void Save(List<GameObject> saveObjects)
        {
            if (!Directory.Exists(Path.Combine(_path)))
                Directory.CreateDirectory(_path);

            List<SavedData> save = new List<SavedData>();

            foreach (var item in saveObjects)
            {
                save.Add(new SavedData
                {
                    Position = item.transform.position,
                    Name = item.name,
                    IsEnabled = true
                });
            }
                _dataList.Save(save, Path.Combine(_path, _fileName));
        }
        public void SaveSeed(Seed seed)
        {
            var file = Path.Combine(_path, _seedFileName);
            _seedData.Save(seed, Path.Combine(_path, _seedFileName));
        }

        //public void Load (Character player)
        //{
        //    var file = Path.Combine(_path, _fileName);
        //    if (!File.Exists(file)) return;
        //    var newPlayer = _data.Load(file);
        //    player.transform.position = newPlayer.Position;
        //    player.name = newPlayer.Name;
        //    player.gameObject.SetActive(newPlayer.IsEnabled);

        //    Debug.Log(newPlayer);

        //}

        public void Load(ref List<GameObject> loadObjects)
        {
            var file = Path.Combine(_path, _fileName);
            if (!File.Exists(file)) return;
            var newObjects = _dataList.Load(file);
            if (newObjects != null) loadObjects.Clear();

            for (int i = 0; i < newObjects.Count; i++)
            {
                loadObjects.Add(new GameObject());
                loadObjects[i].transform.position = newObjects[i].Position;
                loadObjects[i].name = newObjects[i].Name;
                loadObjects[i].gameObject.SetActive(newObjects[i].IsEnabled);

            }
        }

        public Seed LoadSeed() 
        {
            var file = Path.Combine(_path, _seedFileName);
            if (!File.Exists(file)) return default;
            return _seedData.Load(file);
        }
    }
}
