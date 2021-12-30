using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
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

        public string Invoice_date { get; set; }

        public string PorderDis { get; set; }

        public float Rate { get; set; }

        public string MRP { get; set; }

        public string Remarks { get; set; }

        public string scm1 { get; set; }

        public string scm2 { get; set; }

        public float OrderAmount { get; set; }

        public int srlno { get; set; }

        public DateTime CreatedDate { get; set; }

        public int partnercode { get; set; }

        public string OrderGuid { get; set; }

        public string AzureStatus { get; set; }

        public int Sman { get; set; }

        public string INVTYPE { get; set; }
    }
}
