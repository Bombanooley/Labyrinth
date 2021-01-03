using System;
using UnityEngine;
using static UnityEngine.Random;


namespace Labyrinth
{
    public sealed class BadBonus : InteractiveObject, IFlay, IRotation
    {
        private PlayerBall _player;
        private float _lengthFlay;
        private float _speedRotation;

        public event Action<string> Slow = delegate (string str) { };

        private void Awake()
        {
            _lengthFlay = Range(2f, 4.5f);
            _speedRotation = Range(10.0f, 50.0f);
        }

        protected override void Interaction()
        {
            Slow?.Invoke(gameObject.name);
        }

        public override void Execute()
        {
            if (!IsInteractable) { return; }
            Flay();
            Rotation();
        }

        public void Flay()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x, 
                Mathf.PingPong(Time.time, _lengthFlay)+0.4f, 
                transform.localPosition.z);
        }

        public void Rotation()
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * _speedRotation), Space.World);
        }

    }
}
