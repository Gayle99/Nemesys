using System;
using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class DetailsViewModel
    {
        public Report Report { get; set; }
        public Investigation Investigation { get; set; }
        public int Upvotes { get; set; }
    }
}
