﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nemesys.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();

            var model = new ReportsListViewModel();
        }
    }
}