using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SinglaMedicos.Azure
{
    public class Orders
    {
        [JsonProperty(PropertyName = "partnercode")]
        public string partnercode { get; set; }

        [JsonProperty(PropertyName = "customer_id")]
        public string customer_id { get; set; }

        [JsonProperty(PropertyName = "customername")]
        public string customername { get; set; }

        [JsonProperty(PropertyName = "SequenceOrder_id")]
        public int SequenceOrder_id { get; set; }

        [JsonProperty(PropertyName = "order_id")]
        public string order_id { get; set; }

        [JsonProperty(PropertyName = "order_date")]
        public DateTime order_date { get; set; }

        [JsonProperty(PropertyName = "order_status")]
        public string order_status { get; set; }

        [JsonProperty(PropertyName = "Sman")]
        public int Sman { get; set; }

        [JsonProperty(PropertyName = "Area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "NoOfItems")]
        public int NoOfItems { get; set; }

        [JsonProperty(PropertyName = "Amt")]
        public double Amt { get; set; }

        [JsonProperty(PropertyName = "Invoice_No")]
        public string Invoice_No { get; set; }

        [JsonProperty(PropertyName = "Invoice_date")]
        public DateTime Invoice_date { get; set; }

        [JsonProperty(PropertyName = "NetAmount")]
        public double NetAmount { get; set; }

        [JsonProperty(PropertyName = "TaxAmount")]
        public double TaxAmount { get; set; }

        [JsonProperty(PropertyName = "Discountamount")]
        public double Discountamount { get; set; }

        [JsonProperty(PropertyName = "Remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "delivery_date")]
        public DateTime delivery_date { get; set; }

        [JsonProperty(PropertyName = "route")]
        public string route { get; set; }

        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "UpdateStatus")]
        public string UpdateStatus { get; set; }

        [JsonProperty(PropertyName = "ORDERFROM")]
        public string ORDERFROM { get; set; }

        [JsonProperty(PropertyName = "appversion")]
        public string appversion { get; set; }

        [JsonProperty(PropertyName = "ordtaketime")]
        public string ordtaketime { get; set; }

        [JsonProperty(PropertyName = "ordupldtime")]
        public string ordupldtime { get; set; }

        [JsonProperty(PropertyName = "OrderGuid")]
        public string OrderGuid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

    }
}
