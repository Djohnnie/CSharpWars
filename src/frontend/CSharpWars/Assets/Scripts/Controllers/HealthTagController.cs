using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class HealthTagController : TagController
    {
        #region <| Construction |>

        public HealthTagController() : base(1.725f) { }

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
            var slider = gameObject.GetComponentInChildren<Slider>();
            slider.minValue = 0;
            slider.maxValue = bot.MaximumHealth;
            slider.value = bot.CurrentHealth;
        }

        #endregion
    }
}