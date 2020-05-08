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
                    DateOfCreation = DateTime.Today,
                    Location = "Imsida",
                    DateSpotted = DateTime.Today,
                    TypeOfHazard = "Unsafe act",
                    Description = "Something something",
                    Status = "Open",
                    Email = "user@email.com",
                    Phone = "21000000",
                    ImageUrl = "",
                    Upvotes = 0
                },

                new Report()
                {
                    Id = 2,
                    DateOfCreation = DateTime.Today,
                    Location = "Imsida",
                    DateSpotted = DateTime.Today,
                    TypeOfHazard = "Structure",
                    Description = "Something something",
                    Status = "Open",
                    Email = "user2@email.com",
                    Phone = "21000001",
                    ImageUrl = "",
                    Upvotes = 0
                },
                new Report()
                {
                    Id = 3,
                    DateOfCreation = DateTime.Today,
                    Location = "Imsida",
                    DateSpotted = DateTime.Today,
                    TypeOfHazard = "Unsafe act ",
                    Description = "Something something",
                    Status = "Open",
                    Email = "user3@email.com",
                    Phone = "21000002",
                    ImageUrl = "",
                    Upvotes = 0
                }
            };
        }

        public void DeleteReport(Report report)
        {
            throw new NotImplementedException();
        }

        public void UpdateReport(Report report)
        {
            throw new NotImplementedException();
        }

        public void EditReport(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
