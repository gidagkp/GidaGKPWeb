using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class PlotMasterModel
    {
        public int PlotId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> SchemeType { get; set; }
        public Nullable<int> SchemeName { get; set; }
        public Nullable<int> SectorName { get; set; }
        public Nullable<int> PlotRange { get; set; }
        public string PlotArea { get; set; }
        public string PlotNumber { get; set; }
        public string PlotRate { get; set; }
        public Nullable<bool> PlotSideCorner { get; set; }
        public Nullable<bool> PlotSideWideRoad { get; set; }
        public Nullable<bool> PlotSideParkFacing { get; set; }
        public string RateForPlotSide { get; set; }
        public string PlotCategory { get; set; }
        public string PercentageRate { get; set; }
        public string PlotCost { get; set; }
        public string ExtraCharge { get; set; }
        public string GrandTotalCost { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}