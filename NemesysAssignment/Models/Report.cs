using System;
namespace NemesysAssignment.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateOfReport { get; set; }
        public string Location { get; set; }
        public DateTime HazardSpotted { get; set; }
        public string HazardType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string ImageUrl { get; set; }
        public int Upvotes { get; set; }
    }
}
