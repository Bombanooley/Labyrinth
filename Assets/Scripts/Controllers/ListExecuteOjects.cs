using System;
using System.Collections;
using Object = UnityEngine.Object;


namespace Labyrinth
{
    public sealed class ListExecuteObjects : IEnumerator, IEnumerable
    {
        private IExecute[] _interactiveObjects;
        private int _index = -1;
        private InteractiveObject _current;

        public ListExecuteObjects()
        {
            var interactiveObjects = Object.FindObjectsOfType<InteractiveObject>();
            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                if (interactiveObjects[i] is IExecute interactiveObject)
                    AddExecuteObject(interactiveObject);
            }
        }

        public void AddExecuteObject(IExecute execute)
        {
            if (_interactiveObjects == null)
            {
                _interactiveObjects = new[] { execute };
                return;
            }
            Array.Resize(ref _interactiveObjects, Length + 1);
            _interactiveObjects[Length - 1] = execute;
        }

        public void Reload()
        {
            Array.Clear(_interactiveObjects, 0, _interactiveObjects.Length);
            Array.Resize(ref _interactiveObjects, 0);
            var interactiveObjects = Object.FindObjectsOfType<InteractiveObject>();
            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                if (interactiveObjects[i] is IExecute interactiveObject)
                    AddExecuteObject(interactiveObject);
            }
        }

        public IExecute this [int index]
        {
            get => _interactiveObjects[index];
            private set => _interactiveObjects[index] = value;
        }

        public bool MoveNext()
        {
            if (_index == _interactiveObjects.Length -1)
            {
                Reset();
                return false;
            }
            _index++;
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public int Length => _interactiveObjects.Length;

        public void Reset() => _index = -1;

        public object Current => _interactiveObjects[_index];


    }
}
