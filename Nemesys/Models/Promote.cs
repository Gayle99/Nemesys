using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nemesys.Models
{
    public class Promote
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public string Reason { get; set; }
    }
}
