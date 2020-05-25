using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public interface IPromotionRepository
    {
        IEnumerable<Promote> GetAllPromotionRequests();
        Promote GetUserById(string userId);
        Promote GetRequestById(int reqId);
        public void PromotionRequest(Promote role);
        public void DeleteRequest(Promote request);
    }
}