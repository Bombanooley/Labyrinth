using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Labyrinth
{
    public class JsonData<T> : IData<T> where T : class, new()
    //public class JsonData : IData

    {
        public void Save(List<SavedData> data, string path = null)
        {
            List<string> list = new List<string>();
            foreach (var item in data)
            {
                var str = JsonUtility.ToJson(item);
                list.Add(str);
            }
            File.WriteAllLines(path, list);
        }

        //public List<SavedData> Load(string path = null)
        //{
        //    List<SavedData> list = new List<SavedData>();
        //    string[] readedList = File.ReadAllLines(path);
        //    foreach (var item in readedList)
        //    {
        //        list.Add(JsonUtility.FromJson<SavedData>(item));
        //    }
        //    return list;
        //}

        public void Save(T data, string path = null)
        {
            List<string> list = new List<string>();
            if (data is List<SavedData>)
            {
                foreach (var item in data as List<SavedData>)
                {
                    list.Add(JsonUtility.ToJson(item));
                }
            }
            File.WriteAllLines(path, list);
            //File.WriteAllText(path, Crypto.CryptoXOR(str));
        }
        public T Load(string path = null)
        {
            string[] str = File.ReadAllLines(path);
            List<string> list = new List<string>();
            foreach (var item in str)
                list.Add(item);

            T savedData = new T();

            List<SavedData> listSavedData = new List<SavedData>();
            if (listSavedData is List<SavedData>)
            {
                foreach (var item in list)
                {
                    listSavedData.Add(JsonUtility.FromJson<SavedData>(item));
                }
                return listSavedData as T;
            }
            if (savedData is Seed)
            {
                savedData = JsonUtility.FromJson<T>(str[0]);
            }
            return default;
            //return JsonUtility.FromJson<T>(Crypto.CryptoXOR(str));
        }

        //public void Save(List<SavedData> data, string path = null)
        //{
        //    List<string> list = new List<string>();
        //    foreach (var item in data)
        //        list.Add(JsonUtility.ToJson(item));

        //    File.WriteAllLines(path, list);
        //}

        //public List<SavedData> Load(string path)
        //{
        //    string[] list = File.ReadAllLines(path);
        //    List<SavedData> listSavedData = new List<SavedData>();
        //    foreach (var item in list)
        //    {
        //        listSavedData.Add(JsonUtility.FromJson<SavedData>(item));
        //    }
        //    return listSavedData;
        //}

    }
}


