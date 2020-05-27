using Microsoft.EntityFrameworkCore;
using Nemesys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class InvestigationRepository : IInvestigationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public InvestigationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void CreateInvestigation(Investigation investigation)
        {
            _applicationDbContext.Investigations.Add(investigation);
            _applicationDbContext.SaveChanges();
        }

        public void DeleteInvestigation(Investigation investigation)
        {
            _applicationDbContext.Investigations.Remove(investigation);
            _applicationDbContext.SaveChanges();
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _applicationDbContext.Investigations;
        }

        public Investigation GetInvestigatiosById(int id)
        {
            return _applicationDbContext.Investigations.FirstOrDefault(x => x.Id == id);
        }

        public Investigation GetInvestigationByReportId(Report report)
        {
            return _applicationDbContext.Investigations.Include(x=>x.Investigator).FirstOrDefault(x => x.AssociatedReport.Id == report.Id);
        }

        public void UpdateInvestigation(Investigation investigation)
        {
            var current = _applicationDbContext.Investigations.SingleOrDefault(x => x.Id == investigation.Id);

            if (current != null)
            {
                current.LastModified = DateTime.UtcNow;
                current.Description = investigation.Description;

                _applicationDbContext.Entry(current).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _applicationDbContext.SaveChanges();
            }
        }
    }
}
