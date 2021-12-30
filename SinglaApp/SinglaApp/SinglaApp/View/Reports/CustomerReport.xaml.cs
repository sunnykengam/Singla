using Microsoft.AppCenter.Analytics;
using SinglaApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp.View.Reports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerReport : ContentPage
	{
		public CustomerReport ()
		{
            try
            {
                InitializeComponent();
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
                BackgroundImage = "Mainbackground.png";
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerReport--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
        }

       

        private async void SummaryOrder_Clicked(object sender, EventArgs e)
        {
        }

       

        private async void Backbutton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}