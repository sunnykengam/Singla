using Checking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Checking.ViewModel
{
    public static class GetAPIData
    {
        static string url=AppSettings.IPAddress;
        ////public static string serviceUrl = "http://" + url + "/CheckingAppTest/api/";
       //public static string serviceUrl = "http://192.168.1.92/CheckingApp/api/";

       public static string serviceUrl = "http://192.168.2.9/SinglaChecking/api/";
        private static readonly HttpClient _client = new HttpClient(); 

        public static UserLogin Getuserdetails(string username)
        {
            string Url =  serviceUrl + "invoice/GetUserDetail?CheckerId=" + username;
            UserLogin userdetails = new UserLogin();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                AppSettings.UserDetailJson = content;
                userdetails = JsonConvert.DeserializeObject<UserLogin>(content.Replace("[", string.Empty).Replace("]", string.Empty));
            }
            catch (Exception ex)
            {
            }
            return userdetails;
        }

        public static string GetEdpStatus(string custcode, string invdate,string Invnum)
        {
            string content = null;
            string Url = serviceUrl + "Invoice/GetEDPStatus?custcode=" + custcode + "&invdate=" + invdate + "&Invnum=" + Invnum;
            try
            {
                content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                content = Regex.Replace(content, "\"", "");
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public static string GetInvoiceNuminAppInvoiceChanges(string custcode, string invdate,string Invnum)
        {
            string content = null;
            string Url = serviceUrl + "Invoice/GetInvoiceNuminAppInvoiceChanges?custcode=" + custcode + "&invdate=" + invdate + "&Invnum=" + Invnum;
            try
            {
                content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                content = Regex.Replace(content, "\"", "");
                if (content.StartsWith("["))
                {
                    content = content.Substring(1, content.Length - 1);
                }
                if (content.EndsWith("]"))
                {
                    content = content.Substring(0, content.Length - 1);
                }
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public static string GetInvoiceNumbers(string custcode, string invdate,string Invnum)
        {
            string content = null;
            string Url = serviceUrl + "Invoice/GetInvoiceNumbers?custcode=" + custcode + "&invdate=" + invdate + "&Invnum=" + Invnum;
            try
            {
                content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                content = Regex.Replace(content, "\"", "");
                if (content.StartsWith("["))
                {
                    content = content.Substring(1, content.Length - 1);
                }
                if (content.EndsWith("]"))
                {
                    content = content.Substring(0, content.Length - 1);
                }
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        public static ObservableCollection<PickedDetail> GetPickedDetails(string CheckerId)
        {
            string Url = serviceUrl + "Invoice/GetPickedDetails?CheckerId=" + CheckerId + "&InvDate=" + AppSettings.InvoiceDate;
            ObservableCollection<PickedDetail> invoiceslist = new ObservableCollection<PickedDetail>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                invoiceslist = JsonConvert.DeserializeObject<ObservableCollection<PickedDetail>>(content);
            }
            catch (Exception ex)
            {   
            }
            return invoiceslist;
        }   

        public static int GetCheckingHoldCount(string Block, string custcode,string Invnum)
        {
            int holdlst = 0;
            string Url = serviceUrl + "invoice/GetCheckingHoldCount?Custcode=" + custcode + "&InvDt=" + AppSettings.InvoiceDate + "&Block=" + Block + "&Invnum="+ Invnum;
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                holdlst = Convert.ToInt32(content);
            }
            catch (Exception ex)
            {
            }
            return holdlst;
        }

        public static ObservableCollection<InvoiceItem> GetBatchwiseItem(string custcode,DateTime Invdt,int itemid,string Batch,string Invnum)
        {
            string Url = serviceUrl + "Invoice/GetBatchWiseItem?Custcode=" + custcode.Trim() + "&InvDt=" + Invdt.ToString("yyyy-MM-dd") + "&Itemid=" + itemid+ "&Batch=" + Batch + "&Invnum=" + Invnum;
            ObservableCollection<InvoiceItem> invoiceslist = new ObservableCollection<InvoiceItem>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                invoiceslist = JsonConvert.DeserializeObject<ObservableCollection<InvoiceItem>>(content);
            }
            catch (Exception ex)
            {
            }
            return invoiceslist;
        }

        public static BatchIdsByItemId GetBatchwiseMrpandRate(string Batch,  string Exp, string Mrp)
        {
            string Url = serviceUrl + "Invoice/GetBatchMrpExpWiseRate?Batch=" + Batch + "&MRP=" + Mrp + "&Exp=" + Exp ;
            BatchIdsByItemId batchlist = new BatchIdsByItemId();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                batchlist = JsonConvert.DeserializeObject<BatchIdsByItemId>(content);
            }
            catch (Exception ex)
            {
            }
            return batchlist;
        }
        

        public static ObservableCollection<InvoiceItem> Getinvoicelist(string Invnum)
        {
            string Url = serviceUrl + "invoice/GetInvoiceDetails?Custcode=" + AppSettings.CustCode + "&InvDt=" + AppSettings.InvoiceDate + "&Block=" + AppSettings.Block + "&Invnum=" + Invnum;
            ObservableCollection<InvoiceItem> invoiceslist = new ObservableCollection<InvoiceItem>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                invoiceslist = JsonConvert.DeserializeObject<ObservableCollection<InvoiceItem>>(content);
            }
            catch (Exception ex)
            {
            }
            return invoiceslist;
        }

        public static async void UpdateCheckerStartTime(string starttime,string Invnum)
        {
            string Url = serviceUrl + "Invoice/UpdateStartTime/";
            UpdatePickedDetail up = new UpdatePickedDetail();
            up.custCode = AppSettings.CustCode;
            up.InvDt = AppSettings.InvoiceDate;
            up.block = AppSettings.Block;
            up.Invnum = Invnum;
            up.time = starttime;
            try
            {
                string content = JsonConvert.SerializeObject(up);
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public static async void InsertInvoiceChanges(CheckingDetail Checkdetail)
        {
            string Url = serviceUrl + "Invoice/InsertCheckerChanges/";
            try
            {
                if (string.IsNullOrEmpty(Checkdetail.NewQty))
                    Checkdetail.NewQty = "0";
                string content = JsonConvert.SerializeObject(Checkdetail); 
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdateDeleteStatus(UpdateQty Checkdetail)
        {
            string Url = serviceUrl + "Invoice/UpdateDeleteStatus/";
            ObservableCollection<CheckingDetail> pickinglist = new ObservableCollection<CheckingDetail>();
            try
            {
                string content = JsonConvert.SerializeObject(Checkdetail); 
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdateBatchStatus(UpdateQty Checkdetail)
        {
            string Url = serviceUrl + "Invoice/UpdateBatchStatus/";
            try
            {
                string content = JsonConvert.SerializeObject(Checkdetail); 
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdatenewBatchStatus(UpdateBatchStatus Checkdetail)
        {
            string Url = serviceUrl + "Invoice/UpdateNewBatchStatus/";
            try
            {
                string content = JsonConvert.SerializeObject(Checkdetail); 
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void InsertUnpickedChanges(UnpickedDetail pickeddetail)
        {
            string Url = serviceUrl + "Invoice/InsertUnpickedChanges/";
            try
            {
                string content = JsonConvert.SerializeObject(pickeddetail); 
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdateUnpickChange(UpdateQty Checkdetail)
        {
            string Url = serviceUrl + "Invoice/UpdateUnpick/";
            try
            {
                string content = JsonConvert.SerializeObject(Checkdetail);
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdateCkickingDetail(string Invnum)
        {
            string Url = serviceUrl + "Invoice/UpdateChickingDetail/";
            UpdatePickedDetail up = new UpdatePickedDetail();
            up.custCode = AppSettings.CustCode;
            up.InvDt = AppSettings.InvoiceDate;
            up.block = AppSettings.Block;
            up.Invnum = Invnum;
            try
            {
                string content = JsonConvert.SerializeObject(up);
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public static InvoiceStatus GetInvoiceStatus(string CustCode, DateTime InvDate,string Invnum)
        {
            string Url = serviceUrl + "Invoice/GetInvoiceStatus?Custcode=" + CustCode + "&invdate=" + AppSettings.InvoiceDate + "&Invnum=" + Invnum;
            InvoiceStatus Batchlist = new InvoiceStatus();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                Batchlist = JsonConvert.DeserializeObject<InvoiceStatus>(content.Replace("[", string.Empty).Replace("]", string.Empty));
            }
            catch (Exception ex)
            {
            }
            return Batchlist;
        }

        public static async void UpdateCheckerEndTime(string endTime,string Invnum)
        {
            string Url = serviceUrl + "Invoice/UpdateEndTime/";
            UpdatePickedDetail up = new UpdatePickedDetail();
            up.custCode = AppSettings.CustCode;
            up.InvDt = AppSettings.InvoiceDate;
            up.block = AppSettings.Block;
            up.Invnum = Invnum;
            up.time = endTime;
            try
            {
                string content = JsonConvert.SerializeObject(up);
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public static SummaryDetail GetInvoiceSummary(string CustCode,string Invnum)
        {
            string Url = serviceUrl + "Invoice/GetCustomerInvoiceSummary?custcode=" + CustCode + "&invdate=" + AppSettings.InvoiceDate + "&Invnum=" + Invnum;
            SummaryDetail Batchlist = new SummaryDetail();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                Batchlist = JsonConvert.DeserializeObject<SummaryDetail>(content.Replace("[", string.Empty).Replace("]", string.Empty));
            }
            catch (Exception ex)
            {
            }
            return Batchlist;
        }

        public static async void InsertInvoiceSummary(SummaryDetail summarydetail)
        {
            string Url = serviceUrl + "Invoice/InsertInvoiceSummary/";
            try
            {
                string content = JsonConvert.SerializeObject(summarydetail); 
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void InsertSaleAutoMode(string Invnum)
        {
            string Url = serviceUrl + "Invoice/InsertsaleAutoMod/";
            UpdatePickedDetail up = new UpdatePickedDetail();
            up.custCode = AppSettings.CustCode;
            up.InvDt = AppSettings.InvoiceDate;
            up.Invnum = Invnum;
            try
            {
                string content = JsonConvert.SerializeObject(up);
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdateQtyChange(UpdateQty Checkdetail)
        {

            string Url = serviceUrl + "Invoice/UpdateQty/";
            try
            {
                string content = JsonConvert.SerializeObject(Checkdetail); //Serializes or convert the created Post into a JSON String
                HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdatePickList(UpdateQty Checkdetail)
        {
            string Url = serviceUrl + "Invoice/UpdatePickingDetail/";
            ObservableCollection<CheckingDetail> pickinglist = new ObservableCollection<CheckingDetail>();
            try
            {
                string content = JsonConvert.SerializeObject(Checkdetail); //Serializes or convert the created Post into a JSON String
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public interface IFileHelper
        {
            void closeApplication();
        }
    }
}

