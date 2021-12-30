using System;
using System.Collections.Generic;
using System.Text;

namespace Checking.Models
{
   public class UserLogin
    {
        public string checkercode { get; set; }
        public string checkername { get; set; }
        
    }
    public class ItemList
    {
        public int itemid { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string pack { get; set; }
        public string batch { get; set; }
        public string expiry { get; set; }
        public decimal qty { get; set; }
        public double MRP { get; set; }
        public int batchid { get; set; }
    }
    public class PickerList
    {
        public string CustCode { get; set; }
        public string Block { get; set; }
        public int Itemid { get; set; }
        public string batch { get; set; }
        public string PickerDisply { get; set; }
        public int batchid { get; set; }
        public string Scheme { get; set; }
        public string Batchentrytext { get; set; }
        public bool IsChecked { get; set; }
        public double MRP { get; set; }
        public double Rate { get; set; }
        public bool BatchIsFocus { get; set; }
        public bool BatchentryIsVisible { get; set; }

        public bool BatchStatus { get; set; }

        public string WhichBatch { get; set; }
    }
    
    public class PickedDetail
    {
        public string sno { get; set; }
        public int checkerid { get; set; }
        public string checkername { get; set; }
        public int custid { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public int routeid { get; set; }
        public string route { get; set; }
        public string Block { get; set; }
        public string Picked { get; set; }
        public string Checked { get; set; }
        public string pickername { get; set; }
        public string basketNo { get; set; }
        public string phone { get; set; }
        public bool IsDisable { get; set; }
        public string rowcolor { get; set; }
        public string Invnum { get; set; }

    }
    public class UpdatePickedDetail
    {
        public string custCode { get; set; }
        public string InvDt { get; set; }
        public string block { get; set; }
        public string time { get; set; }
        public string Invnum { get; set; }
    }
    public class UpdateQty
    {
        public string custCode { get; set; }
        public string Invnum { get; set; }
        public DateTime Invdt { get; set; }
        public string itemId { get; set; }
        public string Batchid { get; set; }
        public decimal NewQuenty { get; set; }
        public int Id { get; set; }
        public string CheckedTime { get; set; }
        public string Checked { get; set; }
        public string NewBatch { get; set; }
        public string BatchQty { get; set; }
        public string ManualBatch { get; set; }
        
        public double NewFQty { get; set; }
        public string NewBatchid { get; set; }
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
}
