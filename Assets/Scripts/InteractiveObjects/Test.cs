using System;
using System.Linq;
using UnityEngine;


namespace Labyrinth
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            var interObj = new ListExecuteObject();
            //foreach (var item in interObj)
            //{
            //    print(item);
            //}
            //var goodBonusComparer = new GoodBonusComparer();
            ////var objects = FindObjectsOfType<GoodBonus>().ToList();
            ////objects.Sort(goodBonusComparer);
            ////foreach (var item in objects)
            ////{
            ////    print($"{item.name} - {item.point} points");
            ////}
            ////var bonus = GameObject.Find("Point 4").GetComponent<GoodBonus>();
            //var objects = FindObjectsOfType<GoodBonus>().Distinct(new GoodBonusesEqualityComparer());
            //foreach (var item in objects)
            //{
            //    print($"{item.name}");
            //}

            for (int i = 0; i < interObj.Count; i++)
            {
                print($"{interObj[i]}");
            }
        }
    }
}
