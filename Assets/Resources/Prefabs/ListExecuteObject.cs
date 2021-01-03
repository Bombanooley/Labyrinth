using System;
using System.Collections;
using UnityEngine;


namespace Labyrinth
{
    public sealed class ListExecuteObject : IEnumerator, IEnumerable
    {
        private InteractiveObject[] _interactiveObjects;
        private int _index = -1;

        public ListExecuteObject()
        {
            _interactiveObjects = UnityEngine.Object.FindObjectsOfType<InteractiveObject>();
        }

        public bool MoveNext()
        {
            if (_index == _interactiveObjects.Length - 1)
            {
                Reset();
                return false;
            }
            _index++;
            return true;
        }

        public void Reset() => _index = -1;

        public object Current => _interactiveObjects[_index];

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public InteractiveObject this[int index]
        {
            get => _interactiveObjects[index];
            set => _interactiveObjects[index] = value;
        }
        public int Count => _interactiveObjects.Length;
    }
}
