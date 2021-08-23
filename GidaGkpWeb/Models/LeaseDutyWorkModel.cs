using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GidaGkpWeb.Models
{
    public class LeaseDutyWorkModel
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int AllotmentId { get; set; }
        public Decimal AllotmentMoneyPaid { get; set; }
        public string BankGauranteeChallanNo { get; set; }
        public decimal PurchasedStampValue { get; set; }
        public decimal StampValueForBankGaurantee { get; set; }  
        public Nullable<System.DateTime> BankGauranteeDate { get; set; }
        public string EntityNameBehalfOfApplicant { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}