using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class CreateReportViewModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime DateSpotted { get; set; }
        public string TypeOfHazard { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public int Upvotes { get; set; }
        public int MyProperty { get; set; }
    }
}
