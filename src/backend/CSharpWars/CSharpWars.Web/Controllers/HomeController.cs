using System.Linq;
using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Constants;
using CSharpWars.Web.Extensions;
using CSharpWars.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerLogic _playerLogic;

        public HomeController(IPlayerLogic playerLogic)
        {
            _playerLogic = playerLogic;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                return RedirectToAction(nameof(Play));
            }

            var vm = new PlayerViewModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PlayerViewModel vm)
        {
            var login = new LoginDto
            {
                Name = vm.Name,
                Secret = vm.Secret
            };
            var player = await _playerLogic.Login(login);
            if (player != null)
            {
                HttpContext.Session.SetObject("PLAYER", player);
                return RedirectToAction(nameof(Play));
            }

            return View(vm);
        }

        public IActionResult Play()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");
                var vm = new GameViewModel
                {
                    PlayerName = player.Name,
                    BotHealth = 100,
                    BotStamina = 100,
                    BotScript = BotScripts.WalkAround
                };
                return View(vm);
            }

            return RedirectToAction(nameof(Play));
        }
    }
}