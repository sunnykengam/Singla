using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
   public class CustInvoiceDb
    {
        public string id { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string version { get; set; }
        public string deleted { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public string invnum { get; set; }
        public string invdate { get; set; }
        public string noofitems { get; set; }
        public string invamt { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string telephone { get; set; }
        public string mobile { get; set; }
        public string status { get; set; }
        public string DlvryStatus { get; set; }
        public string DlvryDate { get; set; }
        public string DlvryTime { get; set; }
        public int partnercode { get; set; }
        public int CreatedDate { get; set; }
    }
}
