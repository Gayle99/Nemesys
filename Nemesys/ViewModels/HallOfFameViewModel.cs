using Microsoft.AspNetCore.Identity;
using Nemesys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class HallOfFameViewModel
    {
        public List<HighestUpvotedReport> HighestUpvotedReportList { get; set; }
    }

    public class HighestUpvotedReport
    {
        public IdentityUser User { get; set; }
        public int Count { get; set; }
        public Report HighestReport { get; set; }
    }

}
