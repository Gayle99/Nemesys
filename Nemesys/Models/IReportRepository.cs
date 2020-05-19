using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public interface IReportRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportById(int id);
        void UpdateReport(Report report);
        void CreateReport(Report report);
        void DeleteReport(Report report);
        IEnumerable<Report> GetReportsThisYear();
    }
}
