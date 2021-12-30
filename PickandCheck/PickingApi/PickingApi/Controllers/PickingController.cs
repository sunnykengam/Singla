using PickingApi.Models;
using PickingDataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PickingApi.Controllers
{
    public class PickingController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage App_Picking_GetPhoneInfo(string Phoneid)
        {
            HttpResponseMessage response = null;

            Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
            using (Esdata_SMPPLEntities entity = new Esdata_SMPPLEntities())
            {
                try
                {
                    Config.Log("Getting CheckerDetails From Local Server Initiated for phone code :" + Phoneid + " --GetCheckerDetail()");

                    var custdetail = context.App_Picking_New_GetPhoneInfo(Phoneid).FirstOrDefault();

                    response = Request.CreateResponse(HttpStatusCode.OK, custdetail, "application/json");
                    if (custdetail == null)
                    {
                        Config.Log("No Details Found for phone code :" + Phoneid + " --GetCheckerDetail()");
                    }
                }
                catch (Exception ex)
                {
                    Exceptions.SetLogCategory(Config.Category.pickingApiError);
                    Exceptions.HandleContentException(ex, "Unable to read data of phone code : " + Phoneid + " from App_CheckerInfo--GetCheckerDetail()");
                }

            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage App_Picking_GetPickingDetailList(string Phoneid)
        {
            HttpResponseMessage response = null;

            Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
            using (Esdata_SMPPLEntities entity = new Esdata_SMPPLEntities())
            {
                try
                {
                    List<CustomerDetail> objDetails = new List<CustomerDetail>();
                    Config.Log("Getting PhoneCustomer Details From Local Server Initiated for phone code :" + Phoneid + " --GetPhoneCustomerDetail()");
                    int i = 1;
                    var custdetail = context.App_Picking_New_GetPickingDetailList(Phoneid.ToString());
                    foreach (App_Picking_New_GetPickingDetailList_Result objdet in custdetail)
                    {
                        CustomerDetail objCustomer = new CustomerDetail();

                        objCustomer.Sno = i++;
                        objCustomer.routeid = objdet.routeid;
                        objCustomer.route = objdet.route;
                        objCustomer.custcode = objdet.custcode;
                        objCustomer.custname = objdet.custname;
                        objCustomer.invdt = Convert.ToDateTime(objdet.invdt);
                        objCustomer.Invnum = Convert.ToInt32(objdet.NoofInvoices);
                        objCustomer.NoofLines = Convert.ToInt32(objdet.NoofLines);
                        objCustomer.pickername = objdet.pickername;
                        objCustomer.checkername = objdet.checkername;
                        objCustomer.phone = objdet.phone;
                        objCustomer.Block = objdet.Block;
                        objCustomer.Status = objdet.Status;
                        objCustomer.custid = Convert.ToInt32(objdet.custid);
                        objCustomer.ChickerId = Convert.ToInt32(objdet.ChickerId);
                        //if (objdet.PickingStatus != null)
                        //    objCustomer.PickingStatus = objdet.PickingStatus.Trim();
                        //else
                        //    objCustomer.PickingStatus = objdet.PickingStatus;
                        objDetails.Add(objCustomer);

                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, objDetails, "application/json");
                    if (custdetail == null)
                    {
                        Config.Log("No Details Found for phone code :" + Phoneid + " --GetPhoneCustomerDetail()");
                    }
                }
                catch (Exception ex)
                {
                    Exceptions.SetLogCategory(Config.Category.pickingApiError);
                    Exceptions.HandleContentException(ex, "Unable to read data of phone code : " + Phoneid + " from App_GetPickingDetailList--GetPhoneCustomerDetail()");
                }

            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage App_Picking_GetPickList(string Phoneid, string invdt, string custcode,string Block,string Invnum)
        {
            HttpResponseMessage response = null;
            Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
            Stopwatch timer = Stopwatch.StartNew();
            timer.Start();
            string invdate = Convert.ToDateTime(invdt).ToString("yyyy-MM-dd");
            List<PickingDetail> objDetails = new List<PickingDetail>();
            try
            {
                int i = 1;
                Config.Log("Getting Master Invoice Details From Local Server Initiated for phone code :" + Phoneid);
                var picklist = context.App_Picking_New_GetPickList(invdate, custcode.Trim().ToString(), Phoneid.ToString(),Block, Invnum);
                
                foreach (App_Picking_New_GetPickList_Result obj in picklist)
                {
                    PickingDetail obj1 = new PickingDetail();
                   
                    obj1.sno = i.ToString();
                    obj1.Itemname = obj.Itemname;
                    obj1.Pack = obj.Pack;
                    obj1.Location = obj.Location;
                    obj1.Picked = obj.Picked;
                    obj1.Itemid = (int)obj.Itemid;
                    obj1.itemcode = obj.itemcode;
                    if (obj.ITEMCATEGORY != null)
                        obj1.ITEMCATEGORY = (int)obj.ITEMCATEGORY;

                    var Invdt = Convert.ToDateTime(obj.Invdt);
                    obj1.Invdt = Convert.ToDateTime(Invdt.ToString("yyyy-MM-dd"));
                    obj1.CustCode = obj.CustCode;
                    obj1.BasketNo = obj.BasketNo;
                    obj1.CustomerName = obj.CustomerName;
                    obj1.Block = obj.Block;
                    obj1.Qty = (int)obj.Qty;
                    obj1.PhoneId = Phoneid;
                    obj1.Invnum = obj.Invnum;
                    obj1.checkername = obj.checkername;
                    obj1.ChickerId = obj.ChickerId;
                    if (obj.Picked == "Y")
                        obj1.Ischecked = true;
                    i++;
                    objDetails.Add(obj1);
                }
                timer.Stop();
                Config.Log("Getting MasterInvoice Details From Local Server Finished of phone code : " + Phoneid + " --GetMasterInvoiceDetails()");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.pickingApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of phone code : " + Phoneid + " from App_GetPickingDetail--GetMasterInvoiceDetails()");
            }
            if (objDetails.Count == 0)
            {
                Config.Log("No Details Found for phone code :" + Phoneid + ",invdate : " + invdate + ",custcode : " + custcode + " --GetMasterInvoiceDetails()");
            }
            response = Request.CreateResponse(HttpStatusCode.OK, objDetails, "application/json");
            return response;
        }

        [HttpPost]
        public void App_Picking_Update_PickList(List<UpdatePickList> objDetails)
        {
            try
            {
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
                foreach (UpdatePickList objDetail in objDetails)
                {
                    string invdate = Convert.ToDateTime(objDetail.Inv_Date).ToString("yyyy-MM-dd");
                    try
                    {
                        Config.Log("Updating records started for Id :" + objDetail.itemId + " --from UpdatePicklist()");
                        TimeSpan time = Convert.ToDateTime(objDetail.PickedTime).TimeOfDay;
                        context.App_Picking_New_UpdatePickList(invdate, objDetail.Cust_Code, objDetail.itemId, objDetail.Picked, objDetail.BasketNo,time,objDetail.Invnum);
                        Config.Log("Update records finished for Id :" + objDetail.itemId + " on invoice date " + invdate + " --from UpdatePicklist()");

                    }
                    catch (Exception ex)
                    {
                        Exceptions.SetLogCategory(Config.Category.pickingApiError);
                        Exceptions.HandleContentException(ex, "updating records failed for ItemId : " + objDetail.itemId + " from App_NewUpdate_PickList--UpdatePicklist()");
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public void App_Picking_UpdateBasketNo(string invdt, string Block, string CustCode, string BasketNo,string Invnum)
        {
            try
            {
                Config.Log("Updating records started for Id : " + CustCode + " --from UpdateBasketNo()");
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();

                string invdate = Convert.ToDateTime(invdt).ToString("yyyy-MM-dd");
                context.App_Picking_New_UpdateBasketNo(invdate, CustCode.ToString(), Block, BasketNo,Invnum);
                Config.Log("Updating records finished for Id : " + CustCode + " --App_UpdatePickingDetail--from ");
            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.pickingApiError);
                Exceptions.HandleContentException(ex, "updating records failed for ItemId : " + CustCode + " from App_UpdatePickingDetail--UpdatePickingDetail()");
            }
        }

        [HttpPost]
        public void App_Picking_UpdatePickingDetail(string invdt, string Block, string CustCode,string Invnum)
        {
            try
            {
                Config.Log("Updating records started for Id : " + CustCode + " --from UpdatePickingDetail()");
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();

                string invdate = Convert.ToDateTime(invdt).ToString("yyyy-MM-dd");
                context.App_Picking_New_UpdatePickingDetail(invdate, CustCode.Trim().ToString(), Block,Invnum);
                Config.Log("Updating records finished for Id : " + CustCode + " --App_UpdatePickingDetail--from UpdatePickingDetail()");

            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.pickingApiError);
                Exceptions.HandleContentException(ex, "updating records failed for ItemId : " + CustCode + " from App_UpdatePickingDetail--UpdatePickingDetail()");
            }
        }

        [HttpGet]
        public HttpResponseMessage App_Picking_CheckerInfo(string invdt)
        {
            HttpResponseMessage response = null;
            Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
            using (Esdata_SMPPLEntities entity = new Esdata_SMPPLEntities())
            {
                try
                {
                    var custdetail = context.App_Picking_New_CheckerInfo(invdt);
                    response = Request.CreateResponse(HttpStatusCode.OK, custdetail, "application/json");
                    if (custdetail == null)
                    {
                    }
                }
                catch (Exception ex)
                {
                    Exceptions.SetLogCategory(Config.Category.pickingApiError);
                    Exceptions.HandleContentException(ex, "Unable to read data of phone code :  from App_CheckerInfo--GetCheckerDetail()");
                }

            }
            return response;
        }
       
        [HttpGet]
        public HttpResponseMessage App_Picking_GetUnPickingList(string PhoneId)
        {
            HttpResponseMessage response = null;

            Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
            using (Esdata_SMPPLEntities entity = new Esdata_SMPPLEntities())
            {
                try
                {
                    Config.Log("Updating UnPicked Detail to Local Server Initiated for phone code :" + PhoneId + " --GetUnPickedDetail()");
                    var custdetails = context.App_Picking_New_GetUnPickingList(PhoneId);

                    response = Request.CreateResponse(HttpStatusCode.OK, custdetails, "application/json");
                    if (custdetails == null)
                    {
                        Config.Log("No Details Found for Phone Id :" + PhoneId + " --GetUnPickedDetail()");
                    }
                    else
                        Config.Log("Updating UnPicked Detail to Local Server Finished for phone code :" + PhoneId + " --GetUnPickedDetail()");
                }
                catch (Exception ex)
                {
                    Exceptions.SetLogCategory(Config.Category.pickingApiError);
                    Exceptions.HandleContentException(ex, "Unable to update data of phone id : " + PhoneId + " from App_GetUnPickingList--GetUnPickedDetail()");
                }

            }
            return response;
        }
        [HttpPost]
        public void App_Picking_UpdateUnPickList(List<UnPickList> unpicklist)
        {
            try
            {

                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
                foreach (UnPickList obj in unpicklist)
                {
                    Config.Log("Updating UnPickingDetail records started for Id : " + obj.itemId + " --from UpdateUnPickingDetail()");
                    try
                    {
                        string invdate = Convert.ToDateTime(obj.Invdt).ToString("yyyy-MM-dd");
                        context.App_Picking_New_UpdateUnPickList(invdate, obj.custCode.ToString(), obj.itemId, obj.Batchid,obj.Invnum);
                        Config.Log("Updating records finished for ItemId : " + obj.itemId + " --App_UpdateUnPickList--from UpdateUnPickingDetail()");
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SetLogCategory(Config.Category.pickingApiError);
                        Exceptions.HandleContentException(ex, "Updating records failed for ItemId : " + obj.itemId + " from App_UpdateUnPickList--UpdateUnPickingDetail()");
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        [HttpPost]
        public void App_Picking_CheckerAssign(CheckerAssign CheckerDetail)
        {
            try
            {
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
                try
                {
                    string invdate = Convert.ToDateTime(CheckerDetail.InvDt).ToString("yyyy-MM-dd");
                    context.App_Picking_CheckingAssign(invdate, CheckerDetail.custCode.ToString(), CheckerDetail.Block, CheckerDetail.Invnum, CheckerDetail.CheckerId, CheckerDetail.CheckerName);
                }
                catch (Exception ex)
                {
                    Exceptions.SetLogCategory(Config.Category.pickingApiError);
                }
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        public void App_Picking_UpdatePickerStartTime(UpdateTime detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating StartTime To Local Server Initiated for custcode :" + detail.custCode + " UpdateStartTime()");
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
                pickDate = Convert.ToDateTime(detail.InvDt).ToString("yyyy-MM-dd");
                TimeSpan time = Convert.ToDateTime(detail.time).TimeOfDay;
                context.App_Picking_New_UpdatePickerStartTime(pickDate, detail.custCode, detail.block, time,detail.Invnum);
                Config.Log("Updating StartTime To Local Server Finished for Cust Code : " + detail.custCode + " App_UpdateCheckerStartTime()");

            }
            catch (Exception ex)
            {
                Exceptions.HandleContentException(ex, "Unable to Update StartTime of custcode : " + detail.custCode + " and invoice date: " + pickDate + " from App_UpdateCheckerStartTime()");
            }
        }

        [HttpPost]
        public void App_Picking_UpdatePickerEndTime(UpdateTime detail)
        {
            string pickDate = string.Empty;
            try
            {
                Config.Log("Updating StartTime To Local Server Initiated for custcode :" + detail.custCode + " UpdateStartTime()");
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
                pickDate = Convert.ToDateTime(detail.InvDt).ToString("yyyy-MM-dd");
                TimeSpan time = Convert.ToDateTime(detail.time).TimeOfDay;
                context.App_Picking_New_UpdatePickerEndTime(pickDate, detail.custCode, detail.block, time, detail.Invnum);
                Config.Log("Updating StartTime To Local Server Finished for Cust Code : " + detail.custCode + " App_UpdateCheckerStartTime()");

            }
            catch (Exception ex)
            {
                Exceptions.HandleContentException(ex, "Unable to Update StartTime of custcode : " + detail.custCode + " and invoice date: " + pickDate + " from App_UpdateCheckerStartTime()");
            }
        }

        [HttpPost]
        public void App_Picking_New_UpdatePhoneInfo(string Phoneid,string LoginStatus)
        {
            try
            {
                Config.Log("Update AppBlockerUsers From LoginStatus  Initiated for phone code :" + Phoneid + " --GetCheckerDetail()");
                Esdata_SMPPLEntities context = new Esdata_SMPPLEntities();
                context.App_Picking_New_UpdatePhoneInfo(Phoneid, LoginStatus);

            }
            catch (Exception ex)
            {
                Exceptions.SetLogCategory(Config.Category.pickingApiError);
                Exceptions.HandleContentException(ex, "Unable to read data of phone code : " + Phoneid + " from App_CheckerInfo--GetCheckerDetail()");
            }
        }
    }
}
