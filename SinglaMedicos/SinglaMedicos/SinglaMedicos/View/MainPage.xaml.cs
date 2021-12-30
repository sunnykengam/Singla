using SQLite;
using System;
using System.Threading.Tasks;
using SinglaMedicos.Common;
using SinglaMedicos.Model;
using SinglaMedicos.View;
using SinglaMedicos.ViewModel;
using Xamarin.Forms;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                BackgroundColor = Color.White;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainPage--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void saleslogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                AppSettings.loginname = "Salesman Login";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainPage--saleslogin_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async  void customerLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                AppSettings.loginname = "Customer Login";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MainPage--customerLogin_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(null, "Are you sure you want to exit the App ?", "Yes", "No"))
                {
                    base.OnBackButtonPressed();

                    var closer = DependencyService.Get<IFileHelper>();
                    if (closer != null)
                        closer.closeApplication();
                }
            });
            return true;
        }
    }
}
