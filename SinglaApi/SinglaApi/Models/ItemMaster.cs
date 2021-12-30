using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaApi.Models
{
    public class ItemMaster
    {
        public string id { get; set; }
        public int partnercode { get; set; }
        public int itemid { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string manufacturer { get; set; }
        public string packsize { get; set; }
        public float pts { get; set; }
        public float ptr { get; set; }
        public float mrp { get; set; }
        public string itemstatus { get; set; }
        public float boxsize { get; set; }
        public float stock { get; set; }
        public string Halfscheme { get; set; }
        public string Scheme { get; set; }
        public string scm1 { get; set; }
        public string scm2 { get; set; }

        public string caseqty { get; set; }

        public string maxinvqty { get; set; }
        public DateTime creationdate { get; set; }
        public string substitute { get; set; }
        public string manufacturergroup { get; set; }
        public string itemnamesearch { get; set; }
        public string itemremarks { get; set; }

        public string PartItemid { get; set; }
        public DateTime syncdatetime { get; set; }

        public string syncdtime { get; set; }
    }
}