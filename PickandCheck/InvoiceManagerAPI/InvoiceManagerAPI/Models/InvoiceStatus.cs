using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManagerAPI.Models
{
    public class InvoiceStatus
    {
        public int TotalBlocks { get; set; }
        public int pickedlines { get; set; }
        public int checkedlines { get; set; }
    }
    public partial class CustomerInvoiceSummary
    {
        public DateTime Invdate { get; set; }
        public string checkername { get; set; }
        public string routename { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public int Totalines { get; set; }
        public int pickedlines { get; set; }
        public int checkedlines { get; set; }
        public int pendinglines { get; set; }
        public int batchchanges { get; set; }
        public int qtychangelines { get; set; }
        public int nillines { get; set; }
    }
    public partial class PickedDetails
    {
        public int checkerid { get; set; }
        public string checkername { get; set; }
        public int custid { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
        public string routeid { get; set; }
        public string route { get; set; }
        public string Block { get; set; }
        public string Picked { get; set; }
        public string Checked { get; set; }
        public string pickername { get; set; }
        public string basketNo { get; set; }
        public string phone { get; set; }
    }
    public partial class UserMaster
    {
        public int checkerid { get; set; }
        public string checkername { get; set; }
    }
}