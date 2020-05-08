using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.ViewModels;

namespace Nemesys.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public IActionResult Index()
        {
            var model = new ReportsListViewModel();
            model.Reports = _reportRepository.GetAllReports().OrderByDescending(x => x.Upvotes);
            model.ReportsCount = model.Reports.Count();

            return View(model);
        }
    }
}