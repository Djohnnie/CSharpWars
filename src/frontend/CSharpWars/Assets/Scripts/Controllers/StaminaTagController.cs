using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class StaminaTagController : TagController
    {
        #region <| Construction |>

        public StaminaTagController() : base(1.8f) { }

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
            slider.maxValue = bot.MaximumStamina;
            slider.value = bot.CurrentStamina;
        }

        #endregion
    }
}