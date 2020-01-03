using Assets.Scripts.Controllers.Interfaces;
using Assets.Scripts.Model;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class HealthTagController : TagController, IBotDependent
    {
        public HealthTagController() : base(1.725f) { }

        public void UpdateBot(Bot bot)
        {
            var healthSlider = gameObject.GetComponentInChildren<Slider>();
            healthSlider.minValue = 0;
            healthSlider.maxValue = bot.MaximumHealth;
            healthSlider.value = bot.CurrentHealth;
        }

        public void Destroy()
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}