using SinglaAppSyncApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SinglaAppSyncApi.Controllers
{
    public class DataSyncController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetsalesLogin(string Username, string Password)
        {
            List<SalesmanMaster> salesList = null;
            try
            {
                DataManager dm = new DataManager();
                salesList = await dm.GetsalesmanLogincheck(Username, Password);
                if (salesList == null)
                {
                    Trace.TraceInformation("From GetsalesLogin--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("From GetsalesLogin--" + ex.Message);
            }
            return Ok(salesList);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCustLogin(string Username, string Password)
        {
            List<CustomerMaster> custList = null;
            try
            {
                DataManager dm = new DataManager();
                custList = await dm.GetCustomerLogincheck(Username, Password);
                if (custList == null)
                {
                    Trace.TraceInformation("From GetCustLogin--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("From GetCustLogin--" + ex.Message);
            }
            return Ok(custList);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetProducts(int partnercode)
        {
            List<ItemMaster> orderList = null;
            try
            {
                DataManager dm = new DataManager();
                orderList = await dm.GetProductdetails(partnercode);
                if (orderList == null)
                {
                    Trace.TraceInformation("From GetProducts--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("From GetProducts--" + ex.Message);
            }
            return Ok(orderList);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllCustomers(string salesmanid, string partnercode)
        {
            List<CustomerMaster> customerList = null;
            try
            {
                DataManager dm = new DataManager();
                customerList = await dm.GetAllCustomerdetails(salesmanid, partnercode);
                if (customerList == null)
                {
                    Trace.TraceInformation("From GetAllCustomers--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("From GetAllCustomers--" + ex.Message);
            }
            return Ok(customerList);
        }
        [HttpGet]
        public HttpResponseMessage GetCompliteOrdersCustomer(string Custmerid, string Fromdate, string Todate, string Partnercode)
        {
            HttpResponseMessage response = null;
            try
            {
                DataManager dm = new DataManager();

                var orderDetails = dm.GetOrderhistorybycustomet(Custmerid, Fromdate, Todate, Partnercode);

                if (orderDetails.Count != 0)
                {
                    var itemlistcount = orderDetails.Where(i => i.orderItemList.Count != 0).ToList();
                    if (itemlistcount.Count != 0)
                    {
                        var result = Newtonsoft.Json.JsonConvert.SerializeObject(orderDetails);
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(result, Encoding.UTF8, "application/json");
                        Trace.TraceInformation("GetOrders--OrdersJson:" + Custmerid + "--" + result);
                    }
                }
                else
                {
                    response = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetCompliteOrdersSman(string smanid, string Fromdate, string Todate, string Partnercode)
        {
            HttpResponseMessage response = null;
            try
            {
                DataManager dm = new DataManager();

                var orderDetails = dm.GetOrderhistorybySman(smanid, Fromdate, Todate, Partnercode);

                if (orderDetails.Count != 0)
                {
                    var itemlistcount = orderDetails.Where(i => i.orderItemList.Count != 0).ToList();
                    if (itemlistcount.Count != 0)
                    {
                        var result = Newtonsoft.Json.JsonConvert.SerializeObject(orderDetails);
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(result, Encoding.UTF8, "application/json");
                        Trace.TraceInformation("GetOrders--OrdersJson:" + smanid + "--" + result);
                    }
                }
                else
                {
                    response = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetUpdatedstatus(string orderid)
        {
            List<Orderstatus> orderList = null;
            try
            {
                DataManager dm = new DataManager();
                orderList = await dm.Getupdatedstatu(orderid);
                if (orderList == null)
                {
                    Trace.TraceInformation("From Getupdatedstatus--Returns Null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("From Getupdatedstatus--" + ex.Message);
            }
            return Ok(orderList);
        }
    }
}
