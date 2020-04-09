using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NemesysAssignment.Models;
using NemesysAssignment.ViewModels;

namespace NemesysAssignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public HomeController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        public IActionResult Index(string keyword)
        {
            ViewBag.Title = "Nemesys";

            var model = new ReportListViewModel();
            model.Reports = _reportRepository.GetAllReports().OrderByDescending(p => p.DateOfReport);
            model.TotalReports = model.Reports.Count();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
