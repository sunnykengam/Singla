using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SinglaMedicos.Model;
using SinglaMedicos.Model.Company;
using SinglaMedicos.Model.Reports;
using SinglaMedicos.ViewModel;
using SinglaMedicos.ViewModel.Company;
using Xamarin.Forms;
using Newtonsoft.Json;
using Plugin.Settings.Abstractions;
using Plugin.Settings;

namespace SinglaMedicos.Common
{
    public static class AppSettings
    {
        static ISettings UserSetting
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static void ClearAllData()
        {
            username = string.Empty;
            password = string.Empty;
            //loginname = string.Empty;
            LoginSuccess = false;
            UpdateazureDB = false;
            UpdateLocalDB = false;
            salesmanid = 0;
            salesmancode = string.Empty;
            salesmanname = string.Empty;
            partnercode = 0;
            SaleasmanListjson = string.Empty;
            CustomerListjson = string.Empty;
            Customerjson = string.Empty;
            customercode = string.Empty;
            customerid = string.Empty;
            customername = string.Empty;
            PartnerlistJson = string.Empty;
            Saleasmansplitcode = string.Empty;
        }

        public static string username
        {
            get => UserSetting.GetValueOrDefault(nameof(username), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(username), value);
        }

        public static string password
        {
            get => UserSetting.GetValueOrDefault(nameof(password), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(password), value);
        }

        public static string loginname
        {
            get => UserSetting.GetValueOrDefault(nameof(loginname), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(loginname), value);
        }

        public static bool LoginSuccess
        {
            get => UserSetting.GetValueOrDefault(nameof(LoginSuccess), false);
            set => UserSetting.AddOrUpdateValue(nameof(LoginSuccess), value);
        }

        public static bool EntryFocusStatus
        {
            get => UserSetting.GetValueOrDefault(nameof(EntryFocusStatus), false);
            set => UserSetting.AddOrUpdateValue(nameof(EntryFocusStatus), value);
        }

        public static bool UpdateazureDB
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("UpdateazureDB"))
                {
                    var asDbStatus = Application.Current.Properties["UpdateazureDB"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["UpdateazureDB"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool CustomersyncStatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CustomersyncStatus"))
                {
                    var asDbStatus = Application.Current.Properties["CustomersyncStatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["CustomersyncStatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static bool UpdatePendingStatus
        {
            get => UserSetting.GetValueOrDefault(nameof(UpdatePendingStatus), false);
            set => UserSetting.AddOrUpdateValue(nameof(UpdatePendingStatus), value);
        }

        public static List<Orders> PendingOrderList
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PendingOrderList"))
                {
                    var PendingList = Application.Current.Properties["PendingOrderList"];
                    return (List<Orders>)PendingList;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["PendingOrderList"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());

            }
        }

        public static bool ProductssyncStatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("ProductssyncStatus"))
                {
                    var asDbStatus = Application.Current.Properties["ProductssyncStatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["ProductssyncStatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool OutstandingsyncStatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("OutstandingsyncStatus"))
                {
                    var asDbStatus = Application.Current.Properties["OutstandingsyncStatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["OutstandingsyncStatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool UpdateOrderStatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("UpdateOrderStatus"))
                {
                    var asDbStatus = Application.Current.Properties["UpdateOrderStatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["UpdateOrderStatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool PendingOrderStatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PendingOrderStatus"))
                {
                    var asDbStatus = Application.Current.Properties["PendingOrderStatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["PendingOrderStatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool UpdateLocalDB
        {
            get => UserSetting.GetValueOrDefault(nameof(UpdateLocalDB), false);
            set => UserSetting.AddOrUpdateValue(nameof(UpdateLocalDB), value);
        }

        public static int salesmanid
        {
            get => UserSetting.GetValueOrDefault(nameof(salesmanid), 0);
            set => UserSetting.AddOrUpdateValue(nameof(salesmanid), value);
        }

        public static string salesmancode
        {
            get => UserSetting.GetValueOrDefault(nameof(salesmancode), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(salesmancode), value);
        }

        public static string salesmanname
        {
            get => UserSetting.GetValueOrDefault(nameof(salesmanname), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(salesmanname), value);
        }

        public static int partnercode
        {
            get => UserSetting.GetValueOrDefault(nameof(partnercode), 0);
            set => UserSetting.AddOrUpdateValue(nameof(partnercode), value);
        }

        public static string SaleasmanListjson
        {
            get => UserSetting.GetValueOrDefault(nameof(SaleasmanListjson), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(SaleasmanListjson), value);
        }

        public static SalesmanMaster Salesmandetail
        {
            get
            {
                if (!string.IsNullOrEmpty(AppSettings.SaleasmanListjson))
                {
                    var Salesmandetail = JsonConvert.DeserializeObject<SalesmanMaster>(AppSettings.SaleasmanListjson);
                    return (SalesmanMaster)Salesmandetail;
                }
                else
                {
                    return null;
                }

            }
            set
            {
                Application.Current.Properties["Salesmandetail"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string CustomerListjson
        {
            get => UserSetting.GetValueOrDefault(nameof(CustomerListjson), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(CustomerListjson), value);
        }

        public static string Customerjson
        {
            get => UserSetting.GetValueOrDefault(nameof(Customerjson), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(Customerjson), value);
        }

        public static bool OutstandingStatus
        {
            get => UserSetting.GetValueOrDefault(nameof(OutstandingStatus), false);
            set => UserSetting.AddOrUpdateValue(nameof(OutstandingStatus), value);
        }

        public static CustomerMaster custdetail
        {
            get
            {
                if (!string.IsNullOrEmpty(AppSettings.Customerjson))
                {
                    var customerdetail = JsonConvert.DeserializeObject<CustomerMaster>(AppSettings.Customerjson);
                    return (CustomerMaster)customerdetail;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["custdetail"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());

            }
        }

        public static List<CustomerMaster> customerdetail
        {
            get
            {
                if (!string.IsNullOrEmpty(AppSettings.CustomerListjson))
                {
                    var customerdetail = JsonConvert.DeserializeObject<List<CustomerMaster>>(AppSettings.CustomerListjson);
                    return (List<CustomerMaster>)customerdetail;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["customerdetail"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string customercode
        {
            get => UserSetting.GetValueOrDefault(nameof(customercode), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(customercode), value);
        }

        public static string customerid
        {
            get => UserSetting.GetValueOrDefault(nameof(customerid), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(customerid), value);
        }

        public static string customername
        {
            get => UserSetting.GetValueOrDefault(nameof(customername), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(customername), value);
        }

        public static string PartnerlistJson
        {
            get => UserSetting.GetValueOrDefault(nameof(PartnerlistJson), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(PartnerlistJson), value);
        }

        public static List<Partner> Partnerlist
        {
            get
            {
                if (!string.IsNullOrEmpty(AppSettings.PartnerlistJson))
                {
                    var Partnerlist = JsonConvert.DeserializeObject<List<Partner>>(AppSettings.PartnerlistJson);
                    return (List<Partner>)Partnerlist;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["Partnerlist"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string outstandingcustlistJson
        {
            get => UserSetting.GetValueOrDefault(nameof(outstandingcustlistJson), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(outstandingcustlistJson), value);
        }
        public static string SyncFirstDateTime
        {

            get
            {
                if (Application.Current.Properties.ContainsKey("SyncFirstDateTime"))
                {
                    var asDbStatus = Application.Current.Properties["SyncFirstDateTime"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return Convert.ToString(DateTime.Now);
                }
            }
            set
            {
                Application.Current.Properties["SyncFirstDateTime"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static ObservableRangeCollection<Outstanding> outstandingcustlist
        {
            get
            {
                if (!string.IsNullOrEmpty(AppSettings.outstandingcustlistJson))
                {
                    var outstandingcustlist = JsonConvert.DeserializeObject<ObservableRangeCollection<Outstanding>>(AppSettings.outstandingcustlistJson);
                    return (ObservableRangeCollection<Outstanding>)outstandingcustlist;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["outstandingcustlist"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static DateTime publishdate
        {

            get
            {
                if (Application.Current.Properties.ContainsKey("publishdate"))
                {
                    var asDbStatus = Application.Current.Properties["publishdate"];
                    return Convert.ToDateTime(asDbStatus);
                    // do something with id
                }
                else
                {
                    return Convert.ToDateTime(DateTime.Today.AddDays(-1));
                }
            }
            set
            {
                Application.Current.Properties["publishdate"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Fromdate
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Fromdate"))
                {
                    var asDbStatus = Application.Current.Properties["Fromdate"];
                    return Convert.ToString(asDbStatus);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Fromdate"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Todate
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Todate"))
                {
                    var asDbStatus = Application.Current.Properties["Todate"];
                    return Convert.ToString(asDbStatus);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Todate"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool versionupdate
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("versionupdate"))
                {
                    var asDbStatus = Application.Current.Properties["versionupdate"];
                    return Convert.ToBoolean(asDbStatus);
                    // do something with id
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["versionupdate"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string SearchHistory
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("SearchHistory"))
                {
                    var asDbStatus = Application.Current.Properties["SearchHistory"];
                    return Convert.ToString(asDbStatus);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["SearchHistory"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool Pendingorderstatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Pendingorderstatus"))
                {
                    var asDbStatus = Application.Current.Properties["Pendingorderstatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["Pendingorderstatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static ObservableCollection<Custmermonthsales> Customermonthsale
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Customermonthsale"))
                {
                    var Customermonthsale = Application.Current.Properties["Customermonthsale"];
                    return (ObservableCollection<Custmermonthsales>)Customermonthsale;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["Customermonthsale"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());

            }
        }


        public static List<AppPendingOrders> PendingList
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PendingList"))
                {
                    var PendingList = Application.Current.Properties["PendingList"];
                    return (List<AppPendingOrders>)PendingList;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["PendingList"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());

            }
        }

        public static string Custcode
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Custcode"))
                {
                    var asDbStatus = Application.Current.Properties["Custcode"];
                    return Convert.ToString(asDbStatus);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Custcode"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static string CustID
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CustID"))
                {
                    var asDbStatus = Application.Current.Properties["CustID"];
                    return Convert.ToString(asDbStatus);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["CustID"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static string Saleasmansplitcode
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Saleasmansplitcode"))
                {
                    var asDbStatus = Application.Current.Properties["Saleasmansplitcode"];
                    return Convert.ToString(asDbStatus);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Saleasmansplitcode"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool singleCustpendingbill
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("singleCustpendingbill"))
                {
                    var asDbStatus = Application.Current.Properties["singleCustpendingbill"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["singleCustpendingbill"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool cartpagestatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("cartpagestatus"))
                {
                    var asDbStatus = Application.Current.Properties["cartpagestatus"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["cartpagestatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Customercode
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Customercode"))
                {
                    var asDbStatus = Application.Current.Properties["Customercode"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Customercode"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Imagepath
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Imagepath"))
                {
                    var asDbStatus = Application.Current.Properties["Imagepath"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Imagepath"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static List<SinglaMedicos.Azure.AppcustInvoice> Invoicedetails
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Invoicedetails"))
                {
                    var customerdetail = Application.Current.Properties["Invoicedetails"];
                    return (List<SinglaMedicos.Azure.AppcustInvoice>)customerdetail;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["Invoicedetails"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());

            }
        }

        public static int prtcode
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("prtcode"))
                {
                    var asDbStatus = Application.Current.Properties["prtcode"];
                    return Convert.ToInt32(asDbStatus);
                    // do something with id
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Application.Current.Properties["prtcode"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Itemid
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Itemid"))
                {
                    var asDbStatus = Application.Current.Properties["Itemid"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Itemid"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string ProductName
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("ProductName"))
                {
                    var asDbStatus = Application.Current.Properties["ProductName"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["ProductName"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Location
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Location"))
                {
                    var asDbStatus = Application.Current.Properties["Location"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["Location"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string PartnercodeS
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PartnercodeS"))
                {
                    var asDbStatus = Application.Current.Properties["PartnercodeS"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["PartnercodeS"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string PartnerName
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PartnerName"))
                {
                    var asDbStatus = Application.Current.Properties["PartnerName"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["PartnerName"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }


        public static string itemname
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("itemname"))
                {
                    var asDbStatus = Application.Current.Properties["itemname"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["itemname"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string TitelName
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("TitelName"))
                {
                    var asDbStatus = Application.Current.Properties["TitelName"];
                    return Convert.ToString(asDbStatus);
                    // do something with id
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Application.Current.Properties["TitelName"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

       

        public static ObservableCollection<ItemMaster> CartItems
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CartItems"))
                {
                    var CartItems = Application.Current.Properties["CartItems"];
                    return (ObservableCollection<ItemMaster>)CartItems;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["CartItems"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static ObservableRangeCollection<OrdersViewModel> orderlist
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("orderlist"))
                {
                    var orderlist = Application.Current.Properties["orderlist"];
                    return (ObservableRangeCollection<OrdersViewModel>)orderlist;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["orderlist"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static ObservableRangeCollection<MGroupViewModel> MGroup_List
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("MGroup_List"))
                {
                    var orderlist = Application.Current.Properties["MGroup_List"];
                    return (ObservableRangeCollection<MGroupViewModel>)orderlist;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["MGroup_List"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

       

        public static ObservableRangeCollection<OutstandingInvoices> customeroutstanding
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("customeroutstanding"))
                {
                    var customeroutstanding = Application.Current.Properties["customeroutstanding"];
                    return (ObservableRangeCollection<OutstandingInvoices>)customeroutstanding;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["customeroutstanding"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static List<SalesSummary> salessummarylist
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("salessummarylist"))
                {
                    var salessummarylist = Application.Current.Properties["salessummarylist"];
                    return (List<SalesSummary>)salessummarylist;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["salessummarylist"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static List<MGroupName> MG_List
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("MG_List"))
                {
                    var MG_List = Application.Current.Properties["MG_List"];
                    return (List<MGroupName>)MG_List;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["MG_List"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static ObservableRangeCollection<OutstandingViewmodel> outstandingbill
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("outstandingbill"))
                {
                    var outstandingcustlist = Application.Current.Properties["outstandingbill"];
                    return (ObservableRangeCollection<OutstandingViewmodel>)outstandingcustlist;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["outstandingbill"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static List<ItemMaster> Itemdeatial
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Itemdeatial"))
                {
                    var Itemdeatial = Application.Current.Properties["Itemdeatial"];
                    return (List<ItemMaster>)Itemdeatial;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["Itemdeatial"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static List<ItemMaster> AllItemdeatial
        {

            get
            {
                if (Application.Current.Properties.ContainsKey("AllItemdeatial"))
                {
                    var AllItemdeatial = Application.Current.Properties["AllItemdeatial"];
                    return (List<ItemMaster>)AllItemdeatial;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["AllItemdeatial"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static List<ItemMaster> AllItem_list
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("AllItem_list"))
                {
                    var AllItemdeatial = Application.Current.Properties["AllItem_list"];
                    return (List<ItemMaster>)AllItemdeatial;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["AllItem_list"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static bool IsUserLoggedIn
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("IsUserLoggedIn "))
                {
                    var asDbStatus = Application.Current.Properties["IsUserLoggedIn "];
                    return Convert.ToBoolean(asDbStatus);
                    // do something with id
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["IsUserLoggedIn "] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static bool ShowPendingOrders
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("ShowPendingOrders"))
                {
                    var asDbStatus = Application.Current.Properties["ShowPendingOrders"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["ShowPendingOrders"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static bool ShowCompleteOrders
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("ShowCompleteOrders"))
                {
                    var asDbStatus = Application.Current.Properties["ShowCompleteOrders"];
                    return Convert.ToBoolean(asDbStatus);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                Application.Current.Properties["ShowCompleteOrders"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }

        }
        public static int status
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("status"))
                {
                    var asDbStatus = Application.Current.Properties["status"];
                    return Convert.ToInt32(asDbStatus);
                    // do something with id
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Application.Current.Properties["status"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

    }
}
