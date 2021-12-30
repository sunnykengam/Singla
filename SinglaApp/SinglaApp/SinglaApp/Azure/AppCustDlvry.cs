using Newtonsoft.Json;
using System;

namespace SinglaApp.Azure
{
    public class AppCustDlvry
    {
        [JsonProperty(PropertyName = "Custcode")]
        public int Custcode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "CustAltcode")]
        public string CustAltcode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Custname")]
        public string Custname
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "NoofBoxes")]
        public string NoofBoxes
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Noofpackes")]
        public string Noofpackes
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Photo")]
        public string Photo
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "InvoiceImage")]
        public string  InvoiceImage
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "DlvryDate")]
        public string DlvryDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "DlvryTime")]
        public string DlvryTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "InvoiceDeltails")]
        public string InvoiceDeltails
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "InvoiceDate")]
        public string InvoiceDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "DlvryCode")]
        public string DlvryCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "patnercode")]
        public int patnercode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        
        public string Version { get; set; }
    }
}
