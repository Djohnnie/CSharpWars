using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class NameTagController : TagController
    {
        #region <| Construction |>

        public NameTagController() : base(2.0f) { }

        #endregion

        #region <| Start |>

        public async override Task Start()
        {
            await base.Start();
        }

        #endregion

        #region <| Helper Methods |>

        protected override void UpdateTag(Bot bot)
        {
            var txtMesh = gameObject.GetComponent<TextMesh>();
            txtMesh.text = $"{bot.Name} ({bot.PlayerName})";
        }

        #endregion
    }
}