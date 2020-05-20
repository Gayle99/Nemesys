using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class CreateReportViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A title is required")]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [Required(ErrorMessage = "A location is required")]
        [StringLength(50, MinimumLength = 1)]
        public string Location { get; set; }

        public DateTime DateSpotted { get; set; }
        public string TypeOfHazard { get; set; }

        [Required(ErrorMessage = "Report description is required")]
        [StringLength(1500, MinimumLength = 1, ErrorMessage = "Report description cannot be longer than 1500 characters")]
        public string Description { get; set; }

        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageToUpload { get; set; }

    }
}
