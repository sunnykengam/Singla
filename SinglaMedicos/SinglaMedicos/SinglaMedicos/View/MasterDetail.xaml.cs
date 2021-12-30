using SinglaMedicos.Model;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using SinglaMedicos.ViewModel;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SinglaMedicos.Common;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetail : MasterDetailPage
    {
        public SQLiteAsyncConnection database;
        public MasterDetail()
        {
            try
            {
                InitializeComponent();
                BackgroundImage = "Mainbackground.png";
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                if (AppSettings.loginname == "Salesman Login")
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainScreen)));
                    lbname.Text = AppSettings.salesmanname;
                }
                else
                {
                    lbname.Text = AppSettings.customername;
                    lbemail.Text = AppSettings.custdetail.customeremail;
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CustomerMainScreen)));
                }
                lblversion.Text = DependencyService.Get<IFileHelper>().Version;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
        private async void MyProfile_Clicked(object sender, EventArgs e)
        {
            try
            {
                imgProfile.IsEnabled = false;
                MyProfile.IsEnabled = false;
                MyProfile.TextColor = Color.Orange;
                imgProfile.TextColor = Color.Orange;
                ChangePassword.TextColor = Color.Black;
                imgChangepassword.TextColor = Color.Black;
                Last10Orders.TextColor = Color.Black;
                imgLastorders.TextColor = Color.Black;
                Messages.TextColor = Color.Black;
                imgmessage.TextColor = Color.Black;
                location.TextColor = Color.Black;
                imglocation.TextColor = Color.Black;
                PendingOrders.TextColor = Color.Black;
                imgPendingOrders.TextColor = Color.Black;
                Logout.TextColor = Color.Black;
                imgLogout.TextColor = Color.Black;
                AppSettings.Custcode = null;
                await Navigation.PushAsync(new MyProfile());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--MyProfile_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            imgProfile.IsEnabled = true;
            MyProfile.IsEnabled = true;
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                Logout.IsEnabled = false;
                MyProfile.TextColor = Color.Black;
                imgProfile.TextColor = Color.Black;
                ChangePassword.TextColor = Color.Black;
                imgChangepassword.TextColor = Color.Black;
                Last10Orders.TextColor = Color.Black;
                imgLastorders.TextColor = Color.Black;
                Messages.TextColor = Color.Black;
                imgmessage.TextColor = Color.Black;
                location.TextColor = Color.Black;
                imglocation.TextColor = Color.Black;
                PendingOrders.TextColor = Color.Black;
                imgPendingOrders.TextColor = Color.Black;
                Logout.TextColor = Color.Orange;
                imgLogout.TextColor = Color.Orange;
                if (await DisplayAlert("Exit?", "Are you sure? Do you want to Logout", "Yes", "No"))
                {
                    await Navigation.PushAsync(new LoginPage());
                    if (AppSettings.loginname == "Salesman Login")
                    {
                        var DATA =  database.QueryAsync<SalesmanMaster>("Delete from SalesmanMaster ").Result;
                    }
                    var DATA1 =  database.QueryAsync<CustomerMaster>("Delete from CustomerMaster").Result;
                    var date = database.QueryAsync<CustomerMaster>("Delete From ItemMaster").Result;
                    AppSettings.ClearAllData();
                    AppSettings.LoginSuccess = false;
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--Logout_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            Logout.IsEnabled = true;
        }

        private void ChangePassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                ChangePassword.IsEnabled = false;
                MyProfile.TextColor = Color.Black;
                imgProfile.TextColor = Color.Black;
                ChangePassword.TextColor = Color.Orange;
                imgChangepassword.TextColor = Color.Orange;
                Last10Orders.TextColor = Color.Black;
                imgLastorders.TextColor = Color.Black;
                Messages.TextColor = Color.Black;
                imgmessage.TextColor = Color.Black;
                location.TextColor = Color.Black;
                imglocation.TextColor = Color.Black;
                PendingOrders.TextColor = Color.Black;
                imgPendingOrders.TextColor = Color.Black;
                Logout.TextColor = Color.Black;
                imgLogout.TextColor = Color.Black;
                // Navigation.PushAsync(new ChangePassword());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--ChangePassword_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            ChangePassword.IsEnabled = true;
        }

        private void Messages_Clicked(object sender, EventArgs e)
        {
            try
            {
                Messages.IsEnabled = false;
                MyProfile.TextColor = Color.Black;
                imgProfile.TextColor = Color.Black;
                ChangePassword.TextColor = Color.Black;
                imgChangepassword.TextColor = Color.Black;
                Last10Orders.TextColor = Color.Black;
                imgLastorders.TextColor = Color.Black;
                Messages.TextColor = Color.Orange;
                imgmessage.TextColor = Color.Orange;
                location.TextColor = Color.Black;
                imglocation.TextColor = Color.Black;
                PendingOrders.TextColor = Color.Black;
                imgPendingOrders.TextColor = Color.Black;
                Logout.TextColor = Color.Black;
                imgLogout.TextColor = Color.Black;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--Messages_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            Messages.IsEnabled = true;
        }

        private async void Last10Orders_Clicked(object sender, EventArgs e)
        {
            try
            {
                Last10Orders.IsEnabled = false;
                AppSettings.TitelName = "Last 10 Orders";
                MyProfile.TextColor = Color.Black;
                imgProfile.TextColor = Color.Black;
                ChangePassword.TextColor = Color.Black;
                imgChangepassword.TextColor = Color.Black;
                Last10Orders.TextColor = Color.Orange;
                imgLastorders.TextColor = Color.Orange;
                Messages.TextColor = Color.Black;
                imgmessage.TextColor = Color.Black;
                location.TextColor = Color.Black;
                imglocation.TextColor = Color.Black;
                PendingOrders.TextColor = Color.Black;
                imgPendingOrders.TextColor = Color.Black;
                Logout.TextColor = Color.Black;
                imgLogout.TextColor = Color.Black;
                await Navigation.PushAsync(new CompleteOrders(new ViewModel.CompleteViewModel()));
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--Last10Orders_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            Last10Orders.IsEnabled = true;
        }

        private async void PendingOrders_Clicked(object sender, EventArgs e)
        {
            try
            {
                PendingOrders.IsEnabled = false;
                MyProfile.TextColor = Color.Black;
                imgProfile.TextColor = Color.Black;
                ChangePassword.TextColor = Color.Black;
                imgChangepassword.TextColor = Color.Black;
                Last10Orders.TextColor = Color.Black;
                imgLastorders.TextColor = Color.Black;
                Messages.TextColor = Color.Black;
                imgmessage.TextColor = Color.Black;
                location.TextColor = Color.Black;
                imglocation.TextColor = Color.Black;
                PendingOrders.TextColor = Color.Orange;
                imgPendingOrders.TextColor = Color.Orange;
                Logout.TextColor = Color.Black;
                imgLogout.TextColor = Color.Black;
                await Navigation.PushAsync(new PendingOrders());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--PendingOrders_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            PendingOrders.IsEnabled = true;
        }

        private async void Location_Clicked(object sender, EventArgs e)
        {
            location.IsEnabled = false;
            MyProfile.TextColor = Color.Black;
            imgProfile.TextColor = Color.Black;
            ChangePassword.TextColor = Color.Black;
            imgChangepassword.TextColor = Color.Black;
            Last10Orders.TextColor = Color.Black;
            imgLastorders.TextColor = Color.Black;
            Messages.TextColor = Color.Black;
            imgmessage.TextColor = Color.Black;
            location.TextColor = Color.Orange;
            imglocation.TextColor = Color.Orange;
            PendingOrders.TextColor = Color.Black;
            imgPendingOrders.TextColor = Color.Black;
            Logout.TextColor = Color.Black;
            imgLogout.TextColor = Color.Black;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                Locationinfo info = new Locationinfo();
                var location = await locator.GetPositionAsync(TimeSpan.FromTicks(10000));
                Position position = new Position(location.Latitude, location.Longitude);
                var addresses = await locator.GetAddressesForPositionAsync(position);
                string combAdd = string.Empty;
                foreach (var addinfo in addresses)
                {
                    combAdd += addinfo.FeatureName + ",";
                    if (!string.IsNullOrEmpty(addinfo.Thoroughfare))
                        info.Thoroughfare = addinfo.Thoroughfare;

                    if (!string.IsNullOrEmpty(addinfo.Locality))
                        info.Locality = addinfo.Locality;
                    if (!string.IsNullOrEmpty(addinfo.SubLocality))
                        info.SubLocality = addinfo.SubLocality;
                    if (!string.IsNullOrEmpty(addinfo.AdminArea))
                        info.AdminArea = addinfo.AdminArea;
                    if (!string.IsNullOrEmpty(addinfo.PostalCode))
                        info.PostalCode = addinfo.PostalCode;
                    if (!string.IsNullOrEmpty(addinfo.CountryName))
                        info.CountryName = addinfo.CountryName;
                }
                string compadd = info.Thoroughfare + "\n" + info.SubLocality + "," + info.Locality + "\n" + info.AdminArea + "," + info.PostalCode + "\n" + info.CountryName;
                string[] ad = combAdd.Split(',');
                string subAdd = string.Empty;
                foreach (string feu in ad)
                {
                    if (!compadd.Contains(feu))
                    {
                        subAdd += feu + "\n";
                    }
                }
                string address = subAdd + compadd;
                AppSettings.Location = address;
                await Navigation.PushAsync(new MasterDetail());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MasterDetail--Location_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            location.IsEnabled = true;
        }
    }
}