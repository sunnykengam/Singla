using CheckingApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CheckingApi.Controllers
{
    public class CheckingController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetUserDetail(string CheckerId)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var UserDetails = dm.GetCheckingUserMaster(CheckerId, myConnection);
                    if (UserDetails != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetUserDetail--Error : " + ex.Message);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCheckingDetails(string CheckerId, DateTime InvDate)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var UserDetails = dm.GetCheckingDetailsList(CheckerId,InvDate, myConnection);
                    if (UserDetails != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCheckingDetails--Error : " + ex.Message);
            }
            return response;
        }

        [HttpGet]
        public int GetCheckingHoldCount(string Custcode, DateTime InvDt, string Block, string Invnum)
        {
            int DetailsCount = 0;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    DetailsCount = dm.GetCheckingHoldCount(Custcode, InvDt, Block, Invnum, myConnection);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCheckingHoldCount--Error : " + ex.Message);
            }
            return DetailsCount;
        }

        [HttpGet]
        public HttpResponseMessage GetInvoiceDetails(string Custcode, DateTime InvDt, string Block, string Invnum)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var UserDetails = dm.GetInvoiceDetails(Custcode, InvDt, Block, Invnum, myConnection);
                    if (UserDetails != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetInvoiceDetails--Error : " + ex.Message);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetBatchWiseItem(string Custcode, DateTime InvDt, int Itemid, string Batch, string Invnum)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var UserDetails = dm.GetInvoiceDetail(Custcode, InvDt, Itemid,Batch, Invnum, myConnection);
                    if (UserDetails != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetBatchWiseItem--Error : " + ex.Message);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCustomerInvoiceSummary(string custcode, string invdate, string Invnum)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var UserDetails = dm.GetCustomerInvoiceSummary(custcode, invdate, Invnum, myConnection);
                    if (UserDetails != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCustomerInvoiceSummary--Error : " + ex.Message);
            }
            return response;
        }

        [HttpGet]
        public string GetEDPStatus(string custcode, string invdate, string Invnum)
        {
            string EsStatus = string.Empty;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    EsStatus = dm.GetEDPStatus(custcode, invdate, Invnum, myConnection);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCheckingHoldCount--Error : " + ex.Message);
            }
            return EsStatus;
        }


        //[HttpGet]
        //public HttpResponseMessage GetInvoiceNumbers(string custcode, string invdate, string Invnum)
        //{
        //    HttpResponseMessage response = null;
        //    try
        //    {
        //        using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
        //        {
        //            DataManager dm = new DataManager();
        //            var UserDetails = dm.GetInvoiceNumbersinApp_Picklist(custcode, invdate, Invnum, myConnection);
        //            if (UserDetails != null)
        //            {
        //                response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
        //            }
        //            else
        //            {
        //                response = null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.TraceInformation("GetInvoiceNumbers--Error : " + ex.Message);
        //    }
        //    return response;
        //}

        [HttpGet]
        public HttpResponseMessage GetInvoiceNuminAppInvoiceChanges(string custcode, string invdate, string Invnum)
        {
            HttpResponseMessage response = null;
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    DataManager dm = new DataManager();
                    var UserDetails = dm.GetInvoiceNumbersinAppInvoicechanges(custcode, invdate, Invnum, myConnection);
                    if (UserDetails != null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, UserDetails, "application/json");
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetInvoiceNuminAppInvoiceChanges--Error : " + ex.Message);
            }
            return response;
        }
    }
}
