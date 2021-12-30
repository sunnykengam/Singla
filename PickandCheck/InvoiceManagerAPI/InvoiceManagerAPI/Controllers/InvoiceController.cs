using CheckingDataAccess;
using InvoiceManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace InvoiceManagerAPI.Controllers
{
    public class InvoiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetUserDetail(string CheckerId)
        {
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                Config.Log("Getting User Detail From Local Server Initiated for itemId : " + CheckerId + " --GetUserDetail");
                var User = context.App_Checking_New_GetUserMaster(CheckerId);
                response = Request.CreateResponse(HttpStatusCode.OK, User, "application/json");
                Config.Log("Getting User Detail From Local Server Finished for itemId : " + CheckerId + " --GetUserDetail()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read user data of itemId : " + CheckerId + " from App_GetUserMaster--GetUserDetail()");
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPickedDetails(int CheckerId, DateTime InvDate)
        {

            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                Config.Log("Getting Picked Details From Local Server Initiated for itemId :" + CheckerId + " and invdate: " + InvDate.ToString() + " --GetPickedDetails()");
                var PickedDetails = context.App_Checking_New_GetCheckedDetails(CheckerId, InvDate.ToString("yyyy-MM-dd"));
                int i = 1;
                List<PickedDetail> GetPickedDetails = new List<PickedDetail>();
                foreach (App_Checking_New_GetCheckedDetails_Result obj in PickedDetails)
                {
                    PickedDetail objPicked = new PickedDetail();
                    objPicked.sno = i.ToString();
                    i++;
                    objPicked.checkerid = Convert.ToInt32(obj.ChickerId);
                    objPicked.checkername = obj.checkername;
                    objPicked.custid = Convert.ToInt32(obj.custid);
                    objPicked.custcode = obj.custcode;
                    objPicked.custname = obj.custname;
                    objPicked.routeid = obj.routeid;
                    objPicked.pickername = obj.pickername;
                    objPicked.basketNo = obj.basketNo;
                    objPicked.phone = obj.phone;
                    objPicked.route = obj.route;
                    objPicked.Block = obj.Block.Trim();
                    objPicked.Invnum = obj.Invnum.ToString();
                    if (!string.IsNullOrEmpty(obj.Picked))
                        objPicked.Picked = obj.Picked.Trim();
                    if (!string.IsNullOrEmpty(obj.Checked))
                        objPicked.Checked = obj.Checked.Trim();

                    if (objPicked.Picked == "y" && objPicked.Checked == "y")
                        objPicked.IsDisable = false;
                    else
                        objPicked.IsDisable = true;

                    GetPickedDetails.Add(objPicked);
                }
                response = Request.CreateResponse(HttpStatusCode.OK, GetPickedDetails, "application/json");
                Config.Log("Getting Picked Details From Local Server Finished for itemId : " + CheckerId + " and invdate: " + InvDate.ToString() + " --GetPickedDetails()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + CheckerId + " and invdate: " + InvDate.ToString() + " from App_GetPickedDetails--GetPickedDetails()");
            }
            return response;
        }

        [HttpGet]
        public int GetCheckingHoldCount(string Custcode, DateTime InvDt, string Block,string Invnum)
        {
            int count = 0;
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            List<InvoiceDetail> InvoiceDetails = new List<InvoiceDetail>();
            Config.Log("Getting Invoice Details From Local Server Initiated for phone code :" + Custcode + " --GetInvoiceDetails()");
            try
            {
                var invoices = context.App_Checking_New_GetCheckingHoldCount(Custcode, InvDt, Block, Invnum);
                string ddd = invoices.FirstOrDefault().ToString();
                count = Convert.ToInt32(ddd);
            }
            catch (Exception ex)
            {
                count = 0;
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + Custcode + " from App_CheckingDetails--GetInvoiceDetails()");
            }
            response = Request.CreateResponse(HttpStatusCode.OK, InvoiceDetails, "application/json");
            Config.Log("Getting Invoice Details From Local Server Finished for itemId : " + Custcode + " --GetInvoiceDetails()");
            return count;
        }
        [HttpGet]
        public HttpResponseMessage GetInvoiceDetails(string Custcode, DateTime InvDt, string Block, string Invnum)
        {
            HttpResponseMessage response = null;

            SinglaEntities context = new SinglaEntities();
            List<InvoiceDetail> InvoiceDetails = new List<InvoiceDetail>();
            Config.Log("Getting Invoice Details From Local Server Initiated for phone code :" + Custcode + " --GetInvoiceDetails()");
            try
            {
                var invoices = context.App_Checking_New_GetCheckList(Custcode, InvDt.ToString("yyyy-MM-dd"), Block, Invnum);
                int i = 1;
                foreach (App_Checking_New_GetCheckList_Result obj in invoices)
                {
                    InvoiceDetail detail = new InvoiceDetail();

                    detail.sno = i.ToString();

                    detail.Id = obj.Id;
                    detail.CustId = Convert.ToInt32(obj.CustId);
                    detail.CustCode = obj.CustCode;
                    detail.CustomerName = obj.CustomerName;
                    detail.Invdt = Convert.ToDateTime(obj.Invdt);
                    detail.Itemname = obj.Itemname;
                    detail.Pack = obj.Pack;
                    detail.Location = obj.Location;
                    detail.Status = obj.Status;
                    detail.Qty = Convert.ToDouble(obj.Qty);
                    detail.NewQty = Convert.ToInt32(obj.NewQty);
                    detail.Picked = obj.Picked;
                    detail.Invnum = obj.Invnum;
                    detail.Itemid = Convert.ToInt32(obj.Itemid);
                    detail.itemcode = obj.itemcode;
                    detail.Batchid = Convert.ToInt32(obj.Batchid);
                    detail.Batch = obj.Batch;
                    detail.expiry = obj.expiry;
                    detail.isbatch = obj.isbatch;
                    detail.isqty = obj.isqty;
                    detail.isdelete = obj.isdelete;
                    detail.isUnpick = obj.isUnpick;
                    detail.route = obj.route;
                    detail.InvType = obj.InvType;
                    detail.FQty = Convert.ToDouble(obj.FQty);
                    detail.mrp = Convert.ToDecimal(obj.mrp);
                    detail.Rate = Convert.ToDecimal(obj.rate);
                    if (obj.Chicked == "y")
                        detail.Chicked = true;
                    else
                        detail.Chicked = false;
                    detail.BasketNo = obj.BasketNo;
                    detail.CheckedTime = obj.checkedtime;
                    DateTime Fromdate = detail.Invdt.AddMonths(-12);
                    detail.Scheme = obj.Scheme;
                    detail.NewBatchid = obj.Newbatchid;
                    detail.NewBatch = obj.NewBatch;
                    detail.ManualBatch = obj.ManualBatch;
                    detail.NewFQty = Convert.ToInt32(obj.NewFqty);
                    detail.BatchQty = obj.BatchQty;

                    var Batchids = context.App_Checking_New_GetBatchListByItemId(obj.Itemid, Fromdate, detail.Invdt, Convert.ToDouble(obj.rate)).ToList();
                    var batids = Batchids;
                    detail.BatchList = new List<PickerList>();
                    foreach (var item in Batchids.ToList())
                    {
                        PickerList newlist = new PickerList();

                        var isAdded = detail.BatchList.Where(j => j.batch == item.batch).FirstOrDefault();
                        if (isAdded == null)
                        {
                            var mulbatch = batids.Where(k => k.batch == item.batch).ToList();
                            if (mulbatch.Count == 1)
                            {
                                newlist.PickerDisply = "New Batch: " + item.batch + "   Qty: " + (int)item.qty + "   MRP: " + item.MRP + "   Scheme: " + item.Scheme;
                                newlist.batch = item.batch;
                                newlist.batchid = item.batchid;
                                newlist.Scheme = item.Scheme;
                                newlist.MRP = Convert.ToDouble(item.MRP);
                                newlist.Rate = Convert.ToDouble(item.Rate);
                                detail.BatchList.Add(newlist);
                            }
                            else
                            {
                                foreach (App_Checking_New_GetBatchListByItemId_Result item1 in mulbatch)
                                {
                                    item.qty = item.qty + item1.qty;
                                }
                                newlist.PickerDisply = "New Batch: " + item.batch + "   Qty: " + (int)item.qty + "   MRP: " + item.MRP + "   Scheme: " + item.Scheme;
                                newlist.batch = item.batch;
                                newlist.batchid = item.batchid;
                                newlist.Scheme = item.Scheme;
                                newlist.MRP = Convert.ToDouble(item.MRP);
                                newlist.Rate = Convert.ToDouble(item.Rate);
                                detail.BatchList.Add(newlist);
                            }
                        }
                    }
                    if (detail.BatchList.Count() == 1)
                        detail.IsEnabled = false;

                    var result = InvoiceDetails.Where(J => J.Itemname == obj.Itemname).FirstOrDefault();
                    if (result == null)
                    {
                        InvoiceDetails.Add(detail);
                        i++;
                    }
                    else
                    {
                        var result1 = InvoiceDetails.Where(J => J.Itemname == obj.Itemname && J.Batch == obj.Batch).FirstOrDefault();
                        if (result1 != null)
                        {
                            result1.Qty = result1.Qty + Convert.ToInt32(obj.Qty);
                            result1.FQty = result1.FQty + Convert.ToInt32(obj.FQty);
                            result1.NewQty = result1.NewQty + Convert.ToInt32(obj.NewQty);
                        }
                        else
                        {
                            InvoiceDetails.Add(detail);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + Custcode + " from App_CheckingDetails--GetInvoiceDetails()");
            }
            response = Request.CreateResponse(HttpStatusCode.OK, InvoiceDetails, "application/json");
            Config.Log("Getting Invoice Details From Local Server Finished for itemId : " + Custcode + " --GetInvoiceDetails()");
            return response;
        }




        [HttpGet]
        public HttpResponseMessage GetBatchMrpExpWiseRate(string Batch, float MRP, string Exp)
        {
            HttpResponseMessage response = null;
            List<BatchIdsByItemId> BatchidList = new List<BatchIdsByItemId>();
            try
            {
                string dbSales = string.Empty;
                var con = ConfigurationManager.ConnectionStrings["masterDb"].ToString();
                using (SqlConnection dbConnection = new SqlConnection(con))
                {
                    dbConnection.Open();
                    dbSales = "select i.code as itemid,i.barcode as itemcode,i.name as itemname,i.pack as pack,f.batch as batch,f.expiry as expiry," +
                       " f.bqty as qty,f.srate as Rate,f.mrp as MRP,f.psrlno as batchid,f.vdt as Invdt,f.ScmStr as Scheme  from item i, fifo f where f.Itemc = i.code" +
                       " and f.hold <> 'H' and f.mrp = '" + MRP + "' and f.Batch = '" + Batch + "' and f.Expiry = '" + Exp + "'  order by f.vdt desc";
                    SqlCommand cmd = new SqlCommand(dbSales, dbConnection);
                    var BatchData = cmd.ExecuteScalar();
                    if (BatchData == null)
                    {
                        dbSales = "select i.code as itemid,i.barcode as itemcode,i.name as itemname,i.pack as pack,f.batch as batch,f.expiry as expiry," +
                    " f.bqty as qty,f.srate as Rate,f.mrp as MRP,f.psrlno as batchid,f.vdt as Invdt,f.ScmStr as Scheme  from item i, fifo f where f.Itemc = i.code" +
                    " and f.hold <> 'H' and f.mrp = '" + MRP + "' and f.Expiry = '" + Exp + "'  order by f.vdt desc";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(dbSales, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    BatchidList = ds.Tables[0].AsEnumerable().
                               Select(dataRow => new BatchIdsByItemId
                               {
                                   itemid = Convert.ToInt32(dataRow["itemid"].ToString()),
                                   itemcode = dataRow["itemcode"].ToString(),
                                   itemname = dataRow["itemname"].ToString(),
                                   pack = dataRow["pack"].ToString(),
                                   batch = dataRow["batch"].ToString(),
                                   expiry = dataRow["expiry"].ToString(),
                                   qty = Convert.ToDecimal(dataRow["qty"].ToString()),
                                   Rate = Convert.ToDouble(dataRow["Rate"].ToString()),
                                   MRP = Convert.ToDouble(dataRow["MRP"].ToString()),
                                   batchid = Convert.ToInt32(dataRow["batchid"].ToString()),
                                   Invdt = dataRow["Invdt"].ToString(),
                                   Scheme = dataRow["Scheme"].ToString(),

                               }).ToList();
                    dbConnection.Close();
                }
                if(BatchidList.Count==0)
                    response = Request.CreateResponse(HttpStatusCode.OK, BatchidList, "application/json");
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, BatchidList[0], "application/json");
            }
            catch (Exception ex)
            {
            }
            return response;
        }

        

        [HttpGet]
        public HttpResponseMessage GetBatchWiseItem(string Custcode, DateTime InvDt, int Itemid,string Batch,string Invnum)
        {
            HttpResponseMessage response = null;

            SinglaEntities context = new SinglaEntities();
            List<InvoiceDetail> InvoiceDetails = new List<InvoiceDetail>();
            Config.Log("Getting Invoice Details From Local Server Initiated for phone code :" + Custcode + " --GetInvoiceDetails()");
            try
            {
                ObjectResult<App_Checking_New_GetBatchWiseItem_Result> invoices = null;
                int i = 1;

                if (Itemid == 0 || !string.IsNullOrEmpty(Batch))
                {
                     invoices = context.App_Checking_New_GetBatchWiseItem(Custcode, InvDt.ToString("yyyy-MM-dd"), Itemid, Batch, Invnum);
                }
                foreach (App_Checking_New_GetBatchWiseItem_Result obj in invoices)
                {
                    InvoiceDetail detail = new InvoiceDetail();

                    detail.sno = i.ToString();
                    i++;
                    detail.Id = obj.Id;
                    detail.CustCode = obj.CustCode;
                    detail.CustomerName = obj.CustomerName;
                    detail.Invdt = Convert.ToDateTime(obj.Invdt);
                    DateTime Fromdate = detail.Invdt.AddMonths(-6);
                    detail.Qty = Convert.ToInt32(obj.Qty);
                    detail.CustId = Convert.ToInt32(obj.CustId);
                    detail.Itemname = obj.Itemname;
                    detail.Pack = obj.Pack;
                    detail.Location = obj.Location;
                    detail.Status = obj.Status;
                    detail.NewQty = Convert.ToInt32(obj.NewQty);
                    detail.Picked = obj.Picked;
                    detail.Invnum = obj.Invnum;
                    detail.Itemid = Convert.ToInt32(obj.Itemid);
                    detail.Batchid = Convert.ToInt32(obj.Batchid);
                    detail.Batch = obj.Batch;
                    detail.expiry = obj.expiry;
                    detail.isbatch = obj.isbatch;
                    detail.isqty = obj.isqty;
                    detail.isdelete = obj.isdelete;
                    detail.isUnpick = obj.isUnpick;
                    detail.route = obj.route;
                    detail.InvType = obj.InvType;
                    detail.mrp = Convert.ToDecimal(obj.mrp);
                    detail.Rate = Convert.ToDecimal(obj.rate);
                    if (obj.Chicked == "y")
                        detail.Chicked = true;
                    else
                        detail.Chicked = false;
                    detail.BasketNo = obj.BasketNo;
                    detail.itemcode = obj.itemcode;
                    detail.NewBatchid = obj.Newbatchid;
                    detail.NewBatch = obj.NewBatch;
                    detail.ManualBatch = obj.ManualBatch;
                    detail.FQty = Convert.ToInt32(obj.FQty);
                    detail.NewFQty = Convert.ToInt32(obj.NewFqty);
                    InvoiceDetails.Add(detail);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + Custcode + " from App_CheckingDetails--GetInvoiceDetails()");
            }
            response = Request.CreateResponse(HttpStatusCode.OK, InvoiceDetails, "application/json");
            Config.Log("Getting Invoice Details From Local Server Finished for itemId : " + Custcode + " --GetInvoiceDetails()");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetInvoiceStatus(string Custcode, string invdate, string Invnum)
        {
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                Config.Log("Getting Invoice status From Local Server Initiated for itemId :" + Custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
                var Batchids = context.App_Checking_New_GetInvoiceStatus(Custcode, invdate,Invnum);
                response = Request.CreateResponse(HttpStatusCode.OK, Batchids, "application/json");
                Config.Log("Getting Invoice status From Local Server Finished for itemId : " + Custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + Custcode + " and invdate: " + invdate + " from App_InvoiceStatus--GetInvoiceStatus()");
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetEDPStatus(string custcode, string invdate,string Invnum)
        {
            string Result = string.Empty;
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter output = new System.Data.Entity.Core.Objects.ObjectParameter("EsStatus", typeof(string));
                Config.Log("Getting Invoice status From Local Server Initiated for itemId :" + custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
                var Batchids = context.App_Checking_New_GetEdpStatus(custcode, invdate, Invnum, output);
                Result = output.Value.ToString();
                response = Request.CreateResponse(HttpStatusCode.OK, Result, "application/json");
                Config.Log("Getting Invoice status From Local Server Finished for itemId : " + custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + custcode + " and invdate: " + invdate + " from App_InvoiceStatus--GetInvoiceStatus()");
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetInvoiceNuminAppInvoiceChanges(string custcode, DateTime invdate, string Invnum)
        {
            string Result = string.Empty;
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter output = new System.Data.Entity.Core.Objects.ObjectParameter("EsStatus", typeof(string));
                Config.Log("Getting Invoice status From Local Server Initiated for itemId :" + custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
                var Invoicenum = context.App_Checking_New_GetInvoiceNuminAppInvoiceChanges(custcode,  invdate, Invnum);
                Result = Invoicenum.ToString();
                response = Request.CreateResponse(HttpStatusCode.OK, Invoicenum, "application/json");
                Config.Log("Getting Invoice status From Local Server Finished for itemId : " + custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + custcode + " and invdate: " + invdate + " from App_InvoiceStatus--GetInvoiceStatus()");
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetInvoiceNumbers(string custcode, string invdate,string Invnum)
        {
            string Result = string.Empty;
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter output = new System.Data.Entity.Core.Objects.ObjectParameter("EsStatus", typeof(string));
                Config.Log("Getting Invoice status From Local Server Initiated for itemId :" + custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
                var Invoicenum = context.App_Checking_New_GetInvoiceNumbers(custcode, invdate,Invnum);
               Result = Invoicenum.ToString();
                response = Request.CreateResponse(HttpStatusCode.OK, Invoicenum, "application/json");
                Config.Log("Getting Invoice status From Local Server Finished for itemId : " + custcode + " and invdate: " + invdate + " --GetInvoiceStatus()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of itemId : " + custcode + " and invdate: " + invdate + " from App_InvoiceStatus--GetInvoiceStatus()");
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCustomerInvoiceSummary(string custcode, string invdate, string Invnum)
        {
            HttpResponseMessage response = null;
            SinglaEntities context = new SinglaEntities();
            try
            {
                Config.Log("Getting Customer Invoice Summary status From Local Server Initiated for itemId :" + custcode + " and invdate: " + invdate + " --GetCustomerInvoiceSummary()");
                var InvoiceSummary = context.App_Checking_New_GetCustomerInvoiceSummary(custcode, invdate,Invnum);
                response = Request.CreateResponse(HttpStatusCode.OK, InvoiceSummary, "application/json");
                Config.Log("Getting Customer Invoice Summary From Local Server Finished for custcode : " + custcode + " and invdate: " + invdate + " --GetCustomerInvoiceSummary()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of custcode : " + custcode + " and invdate: " + invdate + " from App_GetCustomerInvoiceSummary--GetCustomerInvoiceSummary()");
            }
            return response;
        }

        [HttpPost]
        public void UpdateStartTime(UpdateTime detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating StartTime To Local Server Initiated for custcode :" + detail.custCode + " UpdateStartTime()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.InvDt).ToString("yyyy-MM-dd");
                TimeSpan time = Convert.ToDateTime(detail.time).TimeOfDay;
                context.App_Checking_New_UpdateCheckerStartTime(pickDate, detail.custCode, detail.block, time,detail.Invnum);
                Config.Log("Updating StartTime To Local Server Finished for Cust Code : " + detail.custCode + " App_UpdateCheckerStartTime()");

            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Update StartTime of custcode : " + detail.custCode + " and invoice date: " + pickDate + " from App_UpdateCheckerStartTime()");
            }
        }

        [HttpPost]
        public void InsertCheckerChanges1(CheckingDetail objDetail)
        {
            string invdate = string.Empty;
            try
            {
                Config.Log("Inserting Checker Changes to Local Server Initiated for Cust Code : " + objDetail.CustCode + " --InsertCheckerChanges()");
                SinglaEntities context = new SinglaEntities();
                invdate = Convert.ToDateTime(objDetail.Invdt).ToString("yyyy-MM-dd");
                string Chakerdate = Convert.ToDateTime(objDetail.ChickedDate).ToString("yyyy-MM-dd");
                context.App_Checking_New_InsertAppInvoiceChanges(Convert.ToDateTime(invdate), objDetail.Invnum, objDetail.CustId, objDetail.CustCode, objDetail.CustomerName, objDetail.Itemid, objDetail.Itemcode, objDetail.Itemname, objDetail.Remarks, objDetail.Qty, objDetail.NewQty, objDetail.Batch, objDetail.NewBatch, objDetail.CheckerName, Convert.ToDateTime(Chakerdate), objDetail.Description, objDetail.Isdelete, objDetail.IsStatus, objDetail.NewPsrlno, objDetail.InvType, objDetail.Routename, objDetail.Batchid, objDetail.Id, objDetail.FQty, objDetail.NewFQty, objDetail.MRP, objDetail.Rate);
                Config.Log("Inserting Checker Changes to Local Server Finished for Cust Code : " + objDetail.CustCode + " --InsertInvoiceSummary()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Insert Checker Changes of Cust Code : " + objDetail.CustCode + " and invdate: " + invdate + " from App_InsertCheckerChanges--InsertCheckerChanges()");
            }
        }

        [HttpPost]
        public void InsertCheckerChanges(CheckingDetail objDetail)
        {
            string invdate = string.Empty;
            try
            {
                var con = ConfigurationManager.ConnectionStrings["masterDb"].ToString();
                using (SqlConnection dbConnection = new SqlConnection(con))
                {
                    InsertAppInvoiceChanges(objDetail, dbConnection);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Insert Checker Changes of Cust Code : " + objDetail.CustCode + " and invdate: " + invdate + " from App_InsertCheckerChanges--InsertCheckerChanges()");
            }
        }


        public string InsertAppInvoiceChanges(CheckingDetail invtMaster, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                if (invtMaster != null)
                {
                    myConnection.Open();
                    int InvNum = GetItemCheck(invtMaster, myConnection);
                    if (InvNum==0)
                    {
                        try
                        {
                            string sql = "Insert into AppInvoiceChanges(Invdate,Invnum,custid,custcode,customername,itemid,itemcode,itemname,Remarks,oldqty,newqty,oldbatch,newbatch,Checkername,changedatetime,Description,IsDelete,IsStatus,NewPsrlno,InvType,Routename,OldPsrlno,Id,FQty,NewFQty,Mrp,Rate)values(@Invdate, @Invnum, @custid, @custcode, @customername, @itemid, @itemcode, @itemname, @Remarks, @oldqty, @newqty, @oldbatch, @newbatch, @Checkername, @changedatetime, @Description, @IsDelete, @IsStatus, @NewPsrlno, @invType, @RouteName, @OldPsrlno, @Id, @FQty, @NewFQty, @MRP, @Rate)";
                            SqlCommand cmd = new SqlCommand(sql, myConnection);
                            cmd.Parameters.Add("@Invdate", SqlDbType.Date).Value = invtMaster.Invdt;
                            cmd.Parameters.Add("@Invnum", SqlDbType.Int).Value = invtMaster.Invnum;
                            cmd.Parameters.Add("@custid", SqlDbType.Int).Value = invtMaster.CustId;
                            cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = invtMaster.CustCode;
                            cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = invtMaster.CustomerName;
                            cmd.Parameters.Add("@itemid", SqlDbType.Int).Value = invtMaster.Itemid;
                            cmd.Parameters.Add("@itemcode", SqlDbType.VarChar).Value = invtMaster.Itemcode;
                            cmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = invtMaster.Itemname;
                            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = invtMaster.Remarks;
                            cmd.Parameters.Add("@oldqty", SqlDbType.VarChar).Value = invtMaster.Qty;
                            cmd.Parameters.Add("@newqty", SqlDbType.VarChar).Value = invtMaster.NewQty;
                            cmd.Parameters.Add("@oldbatch", SqlDbType.VarChar).Value = invtMaster.Batch;
                            cmd.Parameters.Add("@newbatch", SqlDbType.VarChar).Value = invtMaster.NewBatch;
                            cmd.Parameters.Add("@Checkername", SqlDbType.VarChar).Value = invtMaster.CheckerName;
                            cmd.Parameters.Add("@changedatetime", SqlDbType.Date).Value = invtMaster.ChickedDate;
                            cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = invtMaster.Description;
                            cmd.Parameters.Add("@IsDelete", SqlDbType.VarChar).Value = invtMaster.Isdelete;
                            cmd.Parameters.Add("@IsStatus", SqlDbType.Int).Value = invtMaster.IsStatus;
                            cmd.Parameters.Add("@NewPsrlno", SqlDbType.Int).Value = invtMaster.NewPsrlno;
                            cmd.Parameters.Add("@InvType", SqlDbType.VarChar).Value = invtMaster.InvType;
                            cmd.Parameters.Add("@Routename", SqlDbType.VarChar).Value = invtMaster.Routename;
                            cmd.Parameters.Add("@OldPsrlno", SqlDbType.Int).Value = invtMaster.Batchid;
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = invtMaster.Id;
                            cmd.Parameters.Add("@FQty", SqlDbType.Int).Value = invtMaster.FQty;
                            cmd.Parameters.Add("@NewFQty", SqlDbType.Int).Value = invtMaster.NewFQty;
                            cmd.Parameters.Add("@Mrp", SqlDbType.Decimal).Value = invtMaster.MRP;
                            cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = invtMaster.Rate;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            response = "success";
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            string sql = "Update AppInvoiceChanges set newqty=@newqty,newbatch=@newbatch,Remarks=@Remarks,FQty=@FQty,NewFQty=@NewFQty where Invdate=@Invdate and Invnum=@Invnum and custcode=@custcode and itemid=@itemid  and Description=@Description and Id=@Id and Mrp=@MRP and Rate=@Rate and newbatch=@newbatch";
                            SqlCommand cmd = new SqlCommand(sql, myConnection);
                            cmd.Parameters.Add("@newqty", SqlDbType.VarChar).Value = invtMaster.NewQty;
                            cmd.Parameters.Add("@newbatch", SqlDbType.VarChar).Value = invtMaster.NewBatch;
                            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = invtMaster.Remarks;
                            cmd.Parameters.Add("@FQty", SqlDbType.Decimal).Value = invtMaster.FQty;
                            cmd.Parameters.Add("@NewFQty", SqlDbType.Int).Value = invtMaster.NewFQty;
                            cmd.Parameters.Add("@Invdate", SqlDbType.Date).Value = invtMaster.Invdt;
                            cmd.Parameters.Add("@Invnum", SqlDbType.Int).Value = invtMaster.Invnum;
                            cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = invtMaster.CustCode;
                            cmd.Parameters.Add("@itemid", SqlDbType.Int).Value = invtMaster.Itemid;
                            cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = invtMaster.Description;
                            cmd.Parameters.Add("@Mrp", SqlDbType.Decimal).Value = invtMaster.MRP;
                            cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = invtMaster.Rate;
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = invtMaster.Id;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            response = "success";
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            myConnection.Close();
            return response;
        }

        public int GetItemCheck(CheckingDetail invtMaster, SqlConnection myConnection)
        {
            int response = 0;
            try
            {
                string sql = "select Invnum from AppInvoiceChanges where Invdate='"+ invtMaster.Invdt.ToString("yyyy-MM-dd") + "' and  Invnum='" + invtMaster.Invnum + "' and custcode='" + invtMaster.CustCode + "' and itemid='" + invtMaster.Itemid + "' and newbatch='" + invtMaster.NewBatch + "'  and Description='" + invtMaster.Description + "' and Id='" + invtMaster.Id + "' ";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                response = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            return response;
        }


        [HttpPost]
        public void UpdateDeleteStatus(UpdateQty detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating Delete Status to Local Server Initiated for Item Id :" + detail.itemId + " --UpdateDeleteStatus()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.Invdt).ToString("yyyy-MM-dd");
                context.App_Checking_New_UpdateItemDeleteStatus(pickDate, detail.custCode.ToString(), detail.itemId, detail.Batchid.ToString(), detail.NewQuenty, detail.Invnum,detail.CheckedTime,detail.Id);
                Config.Log("Updating Delete status To Local Server Finished for Item Id : " + detail.itemId + " App_UpdateItemDeleteStatus --UpdateDeleteStatus()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to update delete status of Item Id : " + detail.itemId + " and invoice date: " + pickDate + " from App_UpdateItemDeleteStatus--UpdateDeleteStatus()");
            }
        }

        [HttpPost]
        public void UpdateBatchStatus(UpdateQty detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating Batch Status to Local Server Initiated for Item Id :" + detail.itemId + " --UpdateBatchStatus()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.Invdt).ToString("yyyy-MM-dd");
                context.App_Checking_New_UpdateItemBatchStatus(pickDate, detail.custCode.ToString(), detail.itemId, detail.Batchid.ToString(), detail.NewQuenty, detail.Invnum, detail.CheckedTime, detail.Id,detail.NewBatchid,detail.NewBatch,detail.BatchQty,detail.ManualBatch);
                Config.Log("Updating Batch status To Local Server Finished for Cust Code : " + detail.custCode.ToString() + " App_UpdateItemBatchStatus --UpdateBatchStatus()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to update Batch of Item Id : " + detail.itemId + " and invoice date: " + pickDate + " from App_UpdateItemBatchStatus--UpdateBatchStatus()");
            }
        }

        [HttpPost]
        public void InsertUnpickedChanges(UnpickedDetail objDetail)
        {
            string invdate = string.Empty;
            try
            {
               
                Config.Log("Inserting Unpicked Changes to Local Server Initiated for Cust Code : " + objDetail.CustCode + " --InsertUnpickedChanges()");
                SinglaEntities context = new SinglaEntities();
                invdate = Convert.ToDateTime(objDetail.Invdt).ToString("yyyy-MM-dd");
                context.App_Checking_New_InsertUnPickedItems(objDetail.CustCode, objDetail.CustomerName, Convert.ToDateTime(invdate), objDetail.Qty, objDetail.CustId, objDetail.Itemname, objDetail.Pack, objDetail.Location, objDetail.Status, objDetail.NewQty, objDetail.Invnum, objDetail.Itemid, objDetail.Batchid, objDetail.Batch, objDetail.expiry, Convert.ToDecimal(objDetail.mrp), objDetail.itemcode, objDetail.routeid, objDetail.route, objDetail.Block, objDetail.checkername, objDetail.CheckerId, objDetail.Phoneid, objDetail.PickerName);
                Config.Log("Inserting Unpicked Changes to Local Server Finished for Cust Code : " + objDetail.CustCode + " and invdate: " + invdate + " --InsertInvoiceSummary()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Insert UnPicked Changes of Cust Code : " + objDetail.CustCode + " and invdate: " + invdate + " from App_InsertUnPickedItems--InsertUnpickedChanges()");
            }
        }

        [HttpPost]
        public void UpdateUnpick(UpdateQty detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating Unpick To Local Server Initiated for Item :" + detail.itemId + " --UpdateUnpick()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.Invdt).ToString("yyyy-MM-dd");
                context.App_Checking_New_UpdateUnpick(pickDate, detail.custCode.ToString(), detail.itemId, detail.Batchid.ToString(), detail.Invnum);
                Config.Log("Updating Qty To Local Server Finished for Cust Code : " + detail.custCode.ToString() + " App_UpdateUnpick --UpdateChickingDetail()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Update Unpick of Item Id : " + detail.itemId + " and invoice date: " + pickDate + " from App_UpdateItemQty--UpdateUnpick()");
            }
        }

        [HttpPost]
        public void UpdateChickingDetail(UpdatePickedDetail detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating Checking Detail To Local Server Initiated for customer code :" + detail.custCode.ToString() + " --UpdateChickingDetail()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.InvDt).ToString("yyyy-MM-dd");
                context.App_Checking_New_UpdateChickingDetail(pickDate, detail.custCode.ToString(), detail.block.ToString(),detail.Invnum);
                Config.Log("Updating Checking Detail To Local Server Finished for Cust Code :" + detail.custCode.ToString() + " --UpdateChickingDetail()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to update data of Customer Code : " + detail.custCode.ToString() + " and invoice date: " + pickDate + " from App_UpdateChickingDetail--UpdateChickingDetail()");
            }
        }

        [HttpPost]
        public void UpdateEndTime(UpdateTime detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating Qty To Local Server Initiated for Item :" + detail.custCode + " --UpdateEndTime()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.InvDt).ToString("yyyy-MM-dd");
                TimeSpan time = Convert.ToDateTime(detail.time).TimeOfDay;
                context.App_Checking_New_UpdateCheckerEndTime(pickDate, detail.custCode.ToString(), detail.block, time, detail.Invnum);
                Config.Log("Updating EndTime To Local Server Finished for Cust Code : " + detail.custCode.ToString() + "App_UpdateCheckerEndTime()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Update EndTime of custcode : " + detail.custCode + " and invoice date: " + pickDate + " from App_UpdateCheckerEndTime()");
            }
        }

        [HttpPost]
        public void InsertInvoiceSummary(InvoiceSummary objDetail)
        {
            string invdate = string.Empty;
            try
            {
                Config.Log("Inserting invoice summary to Local Server Initiated for Cust Code : " + objDetail.custcode + " --InsertInvoiceSummary()");
                SinglaEntities context = new SinglaEntities();
                invdate = Convert.ToDateTime(objDetail.Invdate).ToString("yyyy-MM-dd");
                context.App_Checking_New_InsertInvoiceSummary(Convert.ToDateTime(invdate), objDetail.checkername, objDetail.routename, objDetail.custcode, objDetail.custname, objDetail.Totalines, objDetail.pickedlines, objDetail.checkedlines, objDetail.pendinglines, objDetail.batchchanges, objDetail.qtychangelines, objDetail.nillines, objDetail.Noofboxes, objDetail.Noofpackets,objDetail.InvType, objDetail.Invnum);
                Config.Log("Inserting invoice summary to Local Server Finished for Cust Code : " + objDetail.custcode + " --InsertInvoiceSummary()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Insert invoice summary of Cust Code : " + objDetail.custcode + " and invdate: " + invdate + " from App_InsertInvoiceSummary--InsertInvoiceSummary()");
            }
        }

        [HttpPost]
        public void InsertsaleAutoMod(UpdatePickedDetail detail)
        {
            DateTime invdate;
            try
            {
                Config.Log("Inserting Checker Changes to Local Server Initiated for Cust Code : " + detail.custCode + " --InsertCheckerChanges()");
                SinglaEntities context = new SinglaEntities();
                invdate = Convert.ToDateTime(detail.InvDt.ToString("yyyy-MM-dd"));

                var InvChangeList = context.App_Checking_New_InsertInvoiceChangesToSaleAutomode(detail.custCode, invdate,detail.Invnum);
                Config.Log("Inserting Checker Changes to Local Server Finished for Cust Code : " + detail.custCode + " --InsertInvoiceSummary()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Insert Checker Changes of Cust Code : " + detail.custCode + " and invdate: " + detail.InvDt.ToString() + " from App_InsertCheckerChanges--InsertCheckerChanges()");
            }
        }

        [HttpPost]
        public void UpdateQty(UpdateQty detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating Qty To Local Server Initiated for Item :" + detail.itemId + " --UpdateQty()");
                SinglaEntities context = new SinglaEntities();
                pickDate = Convert.ToDateTime(detail.Invdt).ToString("yyyy-MM-dd");
                context.App_Checking_New_UpdateItemQty(pickDate, detail.custCode.ToString(), detail.itemId, detail.Batchid.ToString(), detail.NewQuenty, detail.Invnum,detail.CheckedTime,detail.Id, detail.NewFQty);
                Config.Log("Updating Qty To Local Server Finished for Cust Code : " + detail.custCode.ToString() + " App_UpdateItemQty --UpdateChickingDetail()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                Exceptions.HandleContentException(ex, "Unable to Update Qty of Item Id : " + detail.itemId + " and invoice date: " + pickDate + " from App_UpdateItemQty--UpdateQty()");
            }
        }

        [HttpPost]
        public void UpdatePickingDetail(UpdateQty objDetail)
        {
            try
            {
                SinglaEntities context = new SinglaEntities();
                string invdate = string.Empty;
                try
                {
                    invdate = Convert.ToDateTime(objDetail.Invdt).ToString("yyyy-MM-dd");
                    Config.Log("Updating Picked Detail list To Local Server Initiated for Cust Code :" + objDetail.custCode.ToString() + " and invdate: " + invdate + " --UpdatePickingDetail()");
                    context.App_Checking_New_UpdateCheckList(invdate, objDetail.custCode.ToString(), objDetail.itemId.ToString(), objDetail.Batchid.ToString(), Convert.ToDecimal(objDetail.NewQuenty), objDetail.Checked, objDetail.Invnum.ToString(), objDetail.CheckedTime, objDetail.Id, objDetail.NewFQty);
                    Config.Log("Updating Picked Detail list To Local Server Finished for Cust Code :" + objDetail.custCode.ToString() + " --UpdatePickingDetail()");
                }
                catch (Exception ex)
                {
                    Exceptions.SetLogCategory(Config.Category.InvoiceManagerApiError);
                    Exceptions.HandleContentException(ex, "Unable to update data for Cust Code : " + objDetail.custCode.ToString() + " and invoice date: " + invdate + " from App_UpdatePickList--UpdatePickingDetail()");
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
