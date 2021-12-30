using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace PickList.Model
{
    public class AppSettings
    {
        static ISettings UserSetting
        {
            get
            {
                return CrossSettings.Current;
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

        public static string PhoneCode
        {
            get => UserSetting.GetValueOrDefault(nameof(PhoneCode), string.Empty);
            set => UserSetting.AddOrUpdateValue(nameof(PhoneCode), value);
        }

        public static string Invnum

        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Invnum"))
                {
                    var asDbStatus = Application.Current.Properties["Invnum"];
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
                Application.Current.Properties["Invnum"] = value;
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

        public static string CurrentBasketNo
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CurrentBasketNo"))
                {
                    var asDbStatus = Application.Current.Properties["CurrentBasketNo"];
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
                Application.Current.Properties["CurrentBasketNo"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string BasketNo
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("BasketNo"))
                {
                    var asDbStatus = Application.Current.Properties["BasketNo"];
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
                Application.Current.Properties["BasketNo"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }

        public static string PickDate
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PickDate"))
                {
                    var asDbStatus = Application.Current.Properties["PickDate"];
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
                Application.Current.Properties["PickDate"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static ObservableCollection<PickingDetail> PickingDateDetails
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("PickingDateDetails"))
                {
                    var asDbStatus = Application.Current.Properties["PickingDateDetails"];
                    return (ObservableCollection<PickingDetail>)asDbStatus;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["PickingDateDetails"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static ObservableCollection<UnPickingList> CheckerUnpickList
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("CheckerUnpickList"))
                {
                    var asDbStatus = Application.Current.Properties["CheckerUnpickList"];
                    return (ObservableCollection<UnPickingList>)asDbStatus;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["CheckerUnpickList"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static CustomerDetail customerDetail
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("customerDetail"))
                {
                    var asDbStatus = Application.Current.Properties["customerDetail"];
                    return (CustomerDetail)asDbStatus;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["customerDetail"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
        public static ObservableCollection<CustomerDetail> customerDetails
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("customerDetails"))
                {
                    var asDbStatus = Application.Current.Properties["customerDetails"];
                    return (ObservableCollection<CustomerDetail>)asDbStatus;
                    // do something with id
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Application.Current.Properties["customerDetails"] = value;
                Task.Run(async () => await Application.Current.SavePropertiesAsync());
            }
        }
    }
}
