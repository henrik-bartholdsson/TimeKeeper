using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Services;
using TimeKeeper.Ui.ViewModels;

namespace TimeKeeper.Ui.Controllers
{
    [Authorize]
    public class DeviationsController : Controller
    {
        private readonly ITimeKeeperService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeviationsController(ITimeKeeperService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index(string year, string month, int changeMonth, int organisationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            DateTime requestedDate;
            WorkMonthDto workMonth;

            var aa = _service.GetOrganisationsWhereUserIsMember(userId).ToList();

            if (organisationId == 0) // Must be refactored
            {
                organisationId = aa[0].Id;
            }

            try // Must be refactored
            {
                if (String.IsNullOrEmpty(year) || String.IsNullOrEmpty(month))
                {
                    workMonth = _service.GetLastWorkMonth(userId, organisationId);
                    return View(workMonth);
                }

                requestedDate = DateTime.Parse($"{year}/{month}/01").AddMonths(changeMonth);
                workMonth = _service.GetWorkMonth(userId, organisationId, requestedDate);

                if (workMonth == null)
                {
                    requestedDate = DateTime.Parse($"{year}/{month}/01");
                    workMonth = _service.GetWorkMonth(userId, organisationId, requestedDate);
                }

                return View(workMonth);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager....";
                return View("ErrorPage");
            }
        }


        [HttpGet]
        public IActionResult Add(string year, string month, int monthId)
        {
            var addDeviationViewModel = new DeviationsViewModel() { InputDeviation = new DeviationDto() };

            addDeviationViewModel.InputDeviation.RequestedDate = DateTime.Parse($"{year}/{month}/01");
            var deviationTypes = _service.GetAllDeviationTypes();

            addDeviationViewModel.SelectDaysInMonth = GetAllDaysInMonthAndYear(addDeviationViewModel.InputDeviation.RequestedDate.Year, addDeviationViewModel.InputDeviation.RequestedDate.Month);
            addDeviationViewModel.SelectDeviations = GetDeviationTypesToSelectList(deviationTypes);
            addDeviationViewModel.InputDeviation.WorkMonthId = monthId;

            return View("AddDeviation", addDeviationViewModel);
        }


        [HttpPost]
        public IActionResult Add(DeviationsViewModel addDeviationViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            addDeviationViewModel.InputDeviation.userId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _service.AddDeviation(addDeviationViewModel.InputDeviation);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    ViewData["Prompt"] = "Contact your manager.";
                    return View("ErrorPage");
                }
            }

            var deviationTypes = _service.GetAllDeviationTypes();

            addDeviationViewModel.SelectDaysInMonth = GetAllDaysInMonthAndYear(addDeviationViewModel.InputDeviation.RequestedDate.Year, addDeviationViewModel.InputDeviation.RequestedDate.Month);
            addDeviationViewModel.SelectDeviations = GetDeviationTypesToSelectList(deviationTypes);

            return View("AddDeviation", addDeviationViewModel);
        }







        #region None action methods
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

        #endregion
    }
}
