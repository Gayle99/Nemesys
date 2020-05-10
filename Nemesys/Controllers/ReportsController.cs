﻿using System;
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
        private readonly IInvestigationRepository _investigationRepository;
        private readonly ReportUpvotedRepository _reportUpvotedRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportsController(IReportRepository reportRepository, IInvestigationRepository investigationRepository,ReportUpvotedRepository reportUpvotedRepository, UserManager<IdentityUser> userManager)
        {
            _reportRepository = reportRepository;
            _investigationRepository = investigationRepository;
            _reportUpvotedRepository = reportUpvotedRepository;
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
            if (report != null) {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    if (_reportUpvotedRepository.CheckIfUpvoted(report, user))
                    {
                        var upvote = _reportUpvotedRepository.GetUpvote(report, user);
                        if(upvote != null)
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
            return RedirectToAction("Details", report.Id);
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult CreateInvestigation(int reportId)
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvestigation([Bind("Id", "Description")] CreateInvestigationViewModel newInvestigation)
        {
            if (ModelState.IsValid)
            {

                Investigation investigation = new Investigation()
                {
                    Id = newInvestigation.Id,
                    Investigator = await _userManager.GetUserAsync(User),
                    DateOfAction = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Description = newInvestigation.Description

                };

                _investigationRepository.CreateInvestigation(investigation);
                return RedirectToAction("Details", newInvestigation.Id);
            }
            else
            {
                return View(newInvestigation);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditInvestigation(int id)
        {
            var investigation = _investigationRepository.GetInvestigatiosById(id);
            if (investigation != null)
            {
                CreateInvestigationViewModel model = new CreateInvestigationViewModel()
                {
                    Description = investigation.Description,
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
        public async Task<IActionResult> EditInvestigation(int id, [Bind("Description")] CreateInvestigationViewModel editInvestigation)
        {


            //1. Check for incoming data integrity
            if (id != editInvestigation.Id)
            {
                return NotFound();
            }

            //2. Check if the user has access to this blog post
            var existingInvestigation = _investigationRepository.GetInvestigatiosById(id);
            if (existingInvestigation != null)
            {
                var loggedIn = await _userManager.GetUserAsync(User);
                if (loggedIn.Id == existingInvestigation.Investigator.Id)
                {
                    //3. Validate model
                    if (ModelState.IsValid)
                    {

                        Investigation investigation = new Investigation()
                        {
                            Id = editInvestigation.Id,
                            Description = editInvestigation.Description
                        };

                        _investigationRepository.UpdateInvestigation(investigation);
                        return RedirectToAction("Details", id);
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var report = _reportRepository.GetReportById(id);
            if (report != null)
            {
                var investigation = _investigationRepository.GetInvestigatiosById(id);
                if (investigation != null)
                {
                    var investigator = await _userManager.GetUserAsync(User);
                    if(investigator.Id == investigation.Investigator.Id)
                    {
                        Report update = new Report()
                        {
                            Id = id,
                            Status = status
                        };

                        _reportRepository.UpdateReport(update);
                        return RedirectToAction("Details", id);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    //If there is no investigation
                    return NotFound();
                }
                
            }
            else
            {
                return NotFound();
            }
        } 
    }
}