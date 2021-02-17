using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using TimeKeeper.Service.Services;
using TimeKeeper.Ui.Models;

namespace TimeKeeper.Ui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITimeKeeperService _service;

        public HomeController(ILogger<HomeController> logger, ITimeKeeperService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            var invitations = _service.GetInvitations("Hej");



            return View(invitations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
