using UnityEngine;


namespace Labyrinth
{
    public class MementoPlayer : IMemento
    {
        public Vector3 position { get; private set; }
        public Quaternion rotation { get; private set; }

        public MementoPlayer(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
