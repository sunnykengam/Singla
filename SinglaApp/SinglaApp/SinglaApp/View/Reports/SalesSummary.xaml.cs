using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using SinglaApp.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp.View.Reports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SalesSummary : ContentPage
	{
        Double GrandTotal = 0.00;
        List<Model.Reports.SalesSummary> serchitems = new List<Model.Reports.SalesSummary>();

        public SalesSummary ()
		{
            try
            {
                InitializeComponent();
                endDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
                startDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime strDate = startDatePicker.Date;
                DateTime endDate = endDatePicker.Date;
                App.Database.GetSalesManSummaryReport(AppSettings.salesmanid.ToString(), strDate, endDate);
                foreach (var lst in AppSettings.salessummarylist)
                {
                    GrandTotal = GrandTotal + (Double)lst.Amount;
                }
                salessummary_List.ItemsSource = AppSettings.salessummarylist;
                lblsalesman.Text = AppSettings.salesmanname;
                if (AppSettings.salessummarylist.Count != 0)
                {
                    lbltarget.Text = AppSettings.salessummarylist[0].salesmantarget.ToString();
                }
                else
                {
                    lbltarget.Text = "0.00";
                }
                lblgrandtotal.Text = GrandTotal.ToString("0.##");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                Analytics.TrackEvent("SalesSummary--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Backbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                });
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var data = ((SearchBar)sender).Text;
                if (data == "")
                {
                    salessummary_List.ItemsSource = AppSettings.salessummarylist;
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        serchitems = AppSettings.salessummarylist.Where(k => k.customername.Contains(keyword.ToUpper())).ToList();
                        salessummary_List.ItemsSource = serchitems;
                    }
                }

            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--Searchbar_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void EndDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                DateTime endDate = Convert.ToDateTime(endDatePicker.Date.ToString("yyyy-MM-dd"));
                startDatePicker.SetValue(DatePicker.MaximumDateProperty, endDate);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--EndDatePicker_DateSelected--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                DateTime strDate = Convert.ToDateTime(startDatePicker.Date.ToString("yyyy-MM-dd"));
                endDatePicker.SetValue(DatePicker.MinimumDateProperty, strDate);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--StartDatePicker_DateSelected--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Getdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(8);
                GrandTotal = 0.00;
                DateTime fromdate = startDatePicker.Date;
                DateTime todate = endDatePicker.Date;
                App.Database.GetSalesManSummaryReport(AppSettings.salesmancode, fromdate, todate);
                foreach (var lst in AppSettings.salessummarylist)
                {
                    GrandTotal = GrandTotal + (Double)lst.Amount;
                }
                salessummary_List.ItemsSource = AppSettings.salessummarylist;
                if (AppSettings.salessummarylist.Count != 0)
                {
                    lbltarget.Text = AppSettings.salessummarylist[0].salesmantarget.ToString();
                }
                else
                {
                    lbltarget.Text = "0.00";
                }
                lblgrandtotal.Text = GrandTotal.ToString("0.##");
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--Getdate_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                salessummary_List.ItemsSource = AppSettings.salessummarylist;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("SalesSummary--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
    }
}