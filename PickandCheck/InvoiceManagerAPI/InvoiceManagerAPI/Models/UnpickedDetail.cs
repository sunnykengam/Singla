using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagerAPI.Models
{
    public class UnpickedDetail
    {
        public string CustCode { get; set; }
      public string CustomerName { get; set; }
      public DateTime Invdt { get; set; }
      public decimal Qty { get; set; }
        public int CustId { get; set; }
        public string Itemname { get; set; }
        public string Pack { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public decimal NewQty { get; set; }
        public string Invnum { get; set; }
        public int Itemid { get; set; }
        public int Batchid { get; set; }
        public string Batch { get; set; }
        public string expiry { get; set; }
        public string mrp { get; set; }
        public string itemcode { get; set; }
        public int routeid { get; set; }
        public string route { get; set; }
        public string Block { get; set; }
        public string checkername { get; set; }
        public string CheckerId { get; set; }
        public string Phoneid { get; set; }
        public string PickerName { get; set; }
    }
}