using Microsoft.AspNetCore.Identity;
using Nemesys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class ReportRepository:IReportRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ReportRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _applicationDbContext.Reports;
        }

        public Report GetReportById(int reportId)
        {
            return _applicationDbContext.Reports.FirstOrDefault(p => p.Id == reportId);
        }

        public void CreateReport(Report report)
        {
            _applicationDbContext.Reports.Add(report);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteReport(Report report)
        {
            _applicationDbContext.Reports.Remove(report);
            _applicationDbContext.SaveChanges();
        }

        public void UpdateReport(Report report)
        {
            var current = _applicationDbContext.Reports.SingleOrDefault(x => x.Id == report.Id);

            if (current != null)
            {
                current.Title = report.Title;
                current.LastModified = DateTime.UtcNow;
                current.Location = report.Location;
                current.TypeOfHazard = report.TypeOfHazard;
                current.Description = report.Description;
                current.ImageUrl = report.ImageUrl;

                _applicationDbContext.Entry(current).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _applicationDbContext.SaveChanges();
            }

        }

        public void UpdateStatus(Report report)
        {
            var current = _applicationDbContext.Reports.SingleOrDefault(x => x.Id == report.Id);

            if (current != null)
            {
                current.Status = report.Status;

                _applicationDbContext.Entry(current).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _applicationDbContext.SaveChanges();
            }
        }

        public Report[] GetReportsByUser(IdentityUser user){
            Report[] reports = _applicationDbContext.Reports.Where(x => x.CreatedBy.Id.Equals(user.Id)).ToArray();
            return reports;
        }

        public IEnumerable<Report> GetReportsThisYear()
        {
            return _applicationDbContext.Reports.Where(x => x.DateOfCreation.Year == DateTime.UtcNow.Year);

        }

        public Report HighestReportOfUser(IdentityUser user)
        {
            Report[] reports = _applicationDbContext.Reports.Where(x => x.CreatedBy.Id.Equals(user.Id)).ToArray();
            return null;
        }
    }
}
