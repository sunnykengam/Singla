using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickingApi.Models
{
    
    public class PickingDetail
    {
        public string sno { get; set; }
        public string Itemname { get; set; }
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
        public Nullable<int> Qty { get; set; }
        public string PhoneId { get; set; }
        public string checkername { get; set; }
        public string ChickerId { get; set; }
        public bool Ischecked { get; set; }
        public string Invnum { get; set; }
    }
    public class UnPickList
    {
        public string Invdt { get; set; }
        public string custCode { get; set; }
        public string itemId { get; set; }
        public string Batchid { get; set; }
        public string Invnum { get; set; }
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

    public class CheckerAssign
    {
        public string InvDt { get; set; }
        public string custCode { get; set; }
        public string Block { get; set; }
        public string Invnum { get; set; }
        public int CheckerId { get; set; }
        public string CheckerName { get; set; }
    }

    public class UpdateTime
    {
        public string custCode { get; set; }
        public string InvDt { get; set; }
        public string block { get; set; }
        public string time { get; set; }
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
        public string PickingStatus { get; set; }
        public int custid { get; set; }
        public int ChickerId { get; set; }
        public string rowcolor { get; set; }

        public bool IsDisable { get; set; }
    }
}