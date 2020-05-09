using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastModified { get; set; }
        public string Location { get; set; }
        public DateTime DateSpotted { get; set; }
        public string TypeOfHazard { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public IdentityUser CreatedBy { get; set; }
        public string ImageUrl { get; set; }
        
    }
}
