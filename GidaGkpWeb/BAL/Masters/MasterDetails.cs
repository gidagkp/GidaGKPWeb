using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using GidaGkpWeb.Global;
using System.Data.Entity;
using GidaGkpWeb.Models;

namespace GidaGkpWeb.BAL
{
    public class MasterDetails
    {
        GidaGKPEntities _db = null;

        public IEnumerable<LookupModel> GetLookupDetail(int? parentLookupId, string lookupTye, bool active = true)
        {
            _db = new GidaGKPEntities();
            return (from lookup in _db.Lookups.Where(x => (x.ParentLookupId == parentLookupId) && x.LookupType == lookupTye && ((active == true && x.IsActive == true) || active == false))
                    select new LookupModel
                    {
                        LookupId = lookup.LookupId,
                        LookupName = lookup.LookupName,
                        LookupType = lookup.LookupType,
                        ParentLookupId = lookup.ParentLookupId,
                    }).OrderBy(x => x.LookupName).ToList();

        }

    }
}