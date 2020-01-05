using Assets.Scripts.Model;
using Assets.Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ArenaController : MonoBehaviour
    {
        private Arena _arena;
        private GameObject _floor;

        [Header("The height of the arena platform.")]
        public float PlatformHeight = .2f;

        void Start()
        {
            _arena = ApiClient.GetArena();
            _floor = GameObject.Find("Floor");
            _floor.transform.localScale = new Vector3(_arena.Width, PlatformHeight, _arena.Height);
            _floor.GetComponent<Renderer>().material.mainTextureScale = new Vector2(_arena.Width, _arena.Height);
        }

        public Vector3 ArenaToWorldPosition(int x, int y)
        {
            if (_arena != null && _floor != null)
            {
                return new Vector3(
                    .5f + x - _arena.Width / 2 + _floor.transform.position.x,
                    PlatformHeight / 2,
                    _arena.Height - .5f - y - _arena.Height / 2 + _floor.transform.position.z);
            }

            return Vector3.zero;
        }
    }
}