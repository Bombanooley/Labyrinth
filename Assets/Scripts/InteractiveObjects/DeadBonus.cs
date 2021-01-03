using UnityEngine;
using static UnityEngine.Random;



namespace Labyrinth
{
    public sealed class DeadBonus : InteractiveObject, IFlay, IRotation
    {
        private float _lengthFlay;
        private float _speedRotation;

        public delegate void delGameOver();
        public event delGameOver GameOver;


        private void Awake()
        {
            _lengthFlay = Range(2f, 4.5f);
            _speedRotation = Range(10.0f, 50.0f);
        }

        public override void Execute()
        {
            if (!IsInteractable) { return; }
            Flay();
            Rotation();
        }

        protected override void Interaction()
        {
            GameOver?.Invoke();
        }

        public void Flay()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.PingPong(Time.time, _lengthFlay) + 0.4f,
                transform.localPosition.z);
        }

        public void Rotation()
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * _speedRotation), Space.World);
        }

    }
}
