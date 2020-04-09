using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NemesysAssignment.Models
{
    public interface IReportRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportById(int reportId);
        void CreateReport(Report newReport);
    }
}
