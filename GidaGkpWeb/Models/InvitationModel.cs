using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class InvitationModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string SectorName { get; set; }
        public string ApplicationNumber { get; set; }
        public string PlotRange { get; set; }
        public string TotalPlot { get; set; }
        public string InterviewMode { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
    }
}