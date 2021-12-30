using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Azure
{
   public class RouteMaster
    {
        [JsonProperty(PropertyName = "id")]
        public string id
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "createdAt")]
        public string createdAt
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "updatedAt")]
        public string updatedAt
        {
            get;
            set;
        }
       
        public string Version { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public string deleted
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Routeid")]
        public string RouteId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "RouteName")]
        public string RouteName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Partnercode")]
        public int Partnercode
        {
            get;
            set;
        }

    }
}
