using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaMedicos.Model;

namespace SinglaMedicos.ViewModel
{
    public class ItemViewModel
    {
        private ItemMaster _itemmaster;
        public ItemViewModel(ItemMaster itemmaster)
        {
            this._itemmaster = itemmaster;
        }
        [JsonProperty(PropertyName = "partnercode")]
        public int partnercode { get { return _itemmaster.partnercode; } }


        [JsonProperty(PropertyName = "itemid")]
        public int itemid { get { return _itemmaster.itemid; } }

        [JsonProperty(PropertyName = "itemcode")]
        public string itemcode { get { return _itemmaster.itemcode; } }

        [JsonProperty(PropertyName = "itemname")]
        public string itemname { get { return _itemmaster.itemname; } }

        [JsonProperty(PropertyName = "manufacturer")]
        public string manufacturer { get { return _itemmaster.manufacturer; } }

          [JsonProperty(PropertyName = "manufacturergroup")]
        public string manufacturergroup { get { return _itemmaster.manufacturergroup; } }

        [JsonProperty(PropertyName = "packsize")]
        public string packsize { get { return _itemmaster.packsize; } }

        [JsonProperty(PropertyName = "ptr")]
        public float ptr { get { return _itemmaster.ptr; } }

        [JsonProperty(PropertyName = "mrp")]
        public float mrp { get { return _itemmaster.mrp; } }

        [JsonProperty(PropertyName = "boxsize")]
        public float boxsize { get { return _itemmaster.boxsize; } }

        [JsonProperty(PropertyName = "stock")]
        public float stock { get { return _itemmaster.stock; } }

        [JsonProperty(PropertyName = "Halfscheme")]
        public string Halfscheme { get { return _itemmaster.Halfscheme; } }

        [JsonProperty(PropertyName = "Scheme")]
        public string Scheme { get { return _itemmaster.Scheme; } }

        public string qty { get { return _itemmaster.qty; } }
        public double OrderAmount { get { return _itemmaster.OrderAmount; } }
        public string dummyqty { get { return _itemmaster.dummyqty; } }
        public double grandtotal { get { return _itemmaster.grandtotal; } }
        public int NoOfItems { get { return _itemmaster.NoOfItems; } }
        public bool Ischecked { get { return _itemmaster.Ischecked; } }
        public int guid { get { return _itemmaster.guid; } }
        public int totalamount { get { return _itemmaster.totalamount; } }
    }
}
