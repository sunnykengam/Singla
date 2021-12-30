using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaApp.Model.Reports
{
    public class Outstanding
    {
        [JsonProperty(PropertyName = "custcode")]
        public string custcode { get; set; }

        [JsonProperty(PropertyName = "customername")]
        public string customername { get; set; }

        [JsonProperty(PropertyName = "custbalanceamount")]
        public decimal custbalanceamount { get; set; }

        [JsonProperty(PropertyName = "salesmancode")]
        public string salesmancode { get; set; }

        public string lblorders { get; set; }

        public List<OutstandingInvoices> inv_List = new List<OutstandingInvoices>();
    }

    public class OutstandingInvoices
    {
        [JsonProperty(PropertyName = "invoicedate")]
        public string invoicedate { get; set; }

        [JsonProperty(PropertyName = "invoicenum")]
        public string invoicenum { get; set; }

        [JsonProperty(PropertyName = "noofitem")]
        public int noofitem { get; set; }

        [JsonProperty(PropertyName = "invtotalamount")]
        public decimal invtotalamount { get; set; }

        [JsonProperty(PropertyName = "invbalanceamount")]
        public decimal invbalanceamount { get; set; }

        [JsonProperty(PropertyName = "totaldiscount")]
        public decimal totaldiscount { get; set; }

        [JsonProperty(PropertyName = "Days")]
        public int Days { get; set; }
    }

    public class Custmermonthsales
    {
        public string partnercode { get; set; }
        public string custcode { get; set; }
        public string customername { get; set; }
        public string mnth { get; set; }
        public decimal sales { get; set; }
    }
}