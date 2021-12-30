using Plugin.Connectivity;
using SinglaMedicos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;
using SinglaMedicos.Common;

namespace SinglaMedicos.View.Reports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Report : ContentPage
	{
		public Report ()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Report--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopAsync();
            });
            return true;
        }

        private async void Collection_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Collection.ScaleTo(3, 100);
                await Collection.ScaleTo(1, 500, Easing.SpringOut);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Report--Collection_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Delivery_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Delivery.ScaleTo(3, 100);
                await Delivery.ScaleTo(1, 500, Easing.SpringOut);
            }
            catch (Exception ex)
            {
            }
        }

        private async void SummaryOrder_Clicked(object sender, EventArgs e)
        {
            try
            {
                await SummaryOrder.ScaleTo(3, 100);
                await SummaryOrder.ScaleTo(1, 500, Easing.SpringOut);
            }
            catch (Exception ex)
            {
            }
        }


        private async void PendingBill_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await PendingBill.ScaleTo(3, 100);
                    await PendingBill.ScaleTo(1, 500, Easing.SpringOut);
                    await Navigation.PushAsync(new Reports.SalesSummary());
                }
                else
                {
                    await PendingBill.ScaleTo(3, 100);
                    await PendingBill.ScaleTo(1, 500, Easing.SpringOut);
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Report--PendingBill_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Backbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}