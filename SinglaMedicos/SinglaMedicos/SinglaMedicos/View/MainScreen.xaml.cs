using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaMedicos.Common;
using SinglaMedicos.Model;
using SinglaMedicos.View.Otif;
using SinglaMedicos.View.Reports;
using SinglaMedicos.View.Syncing;
using SinglaMedicos.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainScreen : ContentPage
    {
        public SQLiteAsyncConnection database;
        private double width = 0;
        private double height = 0;

        public MainScreen()
        {
            try
            {
                InitializeComponent();
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                if (AppSettings.versionupdate)
                {
                    versionupdate.IsVisible = true;
                }
                Marqueelable();
                var partitems = Task.Run(async () => await database.Table<Partner>().Where(i => i.Code == AppSettings.partnercode).FirstOrDefaultAsync()).Result;
                lblstoklist.Text = partitems.Name;
                if (!string.IsNullOrEmpty(AppSettings.Location))
                {
                    popupLocation.IsVisible = true;
                    lblLocation.Text = AppSettings.Location;
                }
               
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Marqueelable()
        {
            try
            {
                double dblWidth = lblsyncMessage.Width + lblsyncMessage.Width;
                // Infinite loop
                while (true)
                {
                    if (lblsyncMessage.TextColor == Color.Black)
                        lblsyncMessage.TextColor = Color.Blue;
                    else if (lblsyncMessage.TextColor == Color.Blue)
                        lblsyncMessage.TextColor = Color.Red;
                    else if (lblsyncMessage.TextColor == Color.Red)
                        lblsyncMessage.TextColor = Color.Green;
                    else
                        lblsyncMessage.TextColor = Color.Black;

                    lblsyncMessage.IsVisible = false;
                    await lblsyncMessage.TranslateTo(dblWidth / 2, 0, 500, Easing.SinIn);
                    lblsyncMessage.IsVisible = true;
                    await lblsyncMessage.TranslateTo(-(dblWidth / 2), 0, 2000, Easing.SinIn);

                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Marqueelable--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Btnlocation_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLocation.IsVisible = false;
                AppSettings.Location = string.Empty;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Btnlocation_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

     
        private async void Inventory_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                AppSettings.ProductName = "Inventory";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Inventory.ScaleTo(3, 100);
                await Inventory.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new Inventory());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Inventory_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Newproducts_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                AppSettings.ProductName = "New Products";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await newproducts.ScaleTo(3, 100);
                await newproducts.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new Inventory());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Newproducts_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void PendingBill_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await PendingBill.ScaleTo(3, 100);
                await PendingBill.ScaleTo(1, 500, Easing.SpringOut);
                AppSettings.singleCustpendingbill = false;
                await Navigation.PushAsync(new PendingBill(new ViewModel.CompleteOutstanding()));
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Newproducts_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void CustomerList_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await CustomerList.ScaleTo(3, 100);
                await CustomerList.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new CustomerList());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--CustomerList_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Delivery_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Delivery.ScaleTo(3, 100);
                await Delivery.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new Delivery());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Delivery_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void neworders_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await neworders.ScaleTo(3, 100);
                await neworders.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new NewOrders());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--neworders_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Orders_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Orders.ScaleTo(3, 100);
                await Orders.ScaleTo(1, 500, Easing.SpringOut);
                AppSettings.TitelName = "Orders";
                AppSettings.singleCustpendingbill = false;
                await Navigation.PushAsync(new CompleteOrders(new ViewModel.CompleteViewModel()));
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Orders_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void PendingOrders_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await PendingOrders.ScaleTo(3, 100);
                await PendingOrders.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new PendingOrders());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--PendingOrders_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void SyncUpdatedata_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await SyncUpdatedata.ScaleTo(3, 100);
                await SyncUpdatedata.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new SyncPage());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--SyncUpdatedata_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Reports_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Reports.ScaleTo(3, 100);
                await Reports.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new Reports.Report());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--Reports_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        protected override bool OnBackButtonPressed()
        {
            BackButtonPressed();
            return true;
        }

        public async Task BackButtonPressed()
        {
            try
            {
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                if (await DisplayAlert("Exit?", "Are you sure? Do you want to close the App", "Yes", "No"))
                {
                    DependencyService.Get<IFileHelper>().closeApplication();
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--BackButtonPressed--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void CompanyList_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await CompanyList.ScaleTo(3, 100);
                await CompanyList.ScaleTo(1, 500, Easing.SpringOut);
                await Navigation.PushAsync(new CompanyList(new ViewModel.Company.MGCompleteViewModel()));
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--CompanyList_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
            //await Feedback.ScaleTo(3, 100);
            //await Feedback.ScaleTo(1, 500, Easing.SpringOut);
        }

        private async void Versupdate_Clicked(object sender, EventArgs e)
        {
            await DependencyService.Get<ILatest>().OpenAppInStore();
        }

        private void Verscancel_Clicked(object sender, EventArgs e)
        {
            versionupdate.IsVisible = false;
        }

        private async void OrderHistory_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    lblLoadingText.Text = "Please wait...";
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await OrderHistory.ScaleTo(3, 100);
                    await OrderHistory.ScaleTo(1, 500, Easing.SpringOut);
                    AppSettings.TitelName = "Orders History";
                    await Navigation.PushAsync(new CompleteOrders(new ViewModel.CompleteViewModel()));
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }

            catch (Exception ex)
            {
                Analytics.TrackEvent("MainScreen--SyncUpdatedata_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }
    }
}