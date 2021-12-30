using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SinglaApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SinglaApi.Controllers
{
    public class SinglaController : ApiController
    {
        List<ItemMaster> itemdatalist = new List<ItemMaster>();
        [HttpGet]
        public HttpResponseMessage GetOrders(string partnercode)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var orderDetails = dm.GetOrderDetailsByOrderdate(partnercode, myConnection);
                    if (orderDetails.Count != 0)
                    {
                        var itemlistcount = orderDetails.Where(i => i.orderItemList.Count != 0).ToList();
                        if (itemlistcount.Count != 0)
                        {
                            var result = Newtonsoft.Json.JsonConvert.SerializeObject(orderDetails);
                            response = Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StringContent(result, Encoding.UTF8, "application/json");
                            Trace.TraceInformation("GetOrders--OrdersJson:" + partnercode + "--" + result);
                        }
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetOrders--Error : " + ex.Message);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdateOrders(List<UpdatedOrderStatus> orderStatus)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                
                    var details = dm.UpdateOrderStatus(orderStatus, myConnection);
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("UpdateOrders--Error : " + ex.Message);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SyncProduct(List<ItemMaster> itemMaster)
        {
            HttpResponseMessage response = null;
            try
            {
                var invoicedetails = JsonConvert.SerializeObject(itemMaster);
                Trace.TraceInformation("SyncProduct--json : " + invoicedetails);
               
                    DataManager dm = new DataManager();
                    string details = string.Empty;
                   //details = dm.BulkInsertProducts(itemMaster);
                   details = dm.InsertProductDetailsToAzureDB(itemMaster);
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }

               
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("SyncProduct--Error : " + ex.Message);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SyncCustomer(List<CustomerMaster> custMaster)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    string details = string.Empty;
                    //details = dm.BulkInsertCustomers(custMaster, myConnection);
                    details = dm.InsertCustomerDetailsToAzureDB(custMaster);
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        Trace.TraceInformation("SyncCustomer--Faluere");
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("SyncCustomer--Error : " + ex.Message);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SyncSalesman(List<SalesmanMaster> salesMaster)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var details = dm.InsertSalesmanDetailsToAzureDB(salesMaster, myConnection);
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("SyncSalesman--Error : " + ex.Message);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdateInvoiceDetail(List<InvoiceDetail> invoiceStatus)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var invoicedetails = JsonConvert.SerializeObject(invoiceStatus);
                    Trace.TraceInformation("UpdateInvoiceDetail--json : " + invoicedetails);
                    var details = dm.UpdatedInvoiceStatus(invoiceStatus, myConnection);
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("UpdateInvoiceDetail--Error : " + ex.Message);
            }
            return response;
        }
        [HttpPost]
        public HttpResponseMessage syncoutstandingDetails(List<PaymentDetail> PaymentDetails)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    //var details = dm.InsertPaymentDetailsToAzureDB(PaymentDetails, myConnection);
                    var details = "success";
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("syncoutstandingDetails--Error : " + ex.Message);
            }
            return response;
        }
        [HttpPost]
        public HttpResponseMessage synPaymentDetails(PaymentMethod PaymentDetail)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var details = dm.InsertPaymentMethodToAzureDB(PaymentDetail, myConnection);
                    
                    if (details == "success")
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(details, Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, details);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("syncoutstandingDetails--Error : " + ex.Message);
            }
            return response;
        }
    }
}
