using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SinglaReportsApi.Models
{
    public class DataManager
    {
        public async Task<List<Outstanding>> CustomerOutstandingReport(int Partnercode, string salesmancode)
        {
            List<Outstanding> customerOutstandingList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                try
                {
                    dbConnection.Open();
                    Trace.TraceInformation("GetCustomerOutstanding--CustomerOutstandingReport-- Retrieving From Azure Started");
                    string dbSales = "select smancode,custcode,custid,customername,sum(balanceamount) as balanceamount from outstanding  where  partnercode = '" + Partnercode + "'"+
                        "  and balanceamount<> 0 and invoice_type = 'SB' and smancode = '"+ salesmancode + "' group by smancode,custcode,customername,custid order by customername";

                    SqlDataAdapter adapter = new SqlDataAdapter(dbSales, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    customerOutstandingList = ds.Tables[0].AsEnumerable().
                               Select(dataRow => new Outstanding
                               {
                                   custcode = dataRow["custcode"].ToString(),
                                   customername = dataRow["customername"].ToString(),
                                   custbalanceamount = Convert.ToDecimal(dataRow["balanceamount"].ToString()),
                                   salesmancode = dataRow["smancode"].ToString(),
                                   inv_List = GetCustomerOutstandingInvoiceReport(Partnercode, dataRow["custid"].ToString(), dbConnection),
                               }).ToList();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetCustomerOutstanding--CustomerOutstandingReport--" + ex.Message);
                }
            }
            return customerOutstandingList;
        }

        public List<OutstandingInvoices> GetCustomerOutstandingInvoiceReport(int Partnercode, string custid, SqlConnection dbConnection)
        {
            List<OutstandingInvoices> customerOutstandingdetList = null;
            try
            {
                Trace.TraceInformation("GetCustomerOutstanding--CustomerOutstandingReport-- GetCustomerOutstandingInvoiceReport --Retrieving From Azure Started");
                customerOutstandingdetList = GetOutstandingReportForCustomer(Partnercode, custid, dbConnection);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCustomerOutstanding--CustomerOutstandingReport--GetCustomerOutstandingInvoiceReport" + ex.Message);
            }
            return customerOutstandingdetList;
        }

        public async Task<List<OutstandingInvoices>> GetOutstandingInvoiceReportForIndividualCustomer(int Partnercode, string custid)
        {
            List<OutstandingInvoices> customerOutstandingdetList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                try
                {
                    dbConnection.Open();
                    Trace.TraceInformation("GetCustomerOutstanding--CustomerOutstandingReport-- GetCustomerOutstandingInvoiceReport --Retrieving From Azure Started");
                    customerOutstandingdetList = GetOutstandingReportForCustomer(Partnercode, custid, dbConnection);
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetOutstandingPendingBillsbyCustomer--GetOutstandingInvoiceReportForIndividualCustomer--" + ex.Message);
                }
            }
            return customerOutstandingdetList;
        }

        private List<OutstandingInvoices> GetOutstandingReportForCustomer(int Partnercode, string custid, SqlConnection dbConnection)
        {
            List<OutstandingInvoices> customerOutstandingdetList;
            string dbSales = "select distinct o.custcode,o.customername,o.invoicedate,o.invoicenum,o.totalamount,o.balanceamount,o.totaldiscount," +
                "o.ordernumber,'0' as noofitems from outstanding o  where o.partnercode = '" + Partnercode + "' and o.balanceamount <> 0 and" +
                " o.invoice_type = 'SB' and o.custid = '" + custid + "' order by invoicedate,invoicenum";
            SqlDataAdapter adapter = new SqlDataAdapter(dbSales, dbConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            customerOutstandingdetList = ds.Tables[0].AsEnumerable().
                       Select(dataRow => new OutstandingInvoices
                       {
                           invoicenum = dataRow["invoicenum"].ToString(),
                           invoicedate = Convert.ToDateTime(dataRow["invoicedate"]).ToString("yyyy-MM-dd"),
                           noofitem = Convert.ToInt32(dataRow["noofitems"].ToString()),
                           invtotalamount = Convert.ToDecimal(dataRow["totalamount"].ToString()),
                           invbalanceamount = Convert.ToDecimal(dataRow["balanceamount"].ToString()),
                           totaldiscount = Convert.ToDecimal(dataRow["totaldiscount"].ToString()),
                           Days = GetTimeDiff(Convert.ToDateTime(dataRow["invoicedate"]).ToString("yyyy-MM-dd")),
                       }).ToList();
            return customerOutstandingdetList;
        }

        public int GetTimeDiff(string invoicedate)
        {
            DateTime inv_date = Convert.ToDateTime(invoicedate);
            TimeSpan diff = DateTime.Today - inv_date;
            return diff.Days;
        }

        public async Task<List<Customermonthsales>> GetCustomerMonthlySaleReport(string custcode, string partnercode)
        {
            List<Customermonthsales> CustmermonthsalesList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                try
                {
                    dbConnection.Open();
                    Trace.TraceInformation("GetCustomerMonthlySale--GetCustomerMonthlySaleReport-- Retrieving From Azure Started");
                    string monthlysales = "SELECT  PARTNERCODE,CUSTCODE,CUSTOMERNAME,MNTH,sum(SALES)as SALES,mnthsequence FROM MONTHLYSALES WHERE" +
                                           " custcode='" + custcode + "' AND PARTNERCODE = '" + partnercode + "'" +
                                           " GROUP BY PARTNERCODE,CUSTCODE,CUSTOMERNAME,MNTH,mnthsequence  ORDER BY mnthsequence desc";

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(monthlysales, dbConnection);
                    sda.Fill(dt);
                    CustmermonthsalesList = (from DataRow row in dt.Rows
                                             select new Customermonthsales
                                             {
                                                 partnercode = row["PARTNERCODE"].ToString(),
                                                 custcode = row["CUSTCODE"].ToString(),
                                                 customername = row["CUSTOMERNAME"].ToString(),
                                                 mnth = row["MNTH"].ToString(),
                                                 sales = Convert.ToDecimal(row["SALES"].ToString())
                                             }).ToList();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetCustomerMonthlySale--GetCustomerMonthlySaleReport--" + ex.Message);
                }
            }
            return CustmermonthsalesList;
        }

        public async Task<List<SalesSummary>> SalesSummaryReport(string salesmancode, string partnercode, string Fromdate, string Todate)
        {
            List<SalesSummary> SalesSummaryList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                try
                {
                    dbConnection.Open();
                    Trace.TraceInformation("GetSalesSummaryReport--SalesSummaryReport-- Retrieving SalesSummary From Azure Started");
                    string dbSales = "select SmanCode,SmanName,SmanTarget,custcode+' - '+customername as customername,count(InvoiceNum) as TotalBills,sum(TotalAmount) as Amount  from SalesSummary where InvoiceDate between '" + Fromdate + "' and '" + Todate + "' and smanid ='" + salesmancode + "' and partnercode='" + partnercode + "' group by SmanCode,SmanName,SmanTarget,custcode,customername order by customername";
                    SqlDataAdapter adapter = new SqlDataAdapter(dbSales, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    SalesSummaryList = ds.Tables[0].AsEnumerable().
                               Select(dataRow => new SalesSummary
                               {
                                   salesmancode = dataRow["SmanCode"].ToString(),
                                   customername = dataRow["customername"].ToString(),
                                   salesmanname = dataRow["SmanName"].ToString(),
                                   salesmantarget = dataRow["SmanTarget"].ToString(),
                                   TotalBills = Convert.ToInt32(dataRow["TotalBills"].ToString()),
                                   Amount = Convert.ToDecimal(dataRow["Amount"])
                               }).ToList();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetSalesSummaryReport--SalesSummaryReport--" + ex.Message);
                }
            }
            return SalesSummaryList;
        }
    }
}