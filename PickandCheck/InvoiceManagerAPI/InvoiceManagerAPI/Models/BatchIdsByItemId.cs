using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagerAPI.Models
{
    public class BatchIdsByItemId
    {
        public int itemid { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string pack { get; set; }
        public string batch { get; set; }
        public string expiry { get; set; }
        public decimal qty { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }
        public string Invdt { get; set; }
        public int batchid { get; set; }
        public string Scheme { get; set; }
    }
}