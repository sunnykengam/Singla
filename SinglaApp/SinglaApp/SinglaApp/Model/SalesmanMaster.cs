using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    public class SalesmanMaster
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "partnercode")]
        public int partnercode { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }

        [JsonProperty(PropertyName = "userpassword")]
        public string userpassword { get; set; }

        [JsonProperty(PropertyName = "salesmanid")]
        public int salesmanid { get; set; }

        [JsonProperty(PropertyName = "salesmancode")]
        public string salesmancode { get; set; }

        [JsonProperty(PropertyName = "salesmanname")]
        public string salesmanname { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string mobile { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        public string telephone { get; set; }

        [JsonProperty(PropertyName = "salesmantarget")]
        public float salesmantarget { get; set; }

        [JsonProperty(PropertyName = "salesmancommision")]
        public string salesmancommision { get; set; }

       
    }
}
