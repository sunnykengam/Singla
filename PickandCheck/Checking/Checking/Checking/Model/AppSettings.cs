using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Checking.Models
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

        public static string UserName
        {
            get => UserSetting.GetValueOrDefault(nameof(UserName), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string Password
        {
            get => UserSetting.GetValueOrDefault(nameof(Password), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(Password), value);
        }

        public static string UserDetailJson
        {
            get => UserSetting.GetValueOrDefault(nameof(UserDetailJson), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(UserDetailJson), value);
        }

        public static string IPAddress
        {
            get => UserSetting.GetValueOrDefault(nameof(IPAddress), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(IPAddress), value);
        }
        

        public static UserLogin UserDetail
        {
            get
            {
                if (!string.IsNullOrEmpty(AppSettings.UserDetailJson))
                {
                    string data = AppSettings.UserDetailJson.Replace("[", string.Empty).Replace("]", string.Empty);
                    var UserDetail = JsonConvert.DeserializeObject<UserLogin>(data);
                    return (UserLogin)UserDetail;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["UserDetail"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static void ClearAllData()
        {
            UserName = string.Empty;
            Password = string.Empty;
            UserDetailJson = string.Empty;
        }

        public static string CustCode
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CustCode"))
                {
                    var asDbStatus = Application.Current.Properties["CustCode"];
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
                Application.Current.Properties["CustCode"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Imagedata
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Imagedata"))
                {
                    var asDbStatus = Application.Current.Properties["Imagedata"];
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
                Application.Current.Properties["Imagedata"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Block
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Block"))
                {
                    var asDbStatus = Application.Current.Properties["Block"];
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
                Application.Current.Properties["Block"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

       

        public static PickedDetail PickedDetail
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PickedDetail"))
                {
                    var asDbStatus = Application.Current.Properties["PickedDetail"];
                    return (PickedDetail)asDbStatus;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["PickedDetail"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string InvoiceDate
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("InvoiceDate"))
                {
                    var asDbStatus = Application.Current.Properties["InvoiceDate"];
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
                Application.Current.Properties["InvoiceDate"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool Basketserchstatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Basketserchstatus"))
                {
                    var asDbStatus = Application.Current.Properties["Basketserchstatus"];
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
                Application.Current.Properties["Basketserchstatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool serchstatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("serchstatus"))
                {
                    var asDbStatus = Application.Current.Properties["serchstatus"];
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
                Application.Current.Properties["serchstatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static bool InsertStatus
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("InsertStatus"))
                {
                    var asDbStatus = Application.Current.Properties["InsertStatus"];
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
                Application.Current.Properties["InsertStatus"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string InvoiceNo
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("InvoiceNo"))
                {
                    var asDbStatus = Application.Current.Properties["InvoiceNo"];
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
                Application.Current.Properties["InvoiceNo"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string CurrentInvoiceNo
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CurrentInvoiceNo"))
                {
                    var asDbStatus = Application.Current.Properties["CurrentInvoiceNo"];
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
                Application.Current.Properties["CurrentInvoiceNo"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string Basket_No
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Basket_No"))
                {
                    var asDbStatus = Application.Current.Properties["Basket_No"];
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
                Application.Current.Properties["Basket_No"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static ObservableCollection<InvoiceItem> CheckerPickedList
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CheckerPickedList"))
                {
                    var CheckerPickedList = Application.Current.Properties["CheckerPickedList"];
                    return (ObservableCollection<InvoiceItem>)CheckerPickedList;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["CheckerPickedList"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static ObservableCollection<InvoiceItem> Picked_List
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Picked_List"))
                {
                    var CheckerPickedList = Application.Current.Properties["Picked_List"];
                    return (ObservableCollection<InvoiceItem>)CheckerPickedList;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["Picked_List"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static List<InvoiceItem> Serch_items
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Serch_items"))
                {
                    var CheckerPickedList = Application.Current.Properties["Serch_items"];
                    return (List<InvoiceItem>)CheckerPickedList;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["Serch_items"] = value;
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

    }
}
