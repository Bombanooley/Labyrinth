using UnityEngine;

namespace Labyrinth
{
    public sealed class PlayerBall : Character
    {
        private Rigidbody _rigidbody;

        private bool _isBoosted;
        private bool _isSlowed;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        public override void Move(Vector3 direction)
        {

            if (direction.sqrMagnitude > 0)
            {
                var movedirection = direction * speed;
                _rigidbody.velocity = movedirection;
            }
        }

        public override void Destroy()
        {
            Destroy(this);
        }

        public void Boost()
        {
            if (!_isBoosted)
            {
                _isBoosted = true;
                speed *= 1.5f;
                Invoke(nameof(BoostStop), 5);
            }
        }

        public void BoostStop()
        {
            speed /= 1.5f;
            _isBoosted = false;
        }
        
        public void Slow(string str)
        {
            if (!_isSlowed)
            {
                _isSlowed = true;
                speed /= 1.5f;
                Invoke(nameof(SlowStop), 5);
            }
        }

        public void SlowStop()
        {
            speed *= 1.5f;
            _isSlowed = false;
        }

        public MementoPlayer Save()
        {
            return new MementoPlayer(transform.position, transform.rotation);
        }
    }

}
