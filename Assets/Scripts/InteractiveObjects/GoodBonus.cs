using System;
using UnityEngine;
using static UnityEngine.Random;


namespace Labyrinth
{
    public sealed class GoodBonus : InteractiveObject, IFlay, IFlicker, IEquatable<GoodBonus>
    {
        private Material _material;
        private float _lengthFlay;
        private RandColor _randColor;

        public int point;

        public delegate void delWin(object o);
        public event delWin Win;
        public event Action<int> OnPointChange = delegate (int i) { };

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _lengthFlay = Range(0.5f, 1.0f);
            var rand = new System.Random();
            var values = Enum.GetValues(typeof(RandColor));
            _randColor = (RandColor)values.GetValue(rand.Next(values.Length));
            _material.color = ColorHSV();
        }

        public override void Execute()
        {
            if (!IsInteractable) { return; }
            Flay();
            Flicker();
        }

        protected override void Interaction()
        {
            OnPointChange?.Invoke(point);
        }

        public void Flay()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x, 
                Mathf.PingPong(Time.time, _lengthFlay) + 0.2f, 
                transform.localPosition.z);
        }

        public void Flicker()
        {
            switch (_randColor)
            {
                case RandColor.r:
                    _material.color = new Color(
                     Mathf.PingPong(Time.time, 1.0f),
                     _material.color.g,
                     _material.color.b);
                    break;

                case RandColor.g:
                    _material.color = new Color(
                     _material.color.r,
                     Mathf.PingPong(Time.time, 1.0f),
                     _material.color.b);
                    break;

                case RandColor.b:
                    _material.color = new Color(
                     _material.color.r,
                     _material.color.g,
                     Mathf.PingPong(Time.time, 1.0f));
                    break;

                default:
                    break;
            }

        }

        public bool Equals (GoodBonus other)
        {
            return point == other.point;
        }


    }
}
