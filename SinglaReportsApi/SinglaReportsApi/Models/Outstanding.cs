using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaReportsApi.Models
{
    public class Outstanding
    {
        public string custcode { get; set; }
        public string customername { get; set; }
        public decimal custbalanceamount { get; set; }
        public string salesmancode { get; set; }
        public List<OutstandingInvoices> inv_List = new List<OutstandingInvoices>();
    }

    public class OutstandingInvoices
    {
        public string invoicedate { get; set; }
        public string invoicenum { get; set; }
        public int noofitem { get; set; }
        public decimal invtotalamount { get; set; }
        public decimal invbalanceamount { get; set; }
        public decimal totaldiscount { get; set; }
        public int Days { get; set; }
    }
    public class Customermonthsales
    {
        public string partnercode { get; set; }
        public string custcode { get; set; }
        public string customername { get; set; }
        public string mnth { get; set; }
        public decimal sales { get; set; }
    }
    public class SalesSummary
    {
        public string salesmancode { get; set; }
        public string salesmanname { get; set; }
        public string salesmantarget { get; set; }
        public string customername { get; set; }
        public int TotalBills { get; set; }
        public decimal Amount { get; set; }
    }
}
