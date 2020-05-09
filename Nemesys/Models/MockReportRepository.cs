using System;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Models
{
    public class MockReportRepository : IReportRepository
    {
        private List<Report> _reports;

        public MockReportRepository()
        {
            if (_reports == null)
            {
                InitializeReports();
            }
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _reports;
        }

        public Report GetReportById(int reportId)
        {
            return _reports.FirstOrDefault(p => p.Id == reportId);
        }

        public void CreateReport(Report report)
        {
            report.Id = _reports.Count + 1;
            _reports.Add(report);
        }

        private void InitializeReports()
        {
            _reports = new List<Report>() {
                new Report()
                {
                    Id = 1,
                    Title = "This is a report",
                    DateOfCreation = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Location = "Faculty of ICT",
                    DateSpotted = DateTime.UtcNow,
                    TypeOfHazard = "Unsafe act",
                    Description = "Something something",
                    Status = "Open",
                    ImageUrl = "/images/img01.jpg",
                },

                new Report()
                {
                    Id = 2,
                    Title = "This is another report",
                    DateOfCreation = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Location = "Faculty of Engineering",
                    DateSpotted = DateTime.UtcNow,
                    TypeOfHazard = "Structure",
                    Description = "Something something",
                    Status = "Open",
                    ImageUrl = "/images/img02.jpg"
                },
                new Report()
                {
                    Id = 3,
                    Title = "You know what this is",
                    DateOfCreation = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Location = "Faculty of Faculties",
                    DateSpotted = DateTime.UtcNow,
                    TypeOfHazard = "Unsafe act ",
                    Description = "Something something",
                    Status = "Open",
                    ImageUrl = "/images/img03.jpg"
                }
            };
        }

        public void DeleteReport(Report report)
        {
            _reports.Remove(report);
        }

        public void UpdateReport(Report report)
        {
            var current = _reports.SingleOrDefault(x => x.Id == report.Id);

            if(current != null)
            {
                current.Id = report.Id;
                current.Title = report.Title;
                current.LastModified = DateTime.UtcNow;
                current.Location = report.Location;
                current.TypeOfHazard = report.TypeOfHazard;
                current.Description = report.Description;
                current.ImageUrl = report.ImageUrl;
            }

        }

    }
}
