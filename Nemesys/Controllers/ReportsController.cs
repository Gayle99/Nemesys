using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.ViewModels;

namespace Nemesys.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportsController(IReportRepository reportRepository, UserManager<IdentityUser> userManager)
        {
            _reportRepository = reportRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            
            var model = new ReportsListViewModel();
            model.Reports = _reportRepository.GetAllReports().OrderByDescending(x => x.Upvotes);
            model.ReportsCount = model.Reports.Count();
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var report = _reportRepository.GetReportById(id);
            if (report == null)
            {
                return NotFound();
            }
            else
            {
                return View(report);
            }
        }

        [HttpGet]
        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }


    }
}