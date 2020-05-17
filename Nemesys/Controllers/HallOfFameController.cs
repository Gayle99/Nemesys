using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;

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

            return View();
        }
    }
}