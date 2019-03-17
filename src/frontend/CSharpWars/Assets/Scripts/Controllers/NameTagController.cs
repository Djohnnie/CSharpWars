using Assets.Scripts.Controllers.Interfaces;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class NameTagController : TagController, IBotDependant
    {
        public NameTagController() : base(2.0f) { }

        public void UpdateBot(Bot bot)
        {
            var txtMesh = gameObject.GetComponent<TextMesh>();
            txtMesh.text = $"{bot.Name} ({bot.PlayerName})";
        }

        public void Destroy()
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}