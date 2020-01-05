using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class HealthTagController : TagController
    {
        public HealthTagController() : base(1.725f) { }

        public override void UpdateTag(Bot bot)
        {
            var slider = gameObject.GetComponentInChildren<Slider>();
            slider.minValue = 0;
            slider.maxValue = bot.MaximumHealth;
            slider.value = bot.CurrentHealth;
        }
    }
}