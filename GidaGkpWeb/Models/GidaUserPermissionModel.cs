using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class GidaUserPermissionModel
    {
        public GidaUserPermissionModel()
        {
            PermissionModel = new List<UserPermissionModel>();
        }
        public GidaUserModel UserModel { get; set; }

        public List<UserPermissionModel> PermissionModel { get; set; }
    }
}