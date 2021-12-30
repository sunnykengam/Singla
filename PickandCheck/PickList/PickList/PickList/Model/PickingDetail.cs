using System;
using System.Collections.Generic;
using System.Text;

namespace PickList.Model
{
    public class PickingDetail
    {
        public string sno { get; set; }
        public string Itemname { get; set; }
        public string Invnum { get; set; }
        public string Pack { get; set; }
        public string Location { get; set; }
        public string Picked { get; set; }
        public int Itemid { get; set; }
        public string itemcode { get; set; }
        public int ITEMCATEGORY { get; set; }
        public DateTime Invdt { get; set; }
        public string CustCode { get; set; }
        public string BasketNo { get; set; }
        public string CustomerName { get; set; }
        public string Block { get; set; }
        public int Qty { get; set; }
        public string PhoneId { get; set; }
        public string checkername { get; set; }
        public string ChickerId { get; set; }
        public bool Ischecked { get; set; }
        public string PickedTime { get; set; }
        [SQLite.Ignore]
        public string rowcolor { get; set; }
    }

    public class UpdatePickList
    {
        public string Picked { get; set; }
        public string BasketNo { get; set; }
        public string Cust_Code { get; set; }
        public DateTime Inv_Date { get; set; }
        public string itemId { get; set; }
        public string PickedTime { get; set; }
        public string Invnum { get; set; }
    }

    public class CustomerDetail
    {
        public int Sno { get; set; }
        public string routeid { get; set; }
        public string route { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public DateTime invdt { get; set; }
        public int Invnum { get; set; }
        public int NoofLines { get; set; }
        public string pickername { get; set; }
        public string checkername { get; set; }
        public string phone { get; set; }
        public string Block { get; set; }
        public string Status { get; set; }
        public string PickingStatus{ get; set; }
        public int custid { get; set; }
        public int ChickerId { get; set; }
        //public string rowcolor { get; set; }
        
        //public bool IsDisable { get; set; }
    }
    public class PhoneInfo
    {
        public Nullable<double> PHONEID { get; set; }
        public string PHONENAME { get; set; }
        public Nullable<double> BLOCKID { get; set; }
        public string BLOCKNAME { get; set; }
        public string LoginStatus { get; set; }
    }
    public class CheckerInfo
    {
        public int CheckerId { get; set; }
        public string CheckerName { get; set; }
        public int TotalBlocks { get; set; }
        public int CompleatedBlocks { get; set; }
        public int PendingBlocks { get; set; }
    }
    public  class UnPickingList
    {
        public string Sno { get; set; }
        public string custcode { get; set; }
        public string CustomerName { get; set; }
        public DateTime Invdt { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<int> CustId { get; set; }
        public string Itemname { get; set; }
        public string Pack { get; set; }
        public string Location { get; set; }
        public string Invnum { get; set; }
        public Nullable<int> Itemid { get; set; }
        public Nullable<int> Batchid { get; set; }
        public string Batch { get; set; }
        public string expiry { get; set; }
        public Nullable<decimal> mrp { get; set; }
        public string itemcode { get; set; }
        public Nullable<int> routeid { get; set; }
        public string route { get; set; }
        public string Block { get; set; }
        public string checkername { get; set; }
        public string CheckerId { get; set; }
        public string Phoneid { get; set; }
        public string Status { get; set; }
        public bool Ischecked { get; set; }
    }

    public class CheckerAssign
    {
        public string InvDt { get; set; }
        public string custCode { get; set; }
        public string Block { get; set; }
        public string Invnum { get; set; }
        public int CheckerId { get; set; }
        public string CheckerName { get; set; }
    }
    public class UnPickList
    {
        public string Invdt { get; set; }
        public string custCode { get; set; }
        public string itemId { get; set; }
        public string Batchid { get; set; }

        public string  Invnum { get; set; }
    }

    public class UpdatePickedDetail
    {
        public string custCode { get; set; }
        public string InvDt { get; set; }
        public string block { get; set; }
        public string time { get; set; }
        public string Invnum { get; set; }
    }
   
}
