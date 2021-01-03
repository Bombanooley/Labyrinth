using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Labyrinth
{
    public sealed class Radar : MonoBehaviour, IExecute
    {
        private Transform _player; 
        private readonly float _mapScale = 2;
        public static List<RadarObject> RadObjects = new List<RadarObject>();
        public sealed class RadarObject
        {
            public Image Icon;
            public GameObject Owner;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public static void RegisterRadarObject(GameObject obj, Image img)
        {
            Image image = Instantiate(img);
            RadObjects.Add(new RadarObject { Owner = obj, Icon = image });
        }

        public static void RemoveRadarObject(GameObject obj)
        {
            List<RadarObject> newList = new List<RadarObject>();
            foreach (RadarObject item in RadObjects)
            {
                if (item.Owner == obj)
                {
                    Destroy(item.Icon);
                    continue;
                }
                newList.Add(item);
            }
            RadObjects.Clear();
            RadObjects.AddRange(newList);
        }

        private void DrawRadarDots()
        {
            foreach (RadarObject item in RadObjects)
            {
                Vector3 radarPos = (item.Owner.transform.position - _player.position);
                float distToObject = Vector3.Distance(_player.position,
                                            item.Owner.transform.position) * _mapScale;
                float deltaY = Mathf.Atan2(radarPos.x, radarPos.y) * Mathf.Rad2Deg -
                                270 - _player.eulerAngles.y;
                radarPos.x = distToObject * Mathf.Cos(deltaY * Mathf.Deg2Rad) * -1;
                radarPos.z = distToObject * Mathf.Sin(deltaY * Mathf.Deg2Rad);
                item.Icon.transform.SetParent(transform);
                item.Icon.transform.position = new Vector3(radarPos.x,
                                                radarPos.z, 0) + transform.position;
            }
        }

        public void Execute()
        {
            if (Time.frameCount % 2 == 0)
            {
                DrawRadarDots();
            }
        }



    }

}
