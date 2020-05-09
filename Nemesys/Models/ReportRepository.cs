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
                current.Id = report.Id;
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
    }
}
