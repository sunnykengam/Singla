using PickList.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PickList.ViewModel
{
    public class GetAPIData
    {
        public static string serviceUrl = "http://192.168.2.9/SinglaPicking/";
      //  public static string serviceUrl = "http://192.168.1.92/PickingApp/";

        //public static string serviceUrl = "http://192.168.0.100/PickingApp/";

        //public static string serviceUrl = "http://183.82.4.45:8080/DeliveryApp/";
        private static readonly HttpClient _client = new HttpClient();

        public static PhoneInfo GetPhoneDetail(string Phoneid)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_GetPhoneInfo?Phoneid=" + Phoneid;
            PhoneInfo phonelist = new PhoneInfo();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                phonelist = JsonConvert.DeserializeObject<PhoneInfo>(content);
            }
            catch (Exception ex)
            {
            }
            return phonelist;
        }

        public static ObservableCollection<CustomerDetail> GetPhoneCustomerDetail(string PhoneCode)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_GetPickingDetailList?Phoneid=" + PhoneCode;
            ObservableCollection<CustomerDetail> pickinglist = new ObservableCollection<CustomerDetail>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                pickinglist = JsonConvert.DeserializeObject<ObservableCollection<CustomerDetail>>(content);
            }
            catch (Exception ex)
            {
            }
            return pickinglist;
        }

        public static ObservableCollection<PickingDetail> GetpickingMasterlist(string PhoneCode, string Custcode, string invdt,string Block,string Invnum)
        {
            string invdate = Convert.ToDateTime(invdt).ToString("yyyy-MM-dd");
            string Url = serviceUrl + "api/Picking/App_Picking_GetPickList?Phoneid=" + PhoneCode + "&invdt=" + invdate + "&custcode=" + Custcode.Trim()+"&Block="+Block + "&Invnum=" + Invnum;
            ObservableCollection<PickingDetail> pickinglist = new ObservableCollection<PickingDetail>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                pickinglist = JsonConvert.DeserializeObject<ObservableCollection<PickingDetail>>(content);
            }
            catch (Exception ex)
            {
            }
            return pickinglist;
        }

        public static async void UpdatePicklist(ObservableCollection<UpdatePickList> pickdetail)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_Update_PickList/";

            try
            {
                string content = JsonConvert.SerializeObject(pickdetail); //Serializes or convert the created Post into a JSON String
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;

            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdateBasketNo(DateTime invdt, string Block, string Custcode, string BasketNo,string Invnum)
        {
            var INV_DT = invdt.ToString("yyyy-MM-dd");
            string Url = serviceUrl + "api/Picking/App_Picking_UpdateBasketNo?invdt=" + INV_DT + "&Block=" + Block + "&CustCode=" + Custcode + "&BasketNo=" + BasketNo + "&Invnum=" + Invnum;
            try
            {
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, null)).Result;
            }
            catch (Exception ex)
            {
            }

        }

        public static async void UpdatePickingDetail(DateTime invdt, string Block,string Custcode,string Invnum)
        {
            var INV_DT = invdt.ToString("yyyy-MM-dd");
            string Url = serviceUrl + "api/Picking/App_Picking_UpdatePickingDetail?invdt=" + INV_DT + "&Block=" + Block + "&CustCode="+ Custcode + "&Invnum=" + Invnum;
            try
            {
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, null)).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public static ObservableCollection<CheckerInfo> GetCheckerDetail(string Invdt)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_CheckerInfo?InvDt=" + Invdt;
            ObservableCollection<CheckerInfo> chickerlist = new ObservableCollection<CheckerInfo>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                chickerlist = JsonConvert.DeserializeObject<ObservableCollection<CheckerInfo>>(content);
            }
            catch (Exception ex)
            {
            }
            return chickerlist;
        }

        public static ObservableCollection<UnPickingList> GetUnpickinglist(string PhoneCode)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_GetUnPickingList?PhoneId=" + PhoneCode;
            ObservableCollection<UnPickingList> pickinglist = new ObservableCollection<UnPickingList>();
            try
            {
                string content = Task.Run(async () => await _client.GetStringAsync(Url)).Result;
                pickinglist = JsonConvert.DeserializeObject<ObservableCollection<UnPickingList>>(content);
            }
            catch (Exception ex)
            {
            }
            return pickinglist;
        }

        public static async void App_Picking_CheckerAssign(CheckerAssign Details)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_CheckerAssign/";
            try
            {
                string content = JsonConvert.SerializeObject(Details);
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result;
            }
            catch (Exception ex)
            {
            }
        }
        public static async void UpdateUnPickingDetail(List<UnPickList> Details)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_UpdateUnPickList/";
            try
            {
                string content = JsonConvert.SerializeObject(Details);
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"))).Result; 
            }
            catch (Exception ex)
            {
            }
        }

        public static async void UpdatePickerStartTime(string starttime,string Invnum)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_UpdatePickerStartTime/";
            UpdatePickedDetail up = new UpdatePickedDetail();
            up.custCode = AppSettings.customerDetail.custcode.Trim();
            up.InvDt = AppSettings.customerDetail.invdt.ToString("yyyy-MM-dd").Trim();
            up.block = AppSettings.customerDetail.Block.Trim();
            up.time = starttime;
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

        public static async void UpdatePickerEndTime(string starttime,string Invnum)
        {
            string Url = serviceUrl + "api/Picking/App_Picking_UpdatePickerEndTime/";
            UpdatePickedDetail up = new UpdatePickedDetail();
            up.custCode = AppSettings.customerDetail.custcode;
            up.InvDt = AppSettings.customerDetail.invdt.ToString("yyyy-MM-dd").Trim();
            up.block = AppSettings.customerDetail.Block;
            up.time = starttime;
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

        public static async void UpdateLoginStatus(string LoginStatus)
        {
            try
            {
                string Url = serviceUrl + "api/Picking/App_Picking_New_UpdatePhoneInfo?Phoneid=" + AppSettings.PhoneCode +"&LoginStatus="+ LoginStatus;
                HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Url, null)).Result;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
