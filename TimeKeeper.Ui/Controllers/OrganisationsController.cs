using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Dto;
using TimeKeeper.Service.Services;
using TimeKeeper.Ui.ViewModels;

namespace TimeKeeper.Ui.Controllers
{
    [Authorize]
    public class OrganisationsController : Controller
    {

        private readonly ITimeKeeperService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganisationsController(ITimeKeeperService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<OrganisationDto> organisations;
            OrganisationsViewModel organisationsViewModel = new OrganisationsViewModel(); ;

            try
            {
                organisations = _service.GetOrganisations(userId);
                organisationsViewModel = new OrganisationsViewModel();
                organisationsViewModel.Organisations = organisations;
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager....";
                return View("ErrorPage");
            }

            return View(organisationsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            OrganisationDto organisationDto = new OrganisationDto();
            List<OrganisationDto> storedOrganisations = new List<OrganisationDto>();

            
            try
            {
                storedOrganisations = _service.GetOrganisations(user.Id).ToList();
                organisationDto = _service.GetOrganisation(Id);
                storedOrganisations = storedOrganisations.Where(x => x.Id != organisationDto.Id).ToList();
                storedOrganisations.Add(new OrganisationDto { Id = 0, Name = "Set to parent" });
                organisationDto.Id = Id;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager....";
                return View("ErrorPage");
            }

            if (user.Id == organisationDto.OrganisationOwner)
                organisationDto.IsOwner = true;

            ViewBag.Parents = new SelectList(storedOrganisations, "Id", "Name", organisationDto.ParentOrganisationId);

            return View(organisationDto);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] OrganisationDto inputOrganisation)
        {
            try
            {
                _service.UpdateOrganisation(inputOrganisation);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager....";
                return View("ErrorPage");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add() // Max nr of org.
        {
            return View("Add");
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] OrganisationDto inputOrganisation)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _service.AddOrganisation(inputOrganisation.Name, user);
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }

            return RedirectToAction("Index");
        }
    }
}
