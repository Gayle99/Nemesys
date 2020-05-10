using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class ReportUpvoted
    {
        public int Id { get; set; }
        public Report Report { get; set; }
        public IdentityUser User { get; set; }

    }
}
