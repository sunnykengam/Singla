using MvvmHelpers;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using SinglaApp.Model.Company;
using SinglaApp.Model.Reports;
using Xamarin.Forms;
using Plugin.Connectivity;
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.ViewModel
{
    public class ItemsDatabase
    {
        private const string AppUrl = "https://singlaappsyncapi.azurewebsites.net/api/";
        private const string AppOrdersUrl = "https://singlaordersyncapi.azurewebsites.net/api/";
        private const string ReportsUrl = "https://singlareports.azurewebsites.net/api/";
        private readonly HttpClient _client = new HttpClient();
        private ObservableCollection<ItemMaster> _posts;
        readonly SQLiteAsyncConnection database;
        public static string serviceUrl = "https://vasuotifservice.azurewebsites.net/api/";
        public ObservableCollection<ItemMaster> productList = new ObservableCollection<ItemMaster>();

        public ItemsDatabase()
        {
            try
            {
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async void InsertLocalToAppSettings()
        {
            try
            {
                AppSettings.AllItemdeatial = Task.Run(async () => await database.Table<ItemMaster>().ToListAsync()).Result;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--InsertLocalToAppSettings--Error : " + AppSettings.salesmancode + "--" +
                                                         ex.Message);
            }
        }

        public async void ItemMasterinsertDB()
        {
            if (AppSettings.ProductssyncStatus == false)
            {
                try
                {
                    AppSettings.ProductssyncStatus = true;
                    List<ItemMaster> invoicedetails = new List<ItemMaster>();
                    string Baseurl = AppUrl + "DataSync/GetProducts?partnercode=" + AppSettings.partnercode + "";
                    string content = _client.GetStringAsync(Baseurl).Result;
                    invoicedetails = JsonConvert.DeserializeObject<List<ItemMaster>>(content);
                    if (invoicedetails.Count != 0)
                    {
                        var DATA = database.QueryAsync<ItemMaster>("Delete from ItemMaster").Result;
                        var productdata = Task.Run(async () => await database.InsertAllAsync(invoicedetails)).Result;
                        AppSettings.AllItemdeatial = null;
                        AppSettings.AllItemdeatial = invoicedetails;
                    }
                }
                catch (Exception ex)
                {
                    Analytics.TrackEvent("ItemsDatabase--ItemMasterinsertDB--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                }
                AppSettings.ProductssyncStatus = false;
            }
        }
    
        public List<CustomerMaster> GetCustLoginCheck(string UserName, string Password)
        {
            List<CustomerMaster> customer = new List<CustomerMaster>();
            try
            {
                AppSettings.customerdetail = null;
                string Baseurl = AppUrl + "DataSync/GetCustLogin?Username=" + UserName + "&Password=" + Password;
                string content = _client.GetStringAsync(Baseurl).Result;
                AppSettings.CustomerListjson = content;
                customer = JsonConvert.DeserializeObject<List<CustomerMaster>>(content);
                if (customer.Count != 0)
                {
                    AppSettings.CustomerListjson = content;
                    var DATA = database.QueryAsync<CustomerMaster>("Delete from CustomerMaster").Result;
                    var productdata = Task.Run(async () => await database.InsertAllAsync(customer)).Result;
                }
            }
            catch (Exception ex)
            {
            }
            return customer;
        }

        public List<SalesmanMaster> GetSalesLoginCheck(string UserName, string Password)
        {
            List<SalesmanMaster> salesman = new List<SalesmanMaster>();
            try
            {
                AppSettings.customerdetail = null;
                string Baseurl = AppUrl + "DataSync/GetsalesLogin?Username=" + UserName + "&Password=" + Password;
                string content = _client.GetStringAsync(Baseurl).Result;
                AppSettings.CustomerListjson = content;
                salesman = JsonConvert.DeserializeObject<List<SalesmanMaster>>(content);
                if (salesman.Count != 0)
                {
                    AppSettings.CustomerListjson = content;
                    var DATA = database.QueryAsync<SalesmanMaster>("Delete from SalesmanMaster").Result;
                    var productdata = Task.Run(async () => await database.InsertAllAsync(salesman)).Result;
                }
            }
            catch (Exception ex)
            {
            }
            return salesman;
        }

        public void CustomerMasterDB()
        {
            if (AppSettings.CustomersyncStatus == false)
            {
                AppSettings.CustomersyncStatus = true;
                try
                {
                    ObservableCollection<CustomerMaster> customers = new ObservableCollection<CustomerMaster>();
                    AppSettings.customerdetail = null;
                    string Baseurl = AppUrl + "DataSync/GetAllCustomers?salesmanid=" + AppSettings.salesmanid + "&partnercode="+AppSettings.partnercode;
                    string content = _client.GetStringAsync(Baseurl).Result;
                  
                    customers = JsonConvert.DeserializeObject<ObservableCollection<CustomerMaster>>(content);
                    if (customers.Count != 0)
                    {
                        AppSettings.customerdetail = customers;
                        var DATA = database.QueryAsync<CustomerMaster>("Delete from CustomerMaster").Result;
                        var productdata = Task.Run(async () => await database.InsertAllAsync(customers)).Result;
                    }

                }
                catch (Exception ex)
                {
                    Analytics.TrackEvent("ItemsDatabase--CustomerMasterDB--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                }
                AppSettings.CustomersyncStatus = false;
            }
        }
        public async void AppOrdersInsertdata()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    List<OrdersErrorLog> Error_logs = new List<OrdersErrorLog>();
                    List<Orders> total_Orders = new List<Orders>();
                    List<updateOrders> ord = new List<updateOrders>();
                    string OrderJson = string.Empty;
                    var localordlist = Task.Run(async () => await database.Table<Orders>().Where(i => i.order_id == null && i.AzureStatus == "Complete.").ToListAsync()).Result;
                    if (localordlist.Count != 0)
                    {
                        foreach (Orders itemorder in localordlist)
                        {
                            itemorder.appversion = DependencyService.Get<IFileHelper>().Version;
                            if (AppSettings.loginname == "Salesman Login")
                            {
                                itemorder.ORDERFROM = "S";
                            }
                            else
                            {
                                itemorder.ORDERFROM = "C";
                            }
                            itemorder.ordupldtime = DateTime.Now.ToString("HH:mm:ss");
                            var orderitemlist = Task.Run(async () => await database.Table<orderitems>().Where(i => i.OrderGuid == itemorder.OrderGuid).ToListAsync()).Result;
                            itemorder.orderitemlist.AddRange(orderitemlist);
                            total_Orders.Add(itemorder);
                        }
                        try
                        {
                            string Baseurl = AppOrdersUrl + "OrderManagement/InsertOrdersLocalToAzureDB/";
                            OrderJson = JsonConvert.SerializeObject(total_Orders);
                            Analytics.TrackEvent("OrderJson : " + AppSettings.salesmancode + "--" + OrderJson);
                            HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Baseurl, new StringContent(OrderJson, Encoding.UTF8, "application/json"))).Result;
                            string data = response.Content.ReadAsStringAsync().Result;
                            ord = JsonConvert.DeserializeObject<List<updateOrders>>(data);
                            Analytics.TrackEvent("OrderReturnJson : " + AppSettings.salesmancode + "--" + OrderJson);
                            foreach (updateOrders obj in ord)
                            {
                                string qry = "update Orders set order_id='" + obj.order_id + "',ordupldtime='" + obj.ordupldtime + "' where OrderGuid ='" + obj.OrderGuid + "'";
                                var DATA = Task.Run(async () => await database.QueryAsync<Orders>(qry)).Result;
                            }
                        }
                        catch (Exception ex)
                        {
                            Analytics.TrackEvent("ItemsDatabase--AppOrdersInsertdata--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                            try
                            {
                                OrdersErrorLog orderror = new OrdersErrorLog();
                                orderror.OrderJson = OrderJson;
                                orderror.partnercode = total_Orders[0].partnercode;
                                orderror.Sman = total_Orders[0].Sman;
                                orderror.order_date = total_Orders[0].order_date;
                                orderror.ErrorMessage = ex.Message;
                                var order = Task.Run(async () => await database.InsertAsync(orderror)).Result;
                            }
                            catch (Exception ex1)
                            {
                                Analytics.TrackEvent("ItemsDatabase--AppOrdersInsertdata--OrdersErrorLog--Error : " + AppSettings.salesmancode + "--" + ex1.Message);
                            }
                        }
                        total_Orders.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--AppOrdersInsertdata--CompleteError : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            AppSettings.UpdateazureDB = false;
        }

        public async void AppOrdersErrorLogInsertdata()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    List<OrdersErrorLog> Error_logs = new List<OrdersErrorLog>();
                    string OrderJson = string.Empty;
                    Error_logs = Task.Run(async () => await database.Table<OrdersErrorLog>().ToListAsync()).Result;
                    if (Error_logs.Count != 0)
                    {
                        string Baseurl = AppOrdersUrl + "OrderManagement/InsertOrdersErrorLog/";
                        OrderJson = JsonConvert.SerializeObject(Error_logs);
                        HttpResponseMessage response = Task.Run(async () => await _client.PostAsync(Baseurl, new StringContent(OrderJson, Encoding.UTF8, "application/json"))).Result;
                        string data = response.Content.ReadAsStringAsync().Result;
                        string ordstatus = JsonConvert.DeserializeObject<string>(data);
                        if (ordstatus == "Yes")
                        {
                            string qry = "Delete from OrdersErrorLog";
                            var DATA = Task.Run(async () => await database.QueryAsync<OrdersErrorLog>(qry)).Result;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--AppOrdersErrorLogInsertdata--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async void UpdateOrderIdandStatus()
        {
            if (AppSettings.UpdateOrderStatus == false)
            {
                AppSettings.UpdateOrderStatus = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        string orderid = string.Empty;
                        var localCustorderdetail = Task.Run(async () => await database.Table<Orders>().Where(i => i.Sman == AppSettings.salesmanid && i.order_id != null).ToListAsync()).Result;
                        foreach (var ord_data in localCustorderdetail)
                        {
                            orderid = orderid + "'" + ord_data.order_id + "'" + ",";
                        }
                        orderid = orderid.TrimEnd(',');
                        string Baseurl = AppUrl + "DataSync/GetUpdatedstatus?orderid=" + orderid;
                        string content = _client.GetStringAsync(Baseurl).Result;
                        var ordstatus = JsonConvert.DeserializeObject<List<Orderstatus>>(content);
                        foreach (var orddata in ordstatus)
                        {
                            string qry = "update Orders set order_status='" + orddata.order_status + "', Invoice_date='" + orddata.Invoice_date + "',Invoice_No='" + orddata.Invoice_No + "',UpdateStatus='" + orddata.UpdateStatus + "' where order_id='" + orddata.order_id + "' ";
                            var DATA = Task.Run(async () => await database.QueryAsync<Orders>(qry)).Result;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                AppSettings.UpdateOrderStatus = false;
            }
        }

        public void GetCustomerMonthWiseSale()
        {
            ObservableRangeCollection<Custmermonthsales> saledetails = new ObservableRangeCollection<Custmermonthsales>();
            try
            {
                string Baseurl = ReportsUrl + "ReportManagement/GetCustomerMonthlySale?custcode=" + AppSettings.customercode + "&partnercode=" + AppSettings.partnercode;
                string content = _client.GetStringAsync(Baseurl).Result;
                saledetails = JsonConvert.DeserializeObject<ObservableRangeCollection<Custmermonthsales>>(content);
                AppSettings.Customermonthsale = saledetails;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--GetCustomerMonthWiseSale--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public void GetOutstandingPendingBill(int patnercode, string salesmancode)
        {
            if (AppSettings.OutstandingsyncStatus == false)
            {
                AppSettings.OutstandingsyncStatus = true;
                ObservableRangeCollection<Outstanding> invoicedetails = new ObservableRangeCollection<Outstanding>();
                try
                {
                    string Baseurl = ReportsUrl + "ReportManagement/GetSalesManCustomerOutstanding?Partnercode=" + patnercode + "&salesmancode=" + salesmancode;
                    string content = _client.GetStringAsync(Baseurl).Result;
                    AppSettings.outstandingcustlistJson = content;
                }
                catch (Exception ex)
                {
                    Analytics.TrackEvent("ItemsDatabase--GetOutstandingPendingBill--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                }
                AppSettings.OutstandingsyncStatus = false;
            }
        }

        public void GetCustomerPendingBill(int patnercode, string custcode)
        {
            ObservableRangeCollection<OutstandingInvoices> invoicedetails = new ObservableRangeCollection<OutstandingInvoices>();
            try
            {
                AppSettings.customeroutstanding = null;
                string Baseurl = ReportsUrl + "ReportManagement/GetOutstandingPendingBillsbyCustomer?Partnercode=" + patnercode + "&custid=" + custcode + "";
                string content = _client.GetStringAsync(Baseurl).Result;
                invoicedetails = JsonConvert.DeserializeObject<ObservableRangeCollection<OutstandingInvoices>>(content);
                AppSettings.customeroutstanding = invoicedetails;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--GetCustomerPendingBill--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public void GetSalesManSummaryReport(string salesmancode, DateTime Fromdate, DateTime Todate)
        {
            List<SalesSummary> salesdetails = new List<SalesSummary>();
            try
            {
                AppSettings.salessummarylist = null;
                string fromdate = Fromdate.ToString("yyyy-MM-dd");
                string todate = Todate.ToString("yyyy-MM-dd");
                string Baseurl = ReportsUrl + "ReportManagement/GetSalesSummaryReport?sman='" + salesmancode + "'&pcode=" + AppSettings.partnercode + "&frmdt=" + fromdate + "&todt=" + todate + "";
                string content = _client.GetStringAsync(Baseurl).Result;
                salesdetails = JsonConvert.DeserializeObject<List<SalesSummary>>(content);
                AppSettings.salessummarylist = salesdetails;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--GetSalesManSummaryReport--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async Task<string> getAppvarsions(string Name)
        {
            try
            {
                var value = Task.Run(async () => await database.Table<Appvarsions>().Where(i => i.Name == Name).FirstOrDefaultAsync()).Result;
                return value.Value;
            }
            catch (Exception ex)
          {
                Analytics.TrackEvent("ItemsDatabase--getAppvarsions--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                return "";
            }
        }

        public async void UpdateAppvarsions(string Name, string value)
        {
            try
            {
                Appvarsions obj = new Appvarsions();
                obj.Name = Name;
                obj.Value = value;
                obj.sno = 1;
                string qry = "update Appvarsions set value='" + value + "' where sno =1";
                var DATA = Task.Run(async () => await database.QueryAsync<Appvarsions>(qry)).Result;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ItemsDatabase--UpdateAppvarsions--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async void GetPendingOrders()
        {
            ObservableCollection<ItemMaster> productList = new ObservableCollection<ItemMaster>();
            try
            {
                var pendlist = Task.Run(async () => await database.Table<orderitems>().Where(i => i.customer_id == AppSettings.customerid && i.AzureStatus == "Pending.").ToListAsync()).Result;
                if (pendlist.Count != 0)
                {
                    foreach (var pendord in pendlist)
                    {
                        ItemMaster Itemdetails = new ItemMaster();
                        if (AppSettings.AllItemdeatial == null)
                        {
                            AppSettings.AllItemdeatial = Task.Run(async () => await database.Table<ItemMaster>().Where(i => i.partnercode == AppSettings.partnercode).ToListAsync()).Result;
                        }
                        var Itemlist = AppSettings.AllItemdeatial.Where(i => i.itemid == pendord.itemcode).FirstOrDefault();
                        Itemdetails.itemname = pendord.itemname;
                        Itemdetails.itemcode = pendord.itemcode.ToString();
                        Itemdetails.stock = Itemlist.stock;
                        if (!string.IsNullOrEmpty(Itemlist.Scheme))
                            Itemdetails.Scheme = Itemlist.Scheme;
                        else
                            Itemdetails.Scheme = "N";

                        Itemdetails.boxsize = Itemlist.boxsize;
                        Itemdetails.Halfscheme = Itemlist.Halfscheme;
                        Itemdetails.itemid = Itemlist.itemid;
                        Itemdetails.manufacturer = Itemlist.manufacturer;
                        Itemdetails.mrp = Itemlist.mrp;
                        Itemdetails.packsize = Itemlist.packsize;
                        Itemdetails.partnercode = pendord.partnercode;
                        Itemdetails.ptr = Itemlist.ptr;
                        Itemdetails.qty = pendord.orderqty;
                        Itemdetails.Increment = pendord.srlno;
                        AppSettings.partnercode = pendord.partnercode;
                        productList.Add(Itemdetails);
                    }
                    AppSettings.CartItems = productList;
                }
            }
            catch (Exception ex)
            {
                AppSettings.CartItems = productList;
                Analytics.TrackEvent("ItemsDatabase--GetPendingOrders--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async void InsertAppcustdelivery(AppCustDlvry Checkdetail)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                string Url = serviceUrl + "Invoice/UpdateCustomerDeliveryDetails/";
                try
                {
                    string content = JsonConvert.SerializeObject(Checkdetail); //Serializes or convert the created Post into a JSON String
                    HttpResponseMessage response = await _client.PostAsync(Url, new StringContent(content, Encoding.UTF8, "application/json"));
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
        }

        public async void UpdateAppcustInvoiceupdatedata(string invnum)
        {
            string Url = serviceUrl + "Invoice/UpdateCustomerInvoice?invnum=" + invnum;
            try
            {
                string content = _client.GetStringAsync(Url).Result;
                // string content = JsonConvert.SerializeObject(Checkdetail); //Serializes or convert the created Post into a JSON String
                // HttpResponseMessage response = await _client.PostAsync(Url,null);
            }
            catch (Exception ex)
            {
            }
        }

        public List<Orders> OrderHistory()
        {
            List<Orders> Orederdetails = new List<Orders>();
            try
            {
                if (AppSettings.loginname == "Salesman Login")
                {
                    if (AppSettings.singleCustpendingbill == true)
                    {
                        string Baseurl = AppUrl + "DataSync/GetCompliteOrdersCustomer?Custmerid=" + AppSettings.CustID + "&Fromdate=" + AppSettings.Fromdate + "&Todate=" + AppSettings.Todate + "&Partnercode=" + AppSettings.partnercode + "";
                        string content = _client.GetStringAsync(Baseurl).Result;
                        Orederdetails = JsonConvert.DeserializeObject<List<Orders>>(content);
                    }
                    else
                    {
                        string Baseurl = AppUrl + "DataSync/GetCompliteOrdersSman?smanid=" + AppSettings.salesmanid + "&Fromdate=" + AppSettings.Fromdate + "&Todate=" + AppSettings.Todate + "&Partnercode=" + AppSettings.partnercode + "";
                        string content = _client.GetStringAsync(Baseurl).Result;
                        Orederdetails = JsonConvert.DeserializeObject<List<Orders>>(content);
                    }

                }
                else
                {
                    string Baseurl = AppUrl + "DataSync/GetCompliteOrdersCustomer?Custmerid=" + AppSettings.customerid + "&Fromdate=" + AppSettings.Fromdate + "&Todate=" + AppSettings.Todate + "&Partnercode=" + AppSettings.partnercode + "";
                    string content = _client.GetStringAsync(Baseurl).Result;
                    Orederdetails = JsonConvert.DeserializeObject<List<Orders>>(content);
                }

            }
            catch (Exception ex)
            {


            }
            return Orederdetails;
        }
        public async void AppcustdeliveryInsertdata()
        {
            try
            {
                //if (CrossConnectivity.Current.IsConnected)
                //{
                //    var AppcustInvoicedata = Task.Run(async () => await database.Table<AppCustDlvry>().Where(i => i.CreatedDate >= AppSettings.publishdate).ToListAsync()).Result;
                //    foreach (var appcustdelvry in AppcustInvoicedata)
                //    {
                //        string uploadedFilename = string.Empty;
                //        SinglaApp.Azure.AppCustDlvry custdelvry = new SinglaApp.Azure.AppCustDlvry();
                //        var CustDlvrydetail = Task.Run(async () => await App.ServiceClient.GetTable<SinglaApp.Azure.AppCustDlvry>().Where(i => i.DlvryCode == appcustdelvry.DlvryCode).ToListAsync()).Result;
                //        if (CustDlvrydetail.Count == 0)
                //        {
                //            custdelvry.Custcode = appcustdelvry.Custcode;
                //            custdelvry.CustAltcode = appcustdelvry.CustAltcode;
                //            custdelvry.Custname = appcustdelvry.Custname;
                //            custdelvry.NoofBoxes = appcustdelvry.NoofBoxes;
                //            custdelvry.Noofpackes = appcustdelvry.Noofpackes;
                //            uploadedFilename = AzureStorage.UploadFileAsync(ContainerType.photos, new MemoryStream(appcustdelvry.InvoiceImage));
                //            custdelvry.Photo = uploadedFilename;
                //            custdelvry.DlvryDate = appcustdelvry.DlvryDate;
                //            custdelvry.DlvryTime = appcustdelvry.DlvryTime;
                //            custdelvry.InvoiceDeltails = appcustdelvry.InvoiceDeltails;
                //            custdelvry.InvoiceDate = appcustdelvry.InvoiceDate;
                //            custdelvry.CreatedDate = appcustdelvry.CreatedDate;
                //            custdelvry.DlvryCode = appcustdelvry.DlvryCode;
                //            custdelvry.patnercode = appcustdelvry.patnercode;

                //            try
                //            {
                //                await App.ServiceClient.GetTable<SinglaApp.Azure.AppCustDlvry>().InsertAsync(custdelvry);
                //            }
                //            catch (Exception ex)
                //            {
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                //}
            }
            catch (Exception ex)
            {
                string error = ex.InnerException.Message;
            }
        }

        public async void AppcustInvoiceupdatedata()
        {
            //try
            //{
            //    var custInvoicedata = Task.Run(async () => await database.Table<AppcustInvoice>().Where(i => i.CreatedDate >= AppSettings.publishdate).ToListAsync()).Result;
            //    foreach (var custinvoice in custInvoicedata)
            //    {
            //        var CustinvoiceDetail = Task.Run(async () => await App.ServiceClient.GetTable<SinglaApp.Azure.AppcustInvoice>().Where(i => i.DlvryStatus == "y" && i.invnum == custinvoice.invnum).ToListAsync()).Result;
            //        if (CustinvoiceDetail.Count == 0)
            //        {
            //            SinglaApp.Azure.AppcustInvoice appcustdelvry = new SinglaApp.Azure.AppcustInvoice();
            //            appcustdelvry.custcode = custinvoice.custcode;
            //            appcustdelvry.custname = custinvoice.custname;
            //            appcustdelvry.invnum = custinvoice.invnum;
            //            appcustdelvry.invdate = Convert.ToDateTime(custinvoice.invdate);
            //            appcustdelvry.address = custinvoice.address;
            //            appcustdelvry.address1 = custinvoice.address1;
            //            appcustdelvry.address2 = custinvoice.address2;
            //            appcustdelvry.address3 = custinvoice.address3;
            //            appcustdelvry.address4 = custinvoice.address4;
            //            appcustdelvry.noofitems = custinvoice.noofitems.ToString();
            //            appcustdelvry.invamt = custinvoice.invamt.ToString();
            //            appcustdelvry.telephone = custinvoice.telephone;
            //            appcustdelvry.mobile = custinvoice.mobile;
            //            appcustdelvry.Status = custinvoice.Status;
            //            appcustdelvry.id = custinvoice.id;
            //            appcustdelvry.DlvryStatus = "Y";
            //            appcustdelvry.DlvryDate = DateTime.Now.ToString("MM/dd/yyyy");
            //            appcustdelvry.DlvryTime = DateTime.Now.ToString("HH:mm:ss");
            //            appcustdelvry.partnercode = custinvoice.partnercode;
            //            try
            //            {
            //                await App.ServiceClient.GetTable<SinglaApp.Azure.AppcustInvoice>().UpdateAsync(appcustdelvry);
            //            }
            //            catch (Exception ex)
            //            {
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }
}
public interface IFileHelper
{
    string GetLocalFilePath(string filename);
    void CopyDatabase();
    string Version { get; }
    void closeApplication();
}
public interface ILatest
{
    string InstalledVersionNumber { get; }
    Task<string> GetLatestVersionNumber();
    Task<string> GetLatestVersionNumber(string appName);
    Task<bool> IsUsingLatestVersion();
    Task OpenAppInStore();
    Task OpenAppInStore(string appName);
}


