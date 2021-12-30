using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaApi.Models
{
    public class SalesmanMaster
    {
        public string id { get; set; }
        public int partnercode { get; set; }
        public string username { get; set; }
        public string userpassword { get; set; }
        public int salesmanid { get; set; }
        public string salesmancode { get; set; }
        public string salesmanname { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public float salesmantarget { get; set; }
        public decimal salesmancommision { get; set; }
        public int Active { get; set; }
        public DateTime syncdatetime { get; set; }
    }
}