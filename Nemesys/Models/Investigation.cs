using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class Investigation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAction { get; set; }
        public DateTime LastModified { get; set; }
        public IdentityUser Investigator { get; set; }

    }
}
