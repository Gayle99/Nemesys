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
    public class HallOfFameController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IInvestigationRepository _investigationRepository;
        private readonly ReportUpvotedRepository _reportUpvotedRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public HallOfFameController(IReportRepository reportRepository, IInvestigationRepository investigationRepository, ReportUpvotedRepository reportUpvotedRepository, UserManager<IdentityUser> userManager)
        {
            _reportRepository = reportRepository;
            _investigationRepository = investigationRepository;
            _reportUpvotedRepository = reportUpvotedRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            try
            {
                var reportsThisYear = _reportRepository.GetReportsThisYear();
                Dictionary<IdentityUser, int> userDictionary = new Dictionary<IdentityUser, int>();
                foreach (Report report in reportsThisYear)
                {
                    if (userDictionary.Count > 0)
                    {
                        if (userDictionary.ContainsKey(report.CreatedBy))
                        {
                            userDictionary[report.CreatedBy] = userDictionary[report.CreatedBy]++;
                        }
                    }
                    else
                    {
                        userDictionary.Add(report.CreatedBy, 1);
                    }
                }

                var model = new HallOfFameViewModel();
                model.HighestUpvotedReportList = new List<HighestUpvotedReport>();
                foreach (var item in userDictionary)
                {
                    HighestUpvotedReport temp = new HighestUpvotedReport()
                    {
                        User = item.Key,
                        Count = item.Value,
                        HighestReport = _reportRepository.GetAllReports().OrderByDescending(x => _reportUpvotedRepository.TotalUpvotes(x)).ToArray()[0]
                    };
                    model.HighestUpvotedReportList.Add(temp);
                }
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}