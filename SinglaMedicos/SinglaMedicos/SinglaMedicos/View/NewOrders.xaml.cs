using SinglaMedicos.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using SinglaMedicos.Common;
using Newtonsoft.Json;
using SinglaMedicos.View.Reports;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrders : ContentPage
    {
        public SQLiteAsyncConnection database;  
        List<CustomerMaster> listitems = new List<CustomerMaster>();
        List<CustomerMaster> serchitems = new List<CustomerMaster>();

        public NewOrders()
        {
            try
            {
                InitializeComponent();
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    stcklst1.Margin = new Thickness(20, 10, 20, 10);
                }
                else
                {
                    Stmainlayout.Margin = new Thickness(10, 10, 10, 10);
                    stcklst1.Margin = new Thickness(40, 0, 40, 0);
                    TakeOrder.Margin = new Thickness(65, 0, 0, 0);
                    collectpayment.Margin = new Thickness(35, 0, 0, 0);
                    delivery.Margin = new Thickness(35, 0, 0, 0);
                }
                if (AppSettings.customerdetail == null)
                {
                    var list = Task.Run(async () => await database.Table<CustomerMaster>().Where(i=>i.partnercode==AppSettings.partnercode).ToListAsync()).Result;
                    AppSettings.CustomerListjson = JsonConvert.SerializeObject(list);
                }
                if (AppSettings.customerdetail.Count != 0)
                {
                    AppSettings.partnercode = AppSettings.customerdetail[0].partnercode;
                    foreach (var item in AppSettings.customerdetail)
                    {
                        CustomerMaster Data = new CustomerMaster();
                        Data.customername = item.customername;
                        Data.customercode = item.customercode;
                        listitems.Add(Data);
                    }
                    listitems = listitems.OrderBy(i => i.customername).ToList();
                    ToastNotification.TostMessage("Total Customers " + AppSettings.customerdetail.Count);
                    lst.ItemsSource = listitems;
                }
                else
                    ToastNotification.TostMessage("Please Sync Customers ");

            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }  

        private async void TakeOrder_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await TakeOrder.ScaleTo(3, 100);
                await TakeOrder.ScaleTo(1, 500, Easing.SpringOut);
                if (AppSettings.CartItems != null)
                    AppSettings.CartItems.Clear();
                AppSettings.Pendingorderstatus = false;
                await Navigation.PushAsync(new TakeOrder());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--TakeOrder_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void CollectPayment_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                AppSettings.Custcode = AppSettings.customercode;
                AppSettings.singleCustpendingbill = true;
                await collectpayment.ScaleTo(3, 100);
                await collectpayment.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new CustomerPendingBill());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--CollectPayment_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Delivery_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait Loading Delivery..";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await delivery.ScaleTo(3, 100);
                await delivery.ScaleTo(1, 500, Easing.SpringOut);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--Delivery_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }
       
        private async void lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = ((ListView)sender).SelectedItem;
                AppSettings.customercode = Convert.ToString(item.GetType().GetProperty("customercode").GetValue(item, null));
                var custdata = AppSettings.customerdetail.Where(i => i.customercode == AppSettings.customercode).FirstOrDefault();
                AppSettings.customerid = custdata.customerid.ToString();
                AppSettings.customername = custdata.customername;
                if (custdata.locked != 0)
                {
                    if (custdata.lockreason.Contains("Day Cr.Limit Exceeds"))
                    {
                        await DisplayAlert("Alert : ", custdata.lockreason + "outstandingbal =" + custdata.outstandingbal, "ok");
                    }
                    else if (custdata.lockreason.Contains("DL Expired"))
                    {
                        await DisplayAlert("Alert : ", custdata.lockreason, "ok");
                    }
                }
                else
                {
                    try
                    {
                        string D_date = custdata.DLExpiry.ToString("yyyy-MM-dd");
                        DateTime DL_date = custdata.DLExpiry;
                        TimeSpan diff = DL_date - DateTime.Today;
                        var days = diff.Days;

                        if (days < 180)
                        {
                            if (days < 0)
                                await DisplayAlert("Alert : ", "DL Expired", "ok");

                            else
                                await DisplayAlert("Alert : ", "DL is Expiring in next " + days + " days : " + D_date, "ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        Analytics.TrackEvent("NewOrders--lst_ItemSelected--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                    }
                }
                stcklst1.IsVisible = true;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--lst_ItemSelected--CompleteError : " + AppSettings.salesmancode + "--" + ex.Message);
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
                    lst.ItemsSource = listitems;
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        serchitems = listitems.Where(C => C.customername.Contains(keyword.ToUpper()) || C.customercode.ToString().Contains(keyword.ToUpper())).ToList();
                        lst.ItemsSource = serchitems;
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--Searchbar_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
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
                Analytics.TrackEvent("NewOrders--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                lst.ItemsSource = listitems;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Sale_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait Loading Customer Month Wise Sale..";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Sale.ScaleTo(3, 100);
                await Sale.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new CustomerMonthSale());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("NewOrders--Sale_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }
    }
}