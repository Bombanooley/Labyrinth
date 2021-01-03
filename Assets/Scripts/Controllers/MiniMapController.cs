using UnityEngine;


namespace Labyrinth
{

    public sealed class MiniMapController : MonoBehaviour, IExecute
    {
        private Transform _player;
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.parent = null;
            transform.rotation = Quaternion.Euler(90.0f, 0f, 0f);
            transform.position = _player.position + new Vector3(0f, 5.0f, 0f);

            var rt = Resources.Load<RenderTexture>("Textures/MiniMapTexture");

            GetComponent<Camera>().targetTexture = rt;
        }

        public void Execute()
        {
            var newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        public void LoadPlayer(Transform player) => _player = player;
    }
}