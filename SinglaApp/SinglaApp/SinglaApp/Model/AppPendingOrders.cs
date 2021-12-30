using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
   public class AppPendingOrders
    {
        [SQLite.Column("customername")]
        public string customername { get; set; }

        [SQLite.Column("salesmancode")]
        public int salesmancode { get; set; }

        [SQLite.Column("customercode")]
        public string customercode { get; set; }

        [SQLite.Column("NoOfItems")]
        public int NoOfItems { get; set; }

        [SQLite.Column("partnercode")]
        public int partnercode { get; set; }

        [SQLite.Column("IsChecked")]
        public bool IsChecked { get; set; }

        [SQLite.Ignore]
        public int totalamount { get; set; }

        [SQLite.Ignore]
        public string guid { get; set; }

        [SQLite.Ignore]
        public bool isenable { get; set; }

         [SQLite.Ignore]
        public bool editisvisible { get; set; }

         [SQLite.Ignore]
        public bool saveisvisible { get; set; }

        [SQLite.Ignore]
        public string bgmcolor { get; set; }

          [SQLite.Ignore]
        public string frmitemcolor { get; set; }

        [SQLite.Ignore]
        public List<CustomerMaster> pickcustomer { set; get; }
    }
}
