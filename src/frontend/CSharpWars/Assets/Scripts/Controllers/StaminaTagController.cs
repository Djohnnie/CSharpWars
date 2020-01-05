using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class StaminaTagController : TagController
    {
        public StaminaTagController() : base(1.8f) { }

        public override void UpdateTag(Bot bot)
        {
            var slider = gameObject.GetComponentInChildren<Slider>();
            slider.minValue = 0;
            slider.maxValue = bot.MaximumStamina;
            slider.value = bot.CurrentStamina;
        }
    }
}