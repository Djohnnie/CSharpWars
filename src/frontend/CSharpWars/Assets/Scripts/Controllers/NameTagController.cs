using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class NameTagController : TagController
    {
        public NameTagController() : base(2.0f) { }

        public override void UpdateTag(Bot bot)
        {
            var txtMesh = gameObject.GetComponent<TextMesh>();
            txtMesh.text = $"{bot.Name} ({bot.PlayerName})";
        }
    }
}