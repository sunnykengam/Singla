using System;
using System.Collections.Generic;
using System.Text;

namespace Checking.Models
{
   public class InvoiceItem
    {
        public string sno { get; set; }
        public string Batch { get; set; }
        public string DisplayBatch { get; set; }
        public int Batchid { get; set; }
        public string Block { get; set; }
        public string CheckerId { get; set; }
        public string CheckerName { get; set; }
        public bool Chicked { get; set; }
        public string CustCode { get; set; }
        public int CustId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Invdt { get; set; }
        public string Invnum { get; set; }
        public bool Ischecked { get; set; }
        public bool Isdelete { get; set; }
        public bool un_pick { get; set; }
        public string ItemStatus { get; set; }
        public int Itemid { get; set; }
        public string Itemname { get; set; }
        public string Location { get; set; }
        public string NewQty { get; set; }
        public string dummyQty { get; set; }
        public string Pack { get; set; }
        public string Picked { get; set; }
        public double Qty { get; set; }

        public string DisPlayQty { get; set; }
        public string Status { get; set; }
        public string expiry { get; set; }
        public string itemcode { get; set; }
        public string itemcolor { get; set; }
        public decimal mrp { get; set; }

        public decimal Rate { get; set; }
        public string pickerTitle { get; set; }
        public string rowPickercolor { get; set; }
        public string rowUpdatecolor { get; set; }
        public string rowcolor { get; set; }
        public string BasketNo { get; set; }
        public string isqty { get; set; }
        public string isbatch { get; set; }
        public string isdelete { get; set; }
        public string isUnpick { get; set; }
        public string route { get; set; }
        public string InvType { get; set; }
        public string CheckedTime { get; set; }

        public string NewBatchid { get; set; }
        public string NewBatch { get; set; }

        public string ManualBatch { get; set; }

        public string BatchQty { get; set; }
        public double FQty { get; set; }
        public double NewFQty { get; set; }
        public string Scheme { get; set; }
        [SQLite.Ignore]
        public string rowqtyUpdatecolor { get; set; }
        [SQLite.Ignore]
        public bool Isqtyenable { get; set; }
        [SQLite.Ignore]
        public int Id { get; set; }
        [SQLite.Ignore]
        public List<PickerList> BatchList { set; get; }
    }
    public class SummaryDetail
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
    public  class InvoiceStatus
    {
        public int TotalBlocks { get; set; }
        public int pickedlines { get; set; }
        public int checkedlines { get; set; }
    }
}
