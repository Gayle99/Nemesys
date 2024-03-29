﻿using Microsoft.AspNetCore.Identity;
using Nemesys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class ReportUpvotedRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ReportUpvotedRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool CheckIfUpvoted(Report report, IdentityUser user)
        {   
            var upvote = _applicationDbContext.ReportUpvoted.FirstOrDefault(x => x.Report.Id == report.Id && x.User.Id == user.Id);
            if(upvote == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void AddUpvote(ReportUpvoted reportUpvoted)
        {
            _applicationDbContext.ReportUpvoted.Add(reportUpvoted);
            _applicationDbContext.SaveChanges();
        }

        public void RemoveUpvote(ReportUpvoted reportUpvoted)
        {
            _applicationDbContext.ReportUpvoted.Remove(reportUpvoted);
            _applicationDbContext.SaveChanges();
        }

        public ReportUpvoted GetUpvote(Report report, IdentityUser user)
        {
            var upvote = _applicationDbContext.ReportUpvoted.FirstOrDefault(x => x.Report.Id == report.Id && x.User.Id == user.Id);
            if (upvote != null)
            {
                return upvote;
            }
            else
            {
                return null;
            }
        }

        public int TotalUpvotes(Report report)
        {
            ReportUpvoted[] upvotes = _applicationDbContext.ReportUpvoted.Where(x => x.Report.Id == report.Id).ToArray();
            if(upvotes != null)
            {
                return upvotes.Count();
            }
            else
            {
                return 0;
            }
        }
    }
}
