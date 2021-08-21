using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class LookupModel
    {
        public int LookupId { get; set; }
        public string LookupType { get; set; }
        public string LookupName { get; set; }
        public Nullable<int> ParentLookupId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}