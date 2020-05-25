using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nemesys.Data;

namespace Nemesys.Models
{
    public class PromoteRepository : IPromotionRepository
    {
        public readonly ApplicationDbContext _appDbContext;

        public void PromotionRequest(Promote role)
        {
            _appDbContext.PromoteRequests.Add(role);
            _appDbContext.SaveChanges();
        }

        public PromoteRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Promote> GetAllPromotionRequests()
        {
            return _appDbContext.PromoteRequests.Include(x => x.User);
        }

        public Promote GetUserById(string userId)
        {
            return _appDbContext.PromoteRequests.Include(x => x.User).FirstOrDefault(p => p.User.Id == userId);
        }

        public Promote GetRequestById(int reqId)
        {
            return _appDbContext.PromoteRequests.FirstOrDefault(p => p.Id == reqId);
        }

        public void DeleteRequest(Promote request)
        {
            _appDbContext.PromoteRequests.Remove(request);
            _appDbContext.SaveChanges();
        }


    }
}