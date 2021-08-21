using GidaGkpWeb.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GidaGkpWeb.Controllers
{
    public class MastersController : Controller
    {
        [HttpPost]
        public JsonResult GetLookupDetail(int? lookupTypeId, string lookupType, bool active = true)
        {
            MasterDetails _details = new MasterDetails();
            if (lookupTypeId == 0)
            {
                lookupTypeId = null;
            }
            return Json(_details.GetLookupDetail(lookupTypeId, lookupType, active), JsonRequestBehavior.AllowGet);
        }
    }
}