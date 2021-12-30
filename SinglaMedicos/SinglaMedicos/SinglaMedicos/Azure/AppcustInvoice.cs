using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Azure
{
   public class AppcustInvoice
    {
      

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
       
        public string Version { get; set; }
        [JsonProperty("custcode")]
        public string custcode
        {
            get; set;
        }


        [JsonProperty("custname")]
        public string custname
        {
            get; set;
        }

        [JsonProperty("invnum")]
        public string invnum
        {
            get; set;
        }

        [JsonProperty("invdate")]
        public DateTime invdate
        {
            get; set;
        }

        [JsonProperty("address")]
        public string address
        {
            get; set;
        }
        [JsonProperty("address1")]
        public string address1
        {
            get; set;
        }

        [JsonProperty("address2")]
        public string address2
        {
            get; set;
        }

        [JsonProperty("address3")]
        public string address3
        {
            get; set;
        }

        [JsonProperty("address4")]
        public string address4
        {
            get; set;
        }



        [JsonProperty("noofitems")]
        public string noofitems
        {
            get; set;
        }


        [JsonProperty("invamt")]
        public string invamt
        {
            get; set;
        }

        [JsonProperty("telephone")]
        public string telephone
        {
            get; set;
        }

        [JsonProperty("mobile")]
        public string mobile
        {
            get; set;
        }

        [JsonProperty("Status")]
        public string Status
        {
            get; set;
        }

        [JsonProperty("partnercode")]
        public int partnercode
        {
            get; set;
        }

        [JsonProperty("DlvryStatus")]
        public string DlvryStatus
        {
            get;
            set;
        }
        [JsonProperty("DlvryDate")]
        public string DlvryDate
        {
            get;
            set;
        }
        [JsonProperty("DlvryTime")]
        public string DlvryTime
        {
            get;
            set;
        }
        

       [JsonProperty("createdAt")]
        public string createdAt
        {
            get;
            set;
        }
        [JsonProperty("RouteId")]
        public int RouteId
        {
            get;
            set;
        }
        [JsonProperty("RouteName")]
        public string RouteName
        {
            get;
            set;
        }

    }
}
