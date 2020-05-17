using Nemesys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class ReportsListViewModel
    {
        public int ReportsCount { get; set; }
        public IEnumerable<ReportWithUpvotes> ReportWithUpvotes { get; set; }
    }

    public class ReportWithUpvotes
    {
        public Report Report { get; set; }
        public int Upvotes { get; set; }
    }
}
