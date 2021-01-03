using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



namespace Labyrinth
{
    public static class Extentions
    {
        public static bool TryBool(this string self)
        {
            return Boolean.TryParse(self, out var res) && res;
        }

        public static float TrySingle(this string self)
        {
            var b = Single.TryParse(self, out float res);
            if (b) return res;
            else return Single.NaN;
        }

        public static T DeepCopy<T> (this T self)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("Type must be IsSerializable");
            if (ReferenceEquals(self, null))
                return default;

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, self);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}