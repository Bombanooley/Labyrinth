using UnityEngine;
using UnityEngine.UI;

namespace Labyrinth
{
    public sealed class RadarObject : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private void OnValidate()
        {
            _icon = Resources.Load<Image>("Textures/RadarObject");
        }

        private void OnDisable()
        {
            Radar.RemoveRadarObject(gameObject);
        }

        private void OnEnable()
        {
            //_icon = Resources.Load<Image>("Textures/RadarObject");
            //Radar.RegisterRadarObject(gameObject, _icon);
        }

    }
}
