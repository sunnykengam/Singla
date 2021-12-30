using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    public class Orders
    {
        [SQLite.Column("srno"), SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int srno { get; set; }

        public string partnercode { get; set; }
        public string customer_id { get; set; }
        public string customername { get; set; }
        public int SequenceOrder_id { get; set; }
        public string order_id { get; set; }
        public string order_date { get; set; }
        public string order_status { get; set; }
        public int Sman { get; set; }
        public string Area { get; set; }
        public int NoOfItems { get; set; }
        public double Amt { get; set; }
        public double NetAmount { get; set; }
        public double TaxAmount { get; set; }
        public double Discountamount { get; set; }
        public string Invoice_No { get; set; }
        public DateTime Invoice_date { get; set; }
        public string Remarks { get; set; }
        public DateTime delivery_date { get; set; }
        public string route { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateStatus { get; set; }
        public string ORDERFROM { get; set; }
        public string appversion { get; set; }
        public string ordtaketime { get; set; }
        public string ordupldtime { get; set; }
        public string OrderGuid { get; set; }
        public string Temporder_id { get; set; }
        public string AzureStatus { get; set; }
        public string INVTYPE { get; set; }

        [SQLite.Ignore]
        public bool isenable { get; set; }

        [SQLite.Ignore]
        public bool editisvisible { get; set; }

        [SQLite.Ignore]
        public bool saveisvisible { get; set; }
        [SQLite.Ignore]
        public bool Saveisenable { get; set; }

        [SQLite.Ignore]
        public bool editisenable { get; set; }

        [SQLite.Ignore]
        public string bgmcolor { get; set; }

        [SQLite.Ignore]
        public bool IsChecked { get; set; }

        [SQLite.Ignore]
        public bool ChallanaChecked { get; set; }

        [SQLite.Ignore]
        public bool InvoiceChecked { get; set; }

        [SQLite.Ignore]
        public string frmitemcolor { get; set; }

        [SQLite.Ignore]
        public string totalamount { get; set; }

        [SQLite.Ignore]
        public string compname { get; set; }

        [SQLite.Ignore]
        public bool localtoazurevisible { get; set; }

        [SQLite.Ignore]
        public List<ItemMaster> itemlist { get; set; } = new List<ItemMaster>();
        [SQLite.Ignore]
        public List<orderitems> orderitemlist { get; set; } = new List<orderitems>();
        [SQLite.Ignore]
        public List<CustomerMaster> pickcustomer { set; get; }
    }
    public class updateOrders
    {
        public string OrderGuid { get; set; }
        public string order_id { get; set; }
        public string ordupldtime { get; set; }
    }
    public class OrdersErrorLog
    {
        [SQLite.Column("partnercode")]
        public string partnercode { get; set; }

        [SQLite.Column("order_date")]
        public string order_date { get; set; }

        [SQLite.Column("Sman")]
        public int Sman { get; set; }

        [SQLite.Column("OrderJson")]
        public string OrderJson { get; set; }

        [SQLite.Column("ErrorMessage")]
        public string ErrorMessage { get; set; }
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
