using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class GidaUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserRoleId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string UserRole { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Password { get; set; }
    }

    public class UserPermissionModel
    {
        public UserPermissionModel()
        {
            PermissionTypeIdList = new List<int>();
        }
        public int PageId { get; set; }
        public string PageName { get; set; }
        public List<int> PermissionTypeIdList { get; set; }
    }

    public class UserPermission
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PermissionName { get; set; }

    }
}