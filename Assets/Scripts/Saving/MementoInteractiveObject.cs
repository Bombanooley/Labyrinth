using UnityEngine;


namespace Labyrinth
{
    public class MementoInteractiveObject : IMemento
    {
        public Vector3 position { get; private set; }
        public Quaternion rotation { get; private set; }
        public bool isInteractable { get; private set; }

        public MementoInteractiveObject(Vector3 position, Quaternion rotation, bool isInteractable)
        {
            this.position = position;
            this.rotation = rotation;
            this.isInteractable = isInteractable;
        }
    }
}
