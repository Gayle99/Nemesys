
﻿using System;
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
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IPromotionRepository promotionRepository, RoleManager<IdentityRole> roleManager)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _promotionRepository = promotionRepository;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Reporter")]
        public IActionResult PromoteRequest()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles= "Reporter")]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("ExistingRequest");
            }

        }

        [HttpGet]
        [Authorize(Roles="Reporter")]
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
        [ValidateAntiForgeryToken]
        public IActionResult RequestList(string id)
        {
            IdentityUser user = _promotionRepository.GetUserById(id).User;

            if (user != null)
            {
                _userManager.AddToRoleAsync(user, "Investigator").Wait();
            }

            var requests = _promotionRepository.GetAllPromotionRequests();

            bool remainingApplications = true;
            while (remainingApplications)
            {
                Promote request = _promotionRepository.GetUserById(id);
                if (request != null)
                {
                    _promotionRepository.DeleteRequest(request);
                }
                else
                {
                    remainingApplications = false;
                }
            }

            return View(requests.ToList());
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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