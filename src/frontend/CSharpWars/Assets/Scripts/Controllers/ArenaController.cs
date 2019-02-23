using Assets.Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ArenaController : MonoBehaviour
    {
        void Start()
        {
            var arena = ApiClient.GetArena();
            gameObject.transform.localScale = new Vector3((float)arena.Width, 1, (float)arena.Height);
            GameObject.Find("Floor").GetComponent<Renderer>().material.mainTextureScale = new Vector2((float)arena.Width, (float)arena.Height);
        }
    }
}