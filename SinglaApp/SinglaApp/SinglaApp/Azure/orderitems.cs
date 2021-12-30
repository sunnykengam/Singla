using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Azure
{
    public class orderitems
    {
        [JsonProperty(PropertyName = "customer_id")]
        public string customer_id { get; set; }

        [JsonProperty(PropertyName = "itemcode")]
        public int itemcode { get; set; }

        [JsonProperty(PropertyName = "itemaltercode")]
        public string itemaltercode { get; set; }

        [JsonProperty(PropertyName = "itemname")]
        public string itemname { get; set; }

        [JsonProperty(PropertyName = "pack")]
        public string pack { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string company { get; set; }

        [JsonProperty(PropertyName = "orderqty")]
        public string orderqty { get; set; }

        [JsonProperty(PropertyName = "orderfqty")]
        public string orderfqty { get; set; }

        [JsonProperty(PropertyName = "order_id")]
        public string order_id { get; set; }

        [JsonProperty(PropertyName = "order_date")]
        public string order_date { get; set; }

        [JsonProperty(PropertyName = "QtyRecd")]
        public string QtyRecd { get; set; }

        [JsonProperty(PropertyName = "FQtyRecd")]
        public string FQtyRecd { get; set; }

        [JsonProperty(PropertyName = "Tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "Invnum")]
        public string Invnum { get; set; }

        [JsonProperty(PropertyName = "Invoice_date")]
        public string Invoice_date { get; set; }

        [JsonProperty(PropertyName = "PorderDis")]
        public string PorderDis { get; set; }

        [JsonProperty(PropertyName = "Rate")]
        public float Rate { get; set; }

        [JsonProperty(PropertyName = "MRP")]
        public string MRP { get; set; }

        [JsonProperty(PropertyName = "Remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "scm1")]
        public string scm1 { get; set; }

        [JsonProperty(PropertyName = "scm2")]
        public string scm2 { get; set; }

        [JsonProperty(PropertyName = "OrderAmount")]
        public float OrderAmount { get; set; }

        [JsonProperty(PropertyName = "srlno")]
        public string srlno { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

    }
}
