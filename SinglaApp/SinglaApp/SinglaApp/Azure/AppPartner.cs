using Newtonsoft.Json;

namespace SinglaApp.Azure
{
    public class AppPartner
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        
        public string Version { get; set; }



        [JsonProperty(PropertyName = "Code")]
        public int Code
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Name")]
        public string Name
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Branch")]
        public string Branch
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "ShortName")]
        public string ShortName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Address")]
        public string Address
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Type")]
        public string Type
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "Status")]
        public string Status
        {
            get;
            set;
        }
    }
}
