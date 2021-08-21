using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class AdminNoticeModel
    {
        public int Id { get; set; }
        public Nullable<int> NoticeTypeId { get; set; }
        public string Notice_Type { get; set; }
        public string Notice_title { get; set; }
        public Nullable<System.DateTime> Notice_Date { get; set; }
        public int? Department { get; set; }
        public string DepartmentName { get; set; }
        public byte[] NoticeDocumentFile { get; set; }
        public string NoticeDocumentFileBase64 { get; set; }
        public string NoticeDocumentName { get; set; }
        public string NoticeDocumentFileType { get; set; }
        public Nullable<bool> NoticeNewTag { get; set; }
        public int? CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}