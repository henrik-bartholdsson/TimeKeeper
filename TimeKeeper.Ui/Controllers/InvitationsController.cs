using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TimeKeeper.Data.Models;
using TimeKeeper.Service.Services;

namespace TimeKeeper.Ui.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class InvitationsController : Controller
    {
        private readonly ITimeKeeperService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public InvitationsController(ITimeKeeperService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Route("/accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {

            var user = await _userManager.GetUserAsync(User);

            try
            {
                _service.AcceptInvotation(id, user.Id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager....";
                return View("ErrorPage");
            }

            return RedirectToAction("Index", "Home", "");
        }


        [Route("/reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _service.RejectInvitation(id, user.Id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ViewData["Prompt"] = "Please contact your manager....";
                return View("ErrorPage");
            }

            return RedirectToAction("Index", "Home", "");
        }


    }
}
