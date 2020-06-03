using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nemesys.Models;
using Nemesys.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nemesys.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IInvestigationRepository _investigationRepository;
        private readonly ReportUpvotedRepository _reportUpvotedRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ReportsController> _logger;
        private readonly IConfiguration _configuration;

        public ReportsController(IReportRepository reportRepository, IInvestigationRepository investigationRepository, ReportUpvotedRepository reportUpvotedRepository, UserManager<IdentityUser> userManager, ILogger<ReportsController> logger, IConfiguration configuration)
        {
            _reportRepository = reportRepository;
            _investigationRepository = investigationRepository;
            _reportUpvotedRepository = reportUpvotedRepository;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            var model = new ReportsListViewModel();
            try
            {
                var reports = _reportRepository.GetAllReports().OrderByDescending(x => _reportUpvotedRepository.TotalUpvotes(x));
                model.ReportWithUpvotes = new List<ReportWithUpvotes>();
                foreach (var report in reports)
                {
                    ReportWithUpvotes temp = new ReportWithUpvotes()
                    {
                        Report = report,
                        Upvotes = _reportUpvotedRepository.TotalUpvotes(report)
                    };
                    model.ReportWithUpvotes.Add(temp);
                }
                model.ReportsCount = model.ReportWithUpvotes.Count;
                return View(model);
            }catch(ArgumentNullException n)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + n.Message);
                return RedirectToAction("Error", "Home");

            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var model = new DetailsViewModel();
                var report = _reportRepository.GetReportById(id);
                var investigation = _investigationRepository.GetInvestigationByReportId(report);

                if (report == null)
                {
                    return NotFound();
                }
                else
                {
                    model.Report = report;
                    model.Upvotes = _reportUpvotedRepository.TotalUpvotes(report);
                    model.Investigation = investigation;
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        [Authorize(Roles = "Reporter")]
        public IActionResult CreateReport()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Reporter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReport([Bind("Title", "Location", "DateSpotted", "TypeOfHazard", "Description", "Status", "Email", "Phone", "ImageToUpload")] CreateReportViewModel newReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    if (newReport.ImageToUpload != null)
                    {
                        var extension = "." + newReport.ImageToUpload.FileName.Split('.')[newReport.ImageToUpload.FileName.Split('.').Length - 1];
                        fileName = Guid.NewGuid().ToString() + extension;
                        var path = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\" + fileName;
                        using (var bits = new FileStream(path, FileMode.Create))
                        {
                            newReport.ImageToUpload.CopyTo(bits);
                        }
                    }

                    Report report = new Report()
                    {
                        Title = newReport.Title,
                        CreatedBy = await _userManager.GetUserAsync(User),
                        DateOfCreation = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        Location = newReport.Location,
                        DateSpotted = newReport.DateSpotted,
                        TypeOfHazard = newReport.TypeOfHazard,
                        Description = newReport.Description,
                        Status = "Open",
                        ImageUrl = "/images/" + fileName,

                    };

                    _reportRepository.CreateReport(report);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newReport);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Reporter")]
        public IActionResult EditReport(int reportId)
        {
            try
            {
                var report = _reportRepository.GetReportById(reportId);
                if (report != null)
                {
                    CreateReportViewModel model = new CreateReportViewModel()
                    {
                        Id = report.Id,
                        Title = report.Title,
                        Location = report.Location,
                        TypeOfHazard = report.TypeOfHazard,
                        Description = report.Description,
                        ImageUrl = report.ImageUrl
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Reporter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReport(int id, [Bind("Title", "Location", "DateSpotted", "TypeOfHazard", "Description", "Status", "ImageToUpload")] CreateReportViewModel newReport)
        {

            try
            {

                //1. Check if the user has access to this blog post
                var existingReport = _reportRepository.GetReportById(id);
                if (existingReport != null)
                {
                    var loggedIn = await _userManager.GetUserAsync(User);
                    if (loggedIn.Id == existingReport.CreatedBy.Id)
                    {
                        //2. Validate model
                        if (ModelState.IsValid)
                        {

                            Report report = new Report()
                            {
                                Id = existingReport.Id,
                                Title = newReport.Title,
                                Location = newReport.Location,
                                TypeOfHazard = newReport.TypeOfHazard,
                                Description = newReport.Description
                            };

                            _reportRepository.UpdateReport(report);
                            return RedirectToAction("Index");
                        }
                        else
                            return View(newReport);
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        [Authorize(Roles = "Reporter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(int reportId)
        {
            try
            {
                var report = _reportRepository.GetReportById(reportId);
                if (report != null)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    if (currentUser.Id == report.CreatedBy.Id)
                    {
                        _reportRepository.DeleteReport(report);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpvoteReport(int reportId)
        {
            try
            {
                var report = _reportRepository.GetReportById(reportId);
                if (report != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        if (_reportUpvotedRepository.CheckIfUpvoted(report, user))
                        {
                            var upvote = _reportUpvotedRepository.GetUpvote(report, user);
                            if (upvote != null)
                            {
                                _reportUpvotedRepository.RemoveUpvote(upvote);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        else
                        {
                            ReportUpvoted newUpvote = new ReportUpvoted()
                            {
                                Report = report,
                                User = user
                            };
                            _reportUpvotedRepository.AddUpvote(newUpvote);

                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Investigator")]
        public IActionResult CreateInvestigation(int reportId)
        {
            try
            {
                var report = _reportRepository.GetReportById(reportId);
                var investigation = _investigationRepository.GetInvestigationByReportId(report);
                if (investigation == null)
                {
                    CreateInvestigationViewModel model = new CreateInvestigationViewModel
                    {
                        Id = reportId
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        [Authorize(Roles = "Investigator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvestigation([Bind("Id", "Description")] CreateInvestigationViewModel newInvestigation)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Investigation investigation = new Investigation()
                    {
                        AssociatedReport = _reportRepository.GetReportById(newInvestigation.Id),
                        Investigator = await _userManager.GetUserAsync(User),
                        DateOfAction = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        Description = newInvestigation.Description

                    };

                    _investigationRepository.CreateInvestigation(investigation);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newInvestigation);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Investigator")]
        public IActionResult EditInvestigation(int investigationId)
        {
            try
            {
                var investigation = _investigationRepository.GetInvestigatiosById(investigationId);
                if (investigation != null)
                {
                    CreateInvestigationViewModel model = new CreateInvestigationViewModel()
                    {
                        Id = investigationId,
                        Description = investigation.Description
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Investigator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInvestigation([Bind("Id","Description")] CreateInvestigationViewModel editInvestigation)
        {
            try
            {


                //1. Check if the user has access to this blog post
                var existingInvestigation = _investigationRepository.GetInvestigatiosById(editInvestigation.Id);
                if (existingInvestigation != null)
                {
                    var loggedIn = await _userManager.GetUserAsync(User);
                    if (loggedIn.Id == existingInvestigation.Investigator.Id)
                    {
                        //2. Validate model
                        if (ModelState.IsValid)
                        {

                            Investigation investigation = new Investigation()
                            {
                                Id = editInvestigation.Id,
                                Description = editInvestigation.Description
                            };

                            _investigationRepository.UpdateInvestigation(investigation);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return View(editInvestigation);
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }

        }


        [HttpPost]
        [Authorize(Roles = "Investigator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int option)
        {
            try
            {
                var report = _reportRepository.GetReportById(id);
                if (report != null)
                {
                    var investigation = _investigationRepository.GetInvestigationByReportId(report);
                    if (investigation != null)
                    {
                        string status = null;
                        if (option == 1)
                        {
                            status = "Open";
                        }
                        else if (option == 2)
                        {
                            status = "Closed";
                        }
                        else if (option == 3)
                        {
                            status = "Being investigated";
                        }
                        else if (option == 4)
                        {
                            status = "No action required";
                        }

                        if (status != null)
                        {
                            var investigator = await _userManager.GetUserAsync(User);
                            if (investigator.Id == investigation.Investigator.Id)
                            {
                                Report update = new Report()
                                {
                                    Id = id,
                                    Status = status
                                };

                                _reportRepository.UpdateStatus(update);

                                if (status.Equals("Closed"))
                                {
                                    var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
                                    var client = new SendGridClient(apiKey);
                                    var from = new EmailAddress("nemesysgj@gmail.com", "Nemesys");
                                    var subject = report.Title + " - Closed";
                                    var to = new EmailAddress(report.CreatedBy.Email, report.CreatedBy.UserName);
                                    var plainTextContent = investigation.Description;
                                    var htmlContent = investigation.Description;
                                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                                    var response = await client.SendEmailAsync(msg);
                                }
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return Unauthorized();
                            }
                        }
                        else
                        {
                            return RedirectToAction("Error", "Home");
                        }

                    }
                    else
                    {
                        //If there is no investigation
                        return RedirectToAction("Error", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.UtcNow + "\t" + e.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}