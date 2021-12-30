using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckingApi.Models
{
    public partial class UserMaster
    {
        public int checkerid { get; set; }
        public string checkername { get; set; }
    }
    public partial class CheckingDetail
    {
        public string sno { get; set; }
        public int checkerid { get; set; }
        public string checkername { get; set; }
        public int custid { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public string routeid { get; set; }
        public string route { get; set; }
        public string Block { get; set; }
        public string Picked { get; set; }
        public string Checked { get; set; }
        public string pickername { get; set; }
        public string basketNo { get; set; }
        public string phone { get; set; }
        public bool IsDisable { get; set; }
        public string Invnum { get; set; }
    }

    public class InvoiceDetail
    {
        public int Id { get; set; }
        public string sno { get; set; }
        public string CustCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime Invdt { get; set; }
        public int Qty { get; set; }
        public int CustId { get; set; }
        public string Itemname { get; set; }
        public string Pack { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public int NewQty { get; set; }
        public string Picked { get; set; }
        public string Invnum { get; set; }
        public int Itemid { get; set; }
        public int Batchid { get; set; }
        public string Batch { get; set; }
        public string expiry { get; set; }
        public decimal mrp { get; set; }
        public decimal Rate { get; set; }
        public bool Chicked { get; set; }
        public string itemcode { get; set; }
        public bool IsEnabled { get; set; }
        public string BasketNo { get; set; }
        public string isqty { get; set; }
        public string isbatch { get; set; }
        public string isdelete { get; set; }
        public string isUnpick { get; set; }
        public string route { get; set; }
        public string InvType { get; set; }
        public string NewBatchid { get; set; }
        public string NewBatch { get; set; }
        public string ManualBatch { get; set; }
        public int FQty { get; set; }
        public int NewFQty { get; set; }
        public string Scheme { get; set; }
        public string BatchQty { get; set; }
        public string CheckedTime { get; set; }
        public List<PickerList> BatchList { set; get; }
    }
    public class PickerList
    {
        public int Itemid { get; set; }
        public string itemcode { get; set; }
        public string Itemname { get; set; }
        public string pack { get; set; }
        public string batch { get; set; }
        public string expiry { get; set; }
        public int qty { get; set; }
        public int batchid { get; set; }
        public string PickerDisply { get; set; }
        public string Scheme { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }

    }

    public class InvoiceSummary
    {
        public DateTime Invdate { get; set; }
        public string Checkername { get; set; }
        public string routename { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public int Totalines { get; set; }
        public string Invnum { get; set; }
        public string Invtype { get; set; }
        public int pickedlines { get; set; }
        public int batchchanges { get; set; }
        public int qtychangelines { get; set; }
        public int nillines { get; set; }
    }
}