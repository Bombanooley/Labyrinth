using System;
using UnityEngine;
using static UnityEngine.Random;


namespace Labyrinth
{

    public abstract class InteractiveObject : MonoBehaviour, IInteractable, IComparable<InteractiveObject>, IExecute

    {
        protected Color _color;

        private bool _isInteractable;
        public bool IsInteractable 
         {
            get { return _isInteractable; }
            private set
            {
                _isInteractable = value;
                GetComponent<Renderer>().enabled = _isInteractable;
                GetComponent<Collider>().enabled = _isInteractable;
            }
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!IsInteractable || !other.CompareTag("Player"))
                return;
            Interaction();
            IsInteractable = false;
        }

        protected abstract void Interaction();

        public abstract void Execute();

        private void Start()
        {
            IsInteractable = true;
            _color = ColorHSV();
        }

        void IAction.Action()
        {
            if (TryGetComponent(out Renderer renderer))
                renderer.material.color = _color;
        }

        void IInitialization.Action()
        {
            if (TryGetComponent(out Renderer renderer))
                renderer.material.color = Color.cyan;
        }


        public int CompareTo(InteractiveObject other)
        {
            return name.CompareTo(other.name);
        }

        public MementoInteractiveObject Save()
        {
            return new MementoInteractiveObject(transform.position, transform.rotation, IsInteractable);
        }
    }
}
