using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class CreateInvestigationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Report description is required")]
        [StringLength(1500, MinimumLength = 1, ErrorMessage = "Report description cannot be longer than 1500 characters")]
        public string Description { get; set; }

    }
}
