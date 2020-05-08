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
        public IEnumerable<Report> Reports { get; set; }
    }
}
