using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model
{
    public class ItemMaster
    {
        [JsonProperty(PropertyName = "partnercode")]
        public int partnercode { get; set; }

        [JsonProperty(PropertyName = "itemid")]
        public int itemid { get; set; }

        [JsonProperty(PropertyName = "itemcode")]
        public string itemcode { get; set; }

        [JsonProperty(PropertyName = "itemname")]
        public string itemname { get; set; }

        [JsonProperty(PropertyName = "manufacturer")]
        public string manufacturer { get; set; }

        [JsonProperty(PropertyName = "manufacturergroup")]
        public string manufacturergroup { get; set; }

        [JsonProperty(PropertyName = "packsize")]
        public string packsize { get; set; }

        [JsonProperty(PropertyName = "ptr")]
        public float ptr { get; set; }

        [JsonProperty(PropertyName = "mrp")]
        public float mrp { get; set; }

        [JsonProperty(PropertyName = "boxsize")]
        public float boxsize { get; set; }

        [JsonProperty(PropertyName = "stock")]
        public float stock { get; set; }

        [JsonProperty(PropertyName = "Halfscheme")]
        public string Halfscheme { get; set; }

        [JsonProperty(PropertyName = "Scheme")]
        public string Scheme { get; set; }

        [JsonProperty(PropertyName = "creationdate")]
        public DateTime creationdate { get; set; }

        [JsonProperty(PropertyName = "substitute")]
        public string substitute { get; set; }

        [JsonProperty(PropertyName = "frmstockcolor")]
        public string frmstockcolor { get; set; }

        [JsonProperty(PropertyName = "itemnamesearch")]
        public string itemnamesearch { get; set; }

        [JsonProperty(PropertyName = "itemcodes")]
        public string itemcodes { get; set; }

        [SQLite.Ignore]
        public string qty { get; set; }

        [SQLite.Ignore]
        public int Increment { get; set; }

        [SQLite.Ignore]
        public string stock1 { get; set; }

        [SQLite.Ignore]
        public double OrderAmount { get; set; }

        [SQLite.Ignore]
        public string dummyqty { get; set; }

        [SQLite.Ignore]
        public double grandtotal { get; set; }

        [SQLite.Ignore]
        public int NoOfItems { get; set; }

        [SQLite.Ignore]
        public bool Ischecked { get; set; }

        [SQLite.Ignore]
        public int guid { get; set; }

        [SQLite.Ignore]
        public bool IsFocus { get; set; }

        [SQLite.Ignore]
        public bool RemoveIsEnabled { get; set; }

        [SQLite.Ignore]
        public int totalamount { get; set; }
    }
}
