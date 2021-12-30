using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaApi.Models
{

    public class login
    {
        public string username { get; set; }
        public string userpassword { get; set; }
    }
    public class OrderDetails
    {
        public string order_id { get; set; }
        public string customer_id { get; set; }
        public string partnercode { get; set; }
        public int NoOfItem { get; set; }
        public DateTime order_date { get; set; }
        public double Amt { get; set; }
        public string IsChallan { get; set; }
        public string ORDERFROM { get; set; }

        public List<OrderItems> orderItemList = new List<OrderItems>();
    }
    public class UpdatedOrderStatus
    {
        public string order_id { get; set; }
        public string order_status { get; set; }
        public string UpdateStatus { get; set; }
        public string delivery_date { get; set; }
    }
    public class OrderItems
    {
        public string itemname { get; set; }
        public int itemcode { get; set; }
        public string orderqty { get; set; }

    }
}

