using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            model.Reports = _reportRepository.GetAllReports().OrderByDescending(x => x.UsersWhoUpvoted.Count);
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
        [Authorize]
        public IActionResult CreateReport()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReport([Bind("Title", "Location", "DateSpotted", "TypeOfHazard", "Description", "Status", "Email", "Phone", "ImageToUpload")] CreateReportViewModel newReport)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (newReport.ImageToUpload != null)
                {
                    //At this point you should check size, extension etc...
                    //.....
                    //Then persist using a new name for consistency (e.g. new Guid)
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
                    Status = newReport.Status,
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

        [HttpGet]
        [Authorize]
        public IActionResult EditReport(int id)
        {
            var report = _reportRepository.GetReportById(id);
            if(report != null)
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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReport(int id, [Bind("Title", "Location", "DateSpotted", "TypeOfHazard", "Description", "Status", "ImageToUpload", "ImageUrl")] CreateReportViewModel newReport)
        {
            

            //1. Check for incoming data integrity
            if (id != newReport.Id)
            {
                return NotFound();
            }

            //2. Check if the user has access to this blog post
            var existingReport = _reportRepository.GetReportById(id);
            if(existingReport != null)
            {
                var loggedIn = await _userManager.GetUserAsync(User);
                if(loggedIn.Id == existingReport.CreatedBy.Id)
                {
            //3. Validate model
                    if (ModelState.IsValid)
                    {
                        if (newReport.ImageToUpload != null)
                        {
                            string fileName = "";
                            //At this point you should check size, extension etc...
                            //.....
                            //Then persist using a new name for consistency (e.g. new Guid)
                            var extension = "." + newReport.ImageToUpload.FileName.Split('.')[newReport.ImageToUpload.FileName.Split('.').Length - 1];
                            fileName = Guid.NewGuid().ToString() + extension;
                            var path = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\reports\\" + fileName;
                            using (var bits = new FileStream(path, FileMode.Create))
                            {
                                newReport.ImageToUpload.CopyTo(bits);
                            }

                            newReport.ImageUrl = "/images/reports/" + fileName;
                        }

                        Report report = new Report()
                        {
                            Id = newReport.Id,
                            Title = newReport.Title,
                            Location = newReport.Location,
                            TypeOfHazard = newReport.TypeOfHazard,
                            Description = newReport.Description,
                            ImageUrl = newReport.ImageUrl
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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = _reportRepository.GetReportById(id);
            if (report != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if(currentUser.Id == report.CreatedBy.Id)
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpvoteReport(int reportId)
        {
            var report = _reportRepository.GetReportById(reportId);
            if (report != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    if (report.UsersWhoUpvoted.Contains(user))
                    {
                        report.UsersWhoUpvoted.Remove(user); //Remove the upvote
                    }
                    else
                    {
                        report.UsersWhoUpvoted.Add(user); //Add the user
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
            return null;
        }
    }
}