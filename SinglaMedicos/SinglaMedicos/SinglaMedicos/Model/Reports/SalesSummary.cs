using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaMedicos.Model.Reports
{
    public class SalesSummary
    {
        public string salesmancode { get; set; }
        public string salesmanname { get; set; }
        public int salesmantarget { get; set; }
        public string customername { get; set; }
        public int TotalBills { get; set; }
        public decimal Amount { get; set; }
    }
}
