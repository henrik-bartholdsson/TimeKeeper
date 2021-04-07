using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


            var a = id;


            return null;
        }

        [Route("/reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            _service.RejectInvitation(id, user.Id);

            return RedirectToAction("Index", "Home", "");
        }


    }
}
