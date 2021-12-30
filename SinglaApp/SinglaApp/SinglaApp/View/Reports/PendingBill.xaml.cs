using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.View.Reports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PendingBill : ContentPage
	{
        public ObservableRangeCollection<OutstandingViewmodel> compoutstandinglist { get; private set; }
           = new ObservableRangeCollection<OutstandingViewmodel>();
        decimal GrandTotal = 0;
        private CompleteOutstanding ViewModel
        {
            get { return (CompleteOutstanding)BindingContext; }
            set { BindingContext = value; }
        }
        public PendingBill (CompleteOutstanding viewModel)
		{
            try
            {
                InitializeComponent();
                compoutstandinglst.RefreshCommand = new Command(() =>
                {
                    this.ViewModel = viewModel;
                    AppSettings.OutstandingStatus = true;
                    ViewModel.LoadDataCommand.Execute(null);
                    compoutstandinglst.IsRefreshing = false;
                });
                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (AppSettings.OutstandingStatus)
                        {
                            AppSettings.OutstandingStatus = false;
                            Task.Run(() =>
                            {
                                App.Database.GetOutstandingPendingBill(AppSettings.partnercode, AppSettings.salesmanid.ToString());
                            }).ConfigureAwait(true);
                        }
                    }
                    return true;
                });
                this.ViewModel = viewModel;
                ViewModel.LoadDataCommand.Execute(null);
                lblorders.Text = AppSettings.outstandingbill.Count + "  Customers";
                foreach (var data in AppSettings.outstandingbill)
                {
                    GrandTotal = GrandTotal + data.custbalanceamount;
                }
                lbltotalamount.Text = "Rs: ₹ " + GrandTotal;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingBill--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopAsync();
            });
            return true;
        }

        private void StateImage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Source"))
            {
                var image = sender as Image;
                image.Opacity = 0;
                image.FadeTo(1, 1000);
            }
        }
        private async void Backbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingBill--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                Analytics.TrackEvent("PendingBill--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var data = ((SearchBar)sender).Text;
                if (data == "")
                {
                    compoutstandinglst.ItemsSource = AppSettings.outstandingbill;
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        var serchitems = AppSettings.outstandingbill.Where(C => C.customername.StartsWith(keyword.ToUpper())).ToList();
                        compoutstandinglst.ItemsSource = serchitems;
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingBill--Searchbar_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                compoutstandinglst.ItemsSource = AppSettings.outstandingbill;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingBill--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
    }
}