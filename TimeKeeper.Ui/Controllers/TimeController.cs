using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Services;
using TimeKeeper.Ui.Models;

namespace TimeKeeper.Ui.Controllers
{
    public class TimeController : Controller
    {
        private readonly ITimeKeeperService _service;

        public TimeController(ITimeKeeperService service)
        {
            _service = service;
        }

        public IActionResult Index(string currentShownDate, int changeMonth)
        {
            DateTime requestedDate;
            if(String.IsNullOrEmpty(currentShownDate))
            {
                currentShownDate = DateTime.Now.ToString();
                requestedDate = DateTime.Parse(currentShownDate).AddMonths(changeMonth);
            }
            else
            {
                requestedDate = DateTime.Parse(currentShownDate).AddMonths(changeMonth);
            }

            // Default ska vara att hämta den älsta månaden som inte är submitted
            // Finns ingen månad för användaren, vad händer då?


            try
            {
                var workMonth = _service.GetWorkMonthByUserId("Hej", requestedDate.Month, 2021);
                ViewData["DateShown"] = requestedDate;
                ViewData["Year"] = requestedDate.Date.ToString("yyyy");
                return View(workMonth);
            }
            catch
            {
                requestedDate = DateTime.Parse(currentShownDate);
                var workMonth = _service.GetWorkMonthByUserId("Hej", requestedDate.Month, 2021);
                ViewData["DateShown"] = requestedDate;
                ViewData["Year"] = requestedDate.Date.ToString("yyyy");
                return View(workMonth);
            }
        }


        public IActionResult Add()
        {
            var deviationTypes = _service.GetAllDeviationTypes();


            ViewData["Deviations"] = new SelectList(deviationTypes, "Id", "InfoText");


            return View("AddDeviation");
        }


        [HttpPost]
        public IActionResult Add(DeviationDto deviation)
        {
            if (ModelState.IsValid)
            {
                deviation.WorkMonthId = 1;
                try
                {
                    _service.AddDeviation(deviation);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                    return View("Error", ex);
                }
            }

            var deviationTypes = _service.GetAllDeviationTypes();

            ViewData["Deviations"] = new SelectList(deviationTypes, "Id", "InfoText", deviation.DeviationType);

            return View("AddDeviation");
        }
    }
}
