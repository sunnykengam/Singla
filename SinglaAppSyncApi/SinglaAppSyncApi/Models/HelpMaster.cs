using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaAppSyncApi.Models
{
    public class HelpMaster
    {
    }
    public class ItemMaster
    {
        public int partnercode { get; set; }
        public int itemid { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string manufacturer { get; set; }
        public string manufacturergroup { get; set; }
        public string packsize { get; set; }
        public float ptr { get; set; }
        public float mrp { get; set; }
        public float boxsize { get; set; }
        public float stock { get; set; }
        public string Halfscheme { get; set; }
        public string frmstockcolor { get; set; }
        public string Scheme { get; set; }
        public DateTime creationdate { get; set; }
        public string substitute { get; set; }
        public string itemnamesearch { get; set; }
    }

    public class CustomerMaster
    {
        public string id { get; set; }
        public int partnercode { get; set; }
        public int customerid { get; set; }
        public string customername { get; set; }
        public string customercode { get; set; }
        public string customeraddress { get; set; }
        public string dlnumber { get; set; }
        public string customeremail { get; set; }
        public string gstnumber { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public decimal creditlimit { get; set; }
        public DateTime DLExpiry { get; set; }
        public string city { get; set; }
        public int pincode { get; set; }
        public string customerstatus { get; set; }
        public decimal outstandingbal { get; set; }
        public int CreditLockDays { get; set; }
        public int salesmancode { get; set; }
        public int salesmanarea { get; set; }
        public int salesmanroute { get; set; }
        public int locked { get; set; }
        public string lockreason { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
    public class SalesmanMaster
    {
        public string id { get; set; }
        public int partnercode { get; set; }
        public string username { get; set; }
        public string userpassword { get; set; }
        public int salesmanid { get; set; }
        public string salesmancode { get; set; }
        public string salesmanname { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public float salesmantarget { get; set; }
        public string salesmancommision { get; set; }
    }
    public class Orders
    {

        public string customername { get; set; }
        public string partnercode { get; set; }
        public string order_id { get; set; }
        public string Temporder_id { get; set; }
        public int SequenceOrder_id { get; set; }
        public int srno { get; set; }
        public string customer_id { get; set; }
        public string order_status { get; set; }
        public string order_date { get; set; }
        public DateTime Invoice_date { get; set; }
        public DateTime delivery_date { get; set; }
        public double Amt { get; set; }
        public double NetAmount { get; set; }
        public double TaxAmount { get; set; }
        public double Discountamount { get; set; }
        public int Sman { get; set; }
        public string Area { get; set; }
        public string route { get; set; }
        public int NoOfItems { get; set; }
        public string id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateStatus { get; set; }
        public string Invoice_No { get; set; }
        public string ORDERFROM { get; set; }
        public string ordupldtime { get; set; }
        public string ordtaketime { get; set; }
        public string OrderGuid { get; set; }

        public List<orderitems> orderItemList = new List<orderitems>();
    }
    public class orderitems
    {
        public string customer_id { get; set; }
        public int itemcode { get; set; }
        public string itemaltercode { get; set; }
        public string itemname { get; set; }
        public string pack { get; set; }
        public string company { get; set; }
        public string orderqty { get; set; }
        public string orderfqty { get; set; }
        public string order_id { get; set; }
        public string order_date { get; set; }
        public string QtyRecd { get; set; }
        public string FQtyRecd { get; set; }
        public string Tag { get; set; }
        public string Invnum { get; set; }
        public Nullable<DateTime> Invoice_date { get; set; }
        public string PorderDis { get; set; }
        public string Rate { get; set; }
        public string MRP { get; set; }
        public string Remarks { get; set; }
        public string scm1 { get; set; }
        public string scm2 { get; set; }
        public float OrderAmount { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class Orderstatus
    {
        public string order_status { get; set; }
        public string Invoice_date { get; set; }
        public string Invoice_No { get; set; }
        public string UpdateStatus { get; set; }
        public string order_id { get; set; }
    }
}