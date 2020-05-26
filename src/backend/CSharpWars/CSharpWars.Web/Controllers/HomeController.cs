using System.Linq;
using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Constants;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Extensions;
using CSharpWars.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CSharpWars.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IPlayerLogic _playerLogic;
        private readonly ITemplateLogic _templateLogic;

        public HomeController(
            IConfiguration configuration,
            IPlayerLogic playerLogic,
            ITemplateLogic templateLogic)
        {
            _configuration = configuration;
            _playerLogic = playerLogic;
            _templateLogic = templateLogic;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                return RedirectToAction(nameof(Play));
            }

            var vm = new PlayerViewModel();
            return View(vm);
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
                    SampleScript = BotScripts.WalkAround,
                    IsTemplatePlayEnabled = _configuration.GetValue<bool>("ENABLE_TEMPLATE_PLAY"),
                    IsCustomPlayEnabled = _configuration.GetValue<bool>("ENABLE_CUSTOM_PLAY")
                };
                ViewData["ArenaUrl"] = _configuration.GetValue<string>("ARENA_URL");
                ViewData["ScriptTemplateUrl"] = _configuration.GetValue<string>("SCRIPT_TEMPLATE_URL");
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult LogOut()
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                HttpContext.Session.Remove("PLAYER");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}