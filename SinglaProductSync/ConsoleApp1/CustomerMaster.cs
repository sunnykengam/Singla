using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaApi.Models
{
    public class CustomerMaster
    {
        public string Id { get; set; }
        public int partnercode { get; set; }
        public float customerid { get; set; }
        public string customername { get; set; }
        public string customercode { get; set; }
        public string customeraddress { get; set; }
        public string dlnumber { get; set; }
        public string dlnumber1 { get; set; }

        public string landmark { get; set; }
        public string customeremail { get; set; }
        public string gstnumber { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public decimal creditlimit { get; set; }
        public DateTime DLExpiry { get; set; }
        public string city { get; set; }
        public int pincode { get; set; }
        public string customerstatus { get; set; }
        public decimal outstandingbal { get; set; }
        public int CreditLockDays { get; set; }
        public int salesmancode { get; set; }
        public int salesmanarea { get; set; }
        public int salesmanroute { get; set; }
        public string Contact { get; set; }
        public string State { get; set; }
        public int Active { get; set; }
        public string lockreason { get; set; }
        public bool Lock { get; set; }
        public string BranchCode { get; set; }
        public DateTime syncdatetime { get; set; }

        public string syncdtime { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}