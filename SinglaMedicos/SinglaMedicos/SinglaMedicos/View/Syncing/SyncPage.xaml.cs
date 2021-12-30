using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaMedicos.Common;
using SinglaMedicos.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.View.Syncing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncPage : ContentPage
    {
        public SyncPage()
        {
            try
            {
                InitializeComponent();
                if (AppSettings.loginname == "Salesman Login")
                {
                }
                else
                {
                    btnCustomers.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SyncPage--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void BtnCustomers_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    lblLoadingText.Text = "Customers Syncing...";
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await Task.Delay(8);
                    await btnCustomers.ScaleTo(3, 100);
                    await btnCustomers.ScaleTo(1, 500, Easing.SpringOut);
                    App.Database.CustomerMasterDB();
                    if (AppSettings.customerdetail.Count == 0)
                        ToastNotification.TostMessage("There are no active Customers for this salesman");
                    else
                        ToastNotification.TostMessage("Updated Customers " + AppSettings.customerdetail.Count);
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SyncPage--BtnCustomers_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;

        }

        private async void BtnProducts_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    lblLoadingText.Text = "Products Syncing...";
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await btnProducts.ScaleTo(3, 100);
                    await btnProducts.ScaleTo(1, 500, Easing.SpringOut);
                    App.Database.ItemMasterinsertDB();
                    if (AppSettings.ProductssyncStatus == true)
                    {
                        ToastNotification.TostMessage("please wait Products Syncing In Background");
                    }
                    else
                    {
                        ToastNotification.TostMessage("Updated Items " + AppSettings.AllItemdeatial.Count);
                    }
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SyncPage--BtnProducts_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }
    }
}
