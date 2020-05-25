using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class ChangeStatusViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Status option is required!")]
        public int Option { get; set; }
    }
}
