using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using SinglaApp.View.Reports;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;
using System.Collections.ObjectModel;

namespace SinglaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerList : ContentPage
    {
        public SQLiteAsyncConnection database;
        List<CustomerMaster> listitems = new List<CustomerMaster>();
        List<CustomerMaster> serchitems = new List<CustomerMaster>();

        public CustomerList()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                if (AppSettings.customerdetail == null)
                {
                    var lst = Task.Run(async () => await database.Table<CustomerMaster>().Where(i => i.partnercode == AppSettings.partnercode).ToListAsync()).Result;
                    AppSettings.customerdetail = new ObservableCollection<CustomerMaster>(lst);
                }
                //foreach (var item in AppSettings.customerdetail)
                //{
                //    CustomerMaster Data = new CustomerMaster();
                //    if (Device.Idiom == TargetIdiom.Phone)
                //        Data.customername = item.customername.Length <= 20 ? item.customername : item.customername.Substring(0, 20) + "..";
                //    else
                //        Data.customername = item.customername;

                //    Data.customercode = item.customercode;
                //    Data.customerid = item.customerid;
                //    lblcount.Text = Convert.ToString(AppSettings.customerdetail.Count);
                //    string custcode =  item.customerid.ToString();
                //    var cmplist = Task.Run(async () => await database.Table<Orders>().Where(i => i.customer_id == custcode && i.AzureStatus == "Complete.").ToListAsync()).Result;
                //    if (cmplist.Count == 0)
                //    {
                //        Data.totalamount = 0;
                //    }
                //    else
                //    {
                //        float totalamount = 0;
                //        foreach (var items in cmplist)
                //        {
                //            totalamount = totalamount + (float)items.Amt;
                //        }
                //        Data.totalamount = totalamount;
                //    }
                //    listitems.Add(Data);
                //}
                lblcount.Text = Convert.ToString(AppSettings.customerdetail.Count);
                custlist.ItemsSource = AppSettings.customerdetail.OrderBy(i => i.customername).Take(30);
                ToastNotification.TostMessage("Total Customers " + AppSettings.customerdetail.Count);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private void Custlist_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                stcklist2.IsVisible = true;
                var item = ((ListView)sender).SelectedItem;
                var Custcode = Convert.ToString(item.GetType().GetProperty("customercode").GetValue(item, null));
                var cust = listitems.Where(i => i.customercode == Custcode).FirstOrDefault();
                AppSettings.Custcode = Custcode;
                AppSettings.CustID = cust.customerid.ToString();
                var custname = Convert.ToString(item.GetType().GetProperty("customername").GetValue(item, null));
                lblcustomercode.Text = Custcode;
                lblcustomername.Text = custname;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--Custlist_ItemSelected--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void MyProfile_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please Wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(1);
                await Navigation.PushAsync(new MyProfile());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--MyProfile_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void ViewPendingBills_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please Wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(1);
                AppSettings.singleCustpendingbill = true;
                AppSettings.customerid = AppSettings.CustID;
                await Navigation.PushAsync(new CustomerPendingBill());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--ViewPendingBills_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void search_Tapped(object sender, EventArgs e)
        {
            try
            {
                NavigationPage.SetHasNavigationBar(this, false);
                searchbar.IsVisible = true;
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    lblLoadingText.Text = "Please Wait...";
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                }
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
            });
            return true;
        }

        private async void Backbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please Wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var data = ((SearchBar)sender).Text;
                if (data == "")
                {
                    custlist.ItemsSource = listitems.Take(30);
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        serchitems = listitems.Where(C => C.customername.Contains(keyword.ToUpper()) || C.customercode.Contains(keyword.ToUpper())).ToList();
                        custlist.ItemsSource = serchitems;
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--Searchbar_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private void onStackCitizenReporterTapped(object sender, EventArgs e)
        {
            stcklist2.IsVisible = false;
        }

        private async void ViewAllOrders_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please Wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(1);
                AppSettings.singleCustpendingbill = true;
                await Navigation.PushAsync(new CompleteOrders(new ViewModel.CompleteViewModel()));
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--ViewAllOrders_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                custlist.ItemsSource = listitems;
                NavigationPage.SetHasNavigationBar(this, true);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerList--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
    }
}