using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    public class AreaMaster
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "partnercode")]
        public int partnercode { get; set; }

        [JsonProperty(PropertyName = "areaid")]
        public int areaid { get; set; }

        [JsonProperty(PropertyName = "areacode")]
        public string areacode { get; set; }

        [JsonProperty(PropertyName = "areaname")]
        public string areaname { get; set; }
    }
}
