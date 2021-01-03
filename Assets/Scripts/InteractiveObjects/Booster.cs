using UnityEngine;
using static UnityEngine.Random;


namespace Labyrinth
{
    public class Booster : InteractiveObject, IFlay
    {
        private PlayerBall _player;
        private float _lengthFlay;

        public delegate void delShaker();
        private delShaker _shaker;
        public event delShaker Shake;

        private void Awake()
        {
            _lengthFlay = Range(0.5f, 1.0f);
        }

        public override void Execute()
        {
            if (!IsInteractable) { return; }
            Flay();
        }

        protected override void Interaction()
        {
            Shake?.Invoke();
        }

        public void Add(delShaker d)
        { _shaker += d; }
        public void Remove (delShaker d)
        { _shaker -= d; }    

        public void Flay()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.PingPong(Time.time, _lengthFlay) + 0.2f,
                transform.localPosition.z);
        }
    }
}