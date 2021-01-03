using UnityEngine;


namespace Labyrinth
{

    public abstract class Character : MonoBehaviour
    {
        public float _speed = 5f;

        public float speed
        {
            get => _speed;
            set
            {
                if (value > 0) _speed = value;
            }
        }

        public abstract void Move(Vector3 vector);

        public abstract void Destroy();
    }
   
}
