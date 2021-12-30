using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaOrderManagementApi.Models
{
    public class HelpMaster
    {
        public class updateOrders
        {
            public string OrderGuid { get; set; }
            public string order_id { get; set; }
            public string ordupldtime { get; set; }
        }
        public class Orders
        {
            public int srno { get; set; }
            public string customername { get; set; }
            public string partnercode { get; set; }
            public string order_id { get; set; }
            public string Temporder_id { get; set; }
            public int SequenceOrder_id { get; set; }
            public string customercode { get; set; }
            public string order_status { get; set; }
            public DateTime order_date { get; set; }
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
            public string appversion { get; set; }
            public string ordtaketime { get; set; }
            public string ordupldtime { get; set; }
            public string OrderGuid { get; set; }
            public string customer_id { get; set; }
            public string INVTYPE { get; set; }
            //public List<ItemMaster> itemlist { get; set; } = new List<ItemMaster>();
            public List<orderitems> orderItemList = new List<orderitems>();
        }
        public class ItemMaster
        {
            public string id { get; set; }
            public int partnercode { get; set; }
            public int itemid { get; set; }
            public string itemcode { get; set; }
            public string itemname { get; set; }
            public string manufacturer { get; set; }
            public string manufacturergroup { get; set; }
            public string packsize { get; set; }
            public float pts { get; set; }
            public float ptr { get; set; }
            public float mrp { get; set; }
            public string itemstatus { get; set; }
            public float boxsize { get; set; }
            public float stock { get; set; }
            public string Halfscheme { get; set; }
            public string frmstockcolor { get; set; }
            public string Scheme { get; set; }
            public DateTime creationdate { get; set; }
            public string substitute { get; set; }
            public string itemname1 { get; set; }
            public string itemname2 { get; set; }
            public string itemname3 { get; set; }
            public string itemname4 { get; set; }
            public string itemname5 { get; set; }
            public string itemname6 { get; set; }
            public string itemname7 { get; set; }
            public string itemname8 { get; set; }
            public string itemnamesearch { get; set; }
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
            public float Rate { get; set; }
            public string MRP { get; set; }
            public string Remarks { get; set; }
            public string scm1 { get; set; }
            public string scm2 { get; set; }
            public float OrderAmount { get; set; }
            public DateTime CreatedDate { get; set; }
            public string partnercode { get; set; }
            public string OrderGuid { get; set; }
            public string INVTYPE { get; set; }
        }
    }
}