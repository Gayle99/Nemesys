using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.ViewModels;

namespace Nemesys.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IPromotionRepository promotionRepository)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _promotionRepository = promotionRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult PromoteRequest()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PromoteRequest([Bind("Reason")] PromoteViewModel promoteViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            Promote existingRequest = _promotionRepository.GetUserById(user.Id);

            if (existingRequest == null)
            {
                if (ModelState.IsValid)
                {
                    Promote newRequest = new Promote()
                    {
                        User = await _userManager.GetUserAsync(User),
                        Reason = promoteViewModel.Reason
                    };

                    _promotionRepository.PromotionRequest(newRequest);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(promoteViewModel);
                }
            }
            else
            {
                return RedirectToAction("PromoteRequest");
            }

        }

        [HttpGet]
        public IActionResult ExistingRequest()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult RequestList()
        {
            var requests = _promotionRepository.GetAllPromotionRequests();
            return View(requests.ToList());
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult RequestList(string id)
        {
            ApplicationUser user = _promotionRepository.GetUserById(id).User;

            if (user != null)
            {
                _userManager.AddToRoleAsync(user, "Investigator").Wait();
            }

            var requests = _promotionRepository.GetAllPromotionRequests();
            return View(requests.ToList());
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult DeleteRequest(int id)
        {
            Promote request = _promotionRepository.GetRequestById(id);
            if (request != null)
            {
                _promotionRepository.DeleteRequest(request);
                return RedirectToAction("RequestList");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
