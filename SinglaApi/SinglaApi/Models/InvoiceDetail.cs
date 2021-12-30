using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglaApi.Models
{
    public class InvoiceDetail
    {
        public string id { get; set; }
        public int Salesman_id { get; set; }
        public int customer_id { get; set; }
        public string order_id { get; set; }
        public string Invoice_No { get; set; }
        public DateTime Invoice_date { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
        public double TaxAmount { get; set; }
        public double Discountamount { get; set; }
        public string Remarks { get; set; }
        public string UpdateStatus { get; set; }
        public int NoOfItem { get; set; }
        public int partnercode { get; set; }
        public string order_status { get; set; }
        public List<InvoiceItemDetail> InvoiceItemList = new List<InvoiceItemDetail>();
    }
    public class InvoiceItemDetail
    {
        public int itemcode { get; set; }
        public string itemaltercode { get; set; }
        public double Price { get; set; }
        public double MRP { get; set; }
        public double PTR { get; set; }
        public string Packsize { get; set; }
        public int Qty { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class PaymentDetail
    {
        public int custid { get; set; }
        public string custcode { get; set; }
        public string customername { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string mobile { get; set; }
        public int partnercode { get; set; }
        public string Invoice_Type { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CreditDebit { get; set; }
        public int Items { get; set; }
        public string OrderNumber { get; set; }
        public string status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal BillAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public int Credit_hold { get; set; }
        public int Smancode { get; set; }
        public int Area { get; set; }
        public int Routeid { get; set; }
        public string Remarks { get; set; }
        public DateTime syncdatetime { get; set; }
    }
    public class PaymentMethod
    {
        public string partnercode { get; set; }
        public string customerid { get; set; }
        public string customername { get; set; }
        public string customercode { get; set; }
        public string customeremail { get; set; }
        public string customermobile { get; set; }
        public string salesmancode { get; set; }
        public string salesmanname { get; set; }
        public string salesmanmobile { get; set; }
        public string paymentmethod  { get; set; }
        public DateTime  paymentdate { get; set; }
        public string chqno { get; set; }
        public DateTime chqdate { get; set; }
        public decimal amount { get; set; }
        public DateTime submitdate { get; set; }
        public string adjustinvoicenumber { get; set; }
        public string bankname { get; set; }
        public string neftnumber { get; set; }
        public DateTime syncdatetime { get; set; }
       
    }
}