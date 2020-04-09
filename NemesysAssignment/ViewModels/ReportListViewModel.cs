using System;
using System.Collections.Generic;
using NemesysAssignment.Models;

namespace NemesysAssignment.ViewModels
{
    public class ReportListViewModel
    {
        public int TotalReports { get; set; }
        public IEnumerable<Report> Reports { get; set; }
    }
}
