using System;
using System.Linq;
using CSharpWars.Poll.Helpers;
using CSharpWars.Poll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWars.Poll.Controllers
{
    public class HomeController : Controller
    {
        private const String VOTED = "VOTED";

        private readonly VoteHelper _voteHelper;

        public HomeController(VoteHelper voteHelper)
        {
            _voteHelper = voteHelper;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(VOTED))
            {
                return RedirectToAction(nameof(Result));
            }

            return View();
        }

        public IActionResult Backend()
        {
            HttpContext.Session.SetString(VOTED, VOTED);
            _voteHelper.Backend++;
            return RedirectToAction(nameof(Result));
        }

        public IActionResult Middleware()
        {
            HttpContext.Session.SetString(VOTED, VOTED);
            _voteHelper.Middleware++;
            return RedirectToAction(nameof(Result));
        }

        public IActionResult Frontend()
        {
            HttpContext.Session.SetString(VOTED, VOTED);
            _voteHelper.Frontend++;
            return RedirectToAction(nameof(Result));
        }

        public IActionResult Result()
        {
            var vm = new ResultViewModel
            {
                Backend = _voteHelper.Backend,
                Middleware = _voteHelper.Middleware,
                Frontend = _voteHelper.Frontend
            };
            return View(vm);
        }
    }
}