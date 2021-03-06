using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Services;
using TimeKeeper.Ui.Models;
using TimeKeeper.Ui.ViewModels;

namespace TimeKeeper.Ui.Controllers
{
    [Authorize]
    public class TimeController : Controller
    {
        private readonly ITimeKeeperService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimeController(ITimeKeeperService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index(string currentShownDate, int changeMonth)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            DateTime requestedDate;
            //if (String.IsNullOrEmpty(currentShownDate))
            //    currentShownDate = DateTime.Now.ToString();

            if (String.IsNullOrEmpty(currentShownDate))
            {
                try
                {
                    var workMonth = _service.GetLastWorkMonthByUserId(userId);
                    requestedDate = DateTime.Parse($"{workMonth.Year}-{workMonth.Month}-01");

                    ViewData["DateShown"] = requestedDate;
                    return View(workMonth);
                }
                catch(Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    ViewData["Prompt"] = "Please contact your manager....";
                    return View("ErrorPage");
                }
            }

            requestedDate = DateTime.Parse(currentShownDate).AddMonths(changeMonth);

            // Default ska vara att hämta den älsta månaden som inte är submitted
            // Finns ingen månad för användaren, vad händer då?

            try
            {
                var workMonth = _service.GetWorkMonthByUserId(userId, requestedDate.Month, requestedDate.Year);

                if (workMonth == null)
                {
                    requestedDate = DateTime.Parse(currentShownDate);
                    workMonth = _service.GetWorkMonthByUserId(userId, requestedDate.Month, requestedDate.Year);
                }

                if (workMonth == null)
                    throw new Exception(@"Requested month could not be found.");

                requestedDate = DateTime.Parse($"{workMonth.Year}-{workMonth.Month}-01");

                ViewData["DateShown"] = requestedDate;

                return View(workMonth);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager.";
                return View("ErrorPage");
            }
        }


        [HttpGet]
        public IActionResult Add(string currentShownDate, int monthId)
        {
            var addDeviationViewModel = new AddDeviationViewModel() { Deviation = new DeviationDto() };

            addDeviationViewModel.Deviation.RequestedDate = DateTime.Parse(currentShownDate);
            var deviationTypes = _service.GetAllDeviationTypes();

            addDeviationViewModel.SelectDaysInMonth = GetAllDaysInMonthAndYear(addDeviationViewModel.Deviation.RequestedDate.Year, addDeviationViewModel.Deviation.RequestedDate.Month);
            addDeviationViewModel.SelectDeviations = GetDeviationTypesToSelectList(deviationTypes);
            addDeviationViewModel.Deviation.WorkMonthId = monthId;

            return View("AddDeviation", addDeviationViewModel);
        }



        [HttpPost]
        public IActionResult Add(AddDeviationViewModel addDeviationViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            addDeviationViewModel.Deviation.userId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _service.AddDeviation(addDeviationViewModel.Deviation); // I denna metod måste man kolla så att användaren får lägga till deviation. + Kolla month Id
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }

            var deviationTypes = _service.GetAllDeviationTypes();

            addDeviationViewModel.SelectDaysInMonth = GetAllDaysInMonthAndYear(addDeviationViewModel.Deviation.RequestedDate.Year, addDeviationViewModel.Deviation.RequestedDate.Month);
            addDeviationViewModel.SelectDeviations = GetDeviationTypesToSelectList(deviationTypes);

            return View("AddDeviation", addDeviationViewModel);
        }




        private SelectList GetDeviationTypesToSelectList(IEnumerable<DeviationTypeDto> deviationTypes)
        {
            return new SelectList(deviationTypes, "Id", "InfoText");
        }


        private IEnumerable<SelectListItem> GetAllDaysInMonthAndYear(int year, int month)
        {
            var nDays = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                    .Select(day => new DateTime(year, month, day))
                    .ToList();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var d in nDays)
            {
                items.Add(new SelectListItem
                {
                    Value = d.Day.ToString(),
                    Text = d.Date.ToString("d/MM") + " - " + d.DayOfWeek.ToString()
                });
            }

            return items;
        }
    }
}
