using Assets.Scripts.Controllers.Interfaces;
using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class StaminaTagController : TagController, IBotDependant
    {
        public StaminaTagController() : base(1.8f) { }

        public void UpdateBot(Bot bot)
        {
            var staminaSlider = gameObject.GetComponentInChildren<Slider>();
            staminaSlider.minValue = 0;
            staminaSlider.maxValue = bot.MaximumStamina;
            staminaSlider.value = bot.CurrentStamina;
        }

        public void Destroy()
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}