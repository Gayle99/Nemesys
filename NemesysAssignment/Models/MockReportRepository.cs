using System;
using System.Collections.Generic;
using System.Linq;

namespace NemesysAssignment.Models
{
    public class MockReportRepository : IReportRepository
    {
        private List<Report> _reports;

        public MockReportRepository()
        {
            if(_reports == null)
            {
                InitializeReports();
            }
        }

        public void CreateReport(Report report)
        {
            report.Id = _reports.Count + 1;
            _reports.Add(report);
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _reports;
        }

        public Report GetReportById(int reportId)
        {
            return _reports.FirstOrDefault(p => p.Id == reportId);
        }

        private void InitializeReports()
        {
            _reports = new List<Report>() {
                new Report()
                {
                    Id = 1,
                    Title = "Hazard in ICT Building",
                    DateOfReport = DateTime.Today,
                    Location = "Faculty of ICT",
                    HazardSpotted = DateTime.Today,
                    HazardType = "Equipment",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis consequat tortor felis, id tristique mi varius sit amet. Pellentesque pellentesque euismod purus eget iaculis. Nullam ut scelerisque lacus, ac egestas velit.",
                    Status = "Open",
                    UserEmail = "user1@gmail.com",
                    UserPhone = "21000000",
                    ImageUrl = "/images/image1.jpg",
                    Upvotes = 0
                },

                new Report()
                {
                    Id = 2,
                    Title = "Hazard in Quadrangle",
                    DateOfReport = DateTime.Today,
                    Location = "Quadrangle",
                    HazardSpotted = DateTime.Today,
                    HazardType = "Unsafe Act",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis consequat tortor felis, id tristique mi varius sit amet. Pellentesque pellentesque euismod purus eget iaculis. Nullam ut scelerisque lacus, ac egestas velit.",
                    Status = "Open",
                    UserEmail = "user2@gmail.com",
                    UserPhone = "21110000",
                    ImageUrl = "/images/image2.jpg",
                    Upvotes = 0
                },
            };
        }
    }
}
