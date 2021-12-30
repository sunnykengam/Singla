using System;
using System.Collections.Generic;
using System.Text;

namespace Checking.Models
{
   public class CheckingDetail
    {
        public int Id { get; set; }
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
        public string batch { get; set; }
        public string NewBatch { get; set; }
        public string expiry { get; set; }
        public decimal mrp { get; set; }
        public decimal Rate  { get; set; }
        public string CheckerName { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public bool Isdelete { get; set; }
        public string Isqty { get; set; }
        public string Isbacth { get; set; }
        public string CheckerId { get; set; }
        public bool IsChecked { get; set; }
        public string Checked { get; set; }
        public int IsStatus { get; set; }
        public int NewPsrlno { get; set; }
        public int OldPsrlno { get; set; }
        public string InvType { get; set; }
        public string Routename { get; set; }
        public string CheckedTime { get; set; }
        public double FQty { get; set; }
        public double NewFQty { get; set; }
    }
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
