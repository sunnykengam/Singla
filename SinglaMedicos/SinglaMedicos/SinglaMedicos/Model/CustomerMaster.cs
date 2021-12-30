using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    public class CustomerMaster
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "partnercode")]
        public int partnercode { get; set; }

        [JsonProperty(PropertyName = "customerid")]
        public int customerid { get; set; }

        [JsonProperty(PropertyName = "customername")]
        public string customername { get; set; }

        [JsonProperty(PropertyName = "customercode")]
        public string customercode { get; set; }

        [JsonProperty(PropertyName = "customeraddress")]
        public string customeraddress { get; set; }

        [JsonProperty(PropertyName = "dlnumber")]
        public string dlnumber { get; set; }

        [JsonProperty(PropertyName = "customeremail")]
        public string customeremail { get; set; }

        [JsonProperty(PropertyName = "gstnumber")]
        public string gstnumber { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string mobile { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        public string telephone { get; set; }

        [JsonProperty(PropertyName = "creditlimit")]
        public string creditlimit { get; set; }

        [JsonProperty(PropertyName = "DLExpiry")]
        public DateTime DLExpiry { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }

        [JsonProperty(PropertyName = "pincode")]
        public int pincode { get; set; }

        [JsonProperty(PropertyName = "customerstatus")]
        public string customerstatus { get; set; }

        [JsonProperty(PropertyName = "outstandingbal")]
        public string outstandingbal { get; set; }

        [JsonProperty(PropertyName = "CreditLockDays")]
        public int CreditLockDays { get; set; }

        [JsonProperty(PropertyName = "salesmancode")]
        public int salesmancode { get; set; }

        [JsonProperty(PropertyName = "salesmanarea")]
        public int salesmanarea { get; set; }

        [JsonProperty(PropertyName = "Contact")]
        public string Contact { get; set; }

        [JsonProperty(PropertyName = "State")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "active")]
        public int active { get; set; }

        [JsonProperty(PropertyName = "salesmanroute")]
        public int salesmanroute{ get; set; }

        [JsonProperty(PropertyName = "locked")]
        public int locked { get; set; }

        [JsonProperty(PropertyName = "lockreason")]
        public string lockreason { get; set; }

        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "UserPassword")]
        public string UserPassword { get; set; }

        [SQLite.Ignore]
        public float totalamount { get; set; }
    }
}
