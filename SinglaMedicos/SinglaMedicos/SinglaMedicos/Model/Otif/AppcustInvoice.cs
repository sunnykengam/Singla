using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    
    [SQLite.Table("AppcustInvoice"), ConditionType("custInvoice", "PrimaryCondition")]
    public class AppcustInvoice
    {
        [SQLite.Column("id"), SQLite.PrimaryKey]
        public string id { get; set; }

        [SQLite.Column("custcode")]
        public string custcode { get; set; }

        [SQLite.Column("custname")]
        public string custname { get; set; }

        [SQLite.Column("invnum")]
        public string invnum { get; set; }

        [SQLite.Column("invdate")]
        public string invdate { get; set; }
       
        [SQLite.Column("noofitems")]
        public int noofitems { get; set; }
        
        [SQLite.Column("invamt")]
        public decimal invamt { get; set; }
        
        [SQLite.Column("address")]
        public string address { get; set; }
        
        [SQLite.Column("address1")]
        public string address1 { get; set; }
       
        [SQLite.Column("address2")]
        public string address2 { get; set; }
        
        [SQLite.Column("address3")]
        public string address3 { get; set; }
        
        [SQLite.Column("address4")]
        public string address4 { get; set; }
        
        [SQLite.Column("telephone")]
        public string telephone { get; set; }
        
        [SQLite.Column("mobile")]
        public string mobile { get; set; }
       
        [SQLite.Column("Status")]
        public string Status { get; set; }
       
        [SQLite.Column("DlvryStatus")]
        public string DlvryStatus { get; set; }
       
        [SQLite.Column("DlvryDate")]
        public string DlvryDate { get; set; }
        
        [SQLite.Column("DlvryTime")]
        public string DlvryTime { get; set; }
       
        [SQLite.Column("partnercode")]
        public int partnercode { get; set; }
        
        [SQLite.Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [SQLite.Column("RouteId")]
        public int RouteId { get; set; }

        [SQLite.Column("RouteName")]
        public string RouteName { get; set; }
    }
    public class CustomerDelivery
    {
        public Nullable<int> Custcode { get; set; }
        public string Custname { get; set; }
        public string CustAltcode { get; set; }
        public Nullable<int> NoofBoxes { get; set; }
        public Nullable<int> Noofpacks { get; set; }
        public Nullable<System.DateTime> DlvryDate { get; set; }
        public Nullable<System.TimeSpan> DlvryTime { get; set; }
        public string InvoiceDeltails { get; set; }
        public string InvoiceDate { get; set; }
        public byte[] InvoiceImage { get; set; }
        public string DelviryCode { get; set; }
        public string CreatedDate { get; set; }
        public string partnercode { get; set; }

    }
}
