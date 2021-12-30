using SinglaReportsApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SinglaReportsApi.Controllers
{
    public class ReportManagementController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesManCustomerOutstanding(int Partnercode, string salesmancode)
        {
            List<Outstanding> outStandingDetailsList = null;
            try
            {
                DataManager dm = new DataManager();
                outStandingDetailsList = await dm.CustomerOutstandingReport(Partnercode, salesmancode);
                if (outStandingDetailsList == null)
                {
                    Trace.TraceInformation("From GetCustomerOutstanding--Returns Null");
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                Trace.TraceInformation("From GetCustomerOutstanding--" + ex.Message);
            }
            return Ok(outStandingDetailsList);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCustomerMonthlySale(string custcode, string partnercode)
        {
            List<Customermonthsales> customermonthsalesList = null;
            try
            {
                DataManager dm = new DataManager();
                customermonthsalesList = await dm.GetCustomerMonthlySaleReport(custcode, partnercode);
                if (customermonthsalesList == null)
                {
                    Trace.TraceInformation("From GetCustomerMonthlySale--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCustomerMonthlySale--" + ex.Message);
            }
            return Ok(customermonthsalesList);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetOutstandingPendingBillsbyCustomer(int Partnercode, string custid)
        {
            List<OutstandingInvoices> customerOutstandingdetList = null;
            try
            {
                DataManager dm = new DataManager();
                customerOutstandingdetList = await dm.GetOutstandingInvoiceReportForIndividualCustomer(Partnercode, custid);

                if (customerOutstandingdetList == null)
                {
                    Trace.TraceInformation("From GetOutstandingPendingBillsbyCustomer--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetOutstandingPendingBillsbyCustomer--" + ex.Message);
            }
            return Ok(customerOutstandingdetList);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSalesSummaryReport(string sman, string pcode, string frmdt, string todt)
        {
            List<SalesSummary> SalesSummaryList = null;
            try
            {
                DataManager dm = new DataManager();
                SalesSummaryList = await dm.SalesSummaryReport(sman, pcode, frmdt, todt);
                if (SalesSummaryList == null)
                {
                    Trace.TraceInformation("From GetSalesSummaryReport--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetSalesSummaryReport--" + ex.Message + " Salesman: " + sman);
            }
            return Ok(SalesSummaryList);
        }
    }
}
