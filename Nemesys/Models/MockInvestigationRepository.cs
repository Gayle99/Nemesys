using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class MockInvestigationRepository : IInvestigationRepository
    {
        private List<Investigation> _InvestigationList;

        public MockInvestigationRepository()
        {
            if (_InvestigationList == null)
            {
                InitializeInvestigation();
            }
        }

        private void InitializeInvestigation()
        {
            _InvestigationList = new List<Investigation>
            {
                new Investigation()
                {
                    Id = 1,
                    Description = "Description of 1",
                    DateOfAction = DateTime.UtcNow,
                    InvestigatorEmail = "a@b.c",
                    InvestigatorDetails = "Investigation of 1"

                },
                new Investigation()
                {
                    Id = 2,
                    Description = "Description of 2",
                    DateOfAction = DateTime.UtcNow,
                    InvestigatorEmail = "a@bb.c",
                    InvestigatorDetails = "Investigation of 2"

                },
                new Investigation()
                {
                    Id = 3,
                    Description = "Description of 3",
                    DateOfAction = DateTime.UtcNow,
                    InvestigatorEmail = "a@bbb.c",
                    InvestigatorDetails = "Investigation of 3"

                }


            };
        }
        public void CreateInvestigation(Investigation investigation)
        {
            investigation.Id = _InvestigationList.Count + 1;
            _InvestigationList.Add(investigation);
        }

        public void DeleteInvestigation(Investigation investigation)
        {
            throw new NotImplementedException();
        }

        public void EditInvestigation(Investigation investigation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _InvestigationList;
        }

        public Investigation GetInvestigatiosById(int id)
        {
            return _InvestigationList.FirstOrDefault(b => b.Id == id);
        }

        public void UpdateInvestigation(Investigation investigation)
        {
            throw new NotImplementedException();
        }
    }
}
