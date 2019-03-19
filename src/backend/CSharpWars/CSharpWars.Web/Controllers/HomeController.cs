using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
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
        private readonly IBotLogic _botLogic;

        public HomeController(IPlayerLogic playerLogic, IBotLogic botLogic)
        {
            _playerLogic = playerLogic;
            _botLogic = botLogic;
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
                    Scripts = BotScripts.All
                };
                return View(vm);
            }

            return RedirectToAction(nameof(Play));
        }

        [HttpPost]
        public async Task<IActionResult> Play(GameViewModel vm)
        {
            if (HttpContext.Session.Keys.Contains("PLAYER"))
            {
                var valid = IsValid(vm);
                if (valid)
                {
                    var player = HttpContext.Session.GetObject<PlayerDto>("PLAYER");

                    var botToCreate = new BotToCreateDto
                    {
                        PlayerId = player.Id,
                        Name = vm.BotName,
                        MaximumHealth = vm.BotHealth,
                        MaximumStamina = vm.BotStamina,
                        Script = BotScripts.All.Single(x => x.Id == vm.SelectedScript).Script.Base64Encode()
                    };
                    await _botLogic.CreateBot(botToCreate);

                    vm.PlayerName = player.Name;
                }

                vm = new GameViewModel
                {
                    PlayerName = vm.PlayerName,
                    BotHealth = 100,
                    BotStamina = 100,
                    Scripts = BotScripts.All
                };

                if (valid)
                {
                    vm.HappyMessage = $"{vm.BotName} for player {vm.PlayerName} has been created successfully!";
                }
                else
                {
                    vm.SadMessage = "You have made some errors!";
                }

                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        private Boolean IsValid(GameViewModel vm)
        {
            return !String.IsNullOrEmpty(vm.BotName) && vm.BotHealth > 0 && vm.BotStamina > 0 && vm.SelectedScript != Guid.Empty;
        }
    }
}