using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeKeeper.Service.Services;

namespace TimeKeeper.Ui.Controllers
{
    public class TimeController : Controller
    {
        private readonly ITimeKeeperService _service;

        public TimeController(ITimeKeeperService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var stuff = _service.GetWorkMonthByUserId("Hej", 1, 2021);

            return View(stuff);
        }
    }
}
