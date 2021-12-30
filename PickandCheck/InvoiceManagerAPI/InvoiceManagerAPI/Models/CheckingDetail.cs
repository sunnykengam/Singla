using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagerAPI.Models
{
    public class CheckingDetail
    {
        public Nullable<int> Id { get; set; }
        public string CustCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime Invdt { get; set; }
        public string Qty { get; set; }
        public int CustId { get; set; }
        public string Itemname { get; set; }
        public string Pack { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string NewQty { get; set; }
        public DateTime ChickedDate { get; set; }
        public int Invnum { get; set; }
        public int Itemid { get; set; }
        public string Itemcode { get; set; }
        public int Batchid { get; set; }
        public string Batch { get; set; }
        public string NewBatch { get; set; }
        public string expiry { get; set; }
        public decimal MRP { get; set; }
        public decimal Rate { get; set; }
        public string CheckerName { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string Isdelete { get; set; }
        public string Isqty { get; set; }
        public string Isbacth { get; set; }
        public string CheckerId { get; set; }
        public string IsChecked { get; set; }
        public string Checked { get; set; }
        public int IsStatus { get; set; }
        public int NewPsrlno { get; set; }
        public string InvType { get; set; }
        public string Routename { get; set; }
        public TimeSpan CheckedTime { get; set; }
        public decimal FQty { get; set; }
        public int NewFQty { get; set; }
    }
    public class InvoiceSummary
    {
        public DateTime Invdate { get; set; }
        public string checkername { get; set; }
        public string routename { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public int Totalines { get; set; }
        public int pickedlines { get; set; }
        public int checkedlines { get; set; }
        public int pendinglines { get; set; }
        public int batchchanges { get; set; }
        public int qtychangelines { get; set; }
        public int nillines { get; set; }
        public int Noofboxes { get; set; }
        public int Noofpackets { get; set; }
        public string InvType { get; set; }
        public string Invnum { get; set; }
    }
}