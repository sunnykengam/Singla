using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
   public class AppPendingProducts
    {
        public string qty { get; set; }
        public string customercode { get; set; }
        public int salesmancode { get; set; }
        public float ptr { get; set; }
        public int itemid { get; set; }
        public int partnercode { get; set; }
    }
}
