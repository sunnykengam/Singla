using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagerAPI.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public string sno { get; set; }
        public string CustCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime Invdt { get; set; }
        public double Qty { get; set; }
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
        public double FQty { get; set; }
        public double NewFQty { get; set; }
        public string Scheme { get; set; }
        public string BatchQty { get; set; }
        public Nullable<System.TimeSpan> CheckedTime { get; set; }
        public List<PickerList> BatchList { set; get; }
    }
    public class PickerList
    {

        public string batch { get; set; }
        public int batchid { get; set; }
        public string PickerDisply { get; set; }
        public string Scheme { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }

    }
    public partial class PickedDetail
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

    public class NewBatchClass
    {
        public int itemid { get; set; }
        public string itemcode { get; set; }
        public DateTime Invdt { get; set; }
        public string itemname { get; set; }
        public string pack { get; set; }
        public string batch { get; set; }
        public double Rate { get; set; }
        public double MRP { get; set; }
        public string Scheme { get; set; }
        public string expiry { get; set; }
        public decimal qty { get; set; }
        public int batchid { get; set; }
    }
    public class UpdatePickedDetail
    {
        public string custCode { get; set; }
        public DateTime InvDt { get; set; }
        public string block { get; set; }
        public string Invnum { get; set; }
    }
    public class UpdateQty
    {
        public string custCode { get; set; }
        public string Invnum { get; set; }
        public DateTime Invdt { get; set; }
        public string itemId { get; set; }
        public string Batchid { get; set; }
        public string Checked { get; set; }
        public decimal NewQuenty { get; set; }
        public int Id { get; set; }
        public TimeSpan CheckedTime { get; set; }
        public string NewBatchid { get; set; }
        public string NewBatch { get; set; }
        public int NewFQty { get; set; }
        public string BatchQty { get; set; }

        public string ManualBatch { get; set; }
        
    }
    public class UpdateBatchStatus
    {
        public int Custid { get; set; }
        public DateTime Invdt { get; set; }
        public int itemId { get; set; }
        public int Batchid { get; set; }
        public decimal NewQuenty { get; set; }
        public string NewBatch { get; set; }
        public string OldBatch { get; set; }
        public int Invnum { get; set; }
    }

    public class UpdateTime
    {
        public string custCode { get; set; }
        public string InvDt { get; set; }
        public string block { get; set; }
        public string time { get; set; }
        public string Invnum { get; set; }
    }
}