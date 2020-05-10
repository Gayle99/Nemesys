using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public interface IInvestigationRepository
    {
        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigatiosById(int id);
        void UpdateInvestigation(Investigation investigation);
        void CreateInvestigation(Investigation investigation);
        void DeleteInvestigation(Investigation investigation);
    }
}
