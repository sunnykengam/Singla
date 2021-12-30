using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SinglaMedicos.Common;
using SinglaMedicos.Model;
using SinglaMedicos.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompleteOrders : ContentPage
    {
        public ObservableRangeCollection<OrdersViewModel> completelist { get; private set; }
           = new ObservableRangeCollection<OrdersViewModel>();
        List<Orders> complist = new List<Orders>();
        public SQLiteAsyncConnection database;
        private CompleteViewModel ViewModel
        {
            get { return (CompleteViewModel)BindingContext; }
            set { BindingContext = value; }
        }
        public CompleteOrders(CompleteViewModel viewModel)
        {
            try
            {
                InitializeComponent();
                comptlist.RefreshCommand = new Command(() =>
                {
                    this.ViewModel = viewModel;
                    ViewModel.LoadDataCommand.Execute(null);
                    comptlist.IsRefreshing = false;
                });
                endDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Now);
                startDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Now);
                AppSettings.Fromdate = startDatePicker.Date.ToString("yyyy-MM-dd");
                AppSettings.Todate = endDatePicker.Date.ToString("yyyy-MM-dd");
                if(AppSettings.singleCustpendingbill==true)
                { 
                    lbltitelname.Text = "Customer Orders";
                    search.IsVisible = false;
                }
                else
                { 
                    lbltitelname.Text = AppSettings.TitelName;
                }
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                if (Device.Idiom != TargetIdiom.Phone)
                {
                    comptlist.RowHeight = 95;
                }
                this.ViewModel = viewModel;
                ViewModel.LoadDataCommand.Execute(null);
            }
            catch (System.Exception ex)
            {
                Analytics.TrackEvent("CompleteOrders--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Navigation.PopAsync();
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
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

        private async void Backbutton_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOrders--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Btnfilter_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                orderfiltering.IsVisible = false;
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(1);
                AppSettings.Fromdate = startDatePicker.Date.ToString("yyyy-MM-dd");
                AppSettings.Todate = endDatePicker.Date.ToString("yyyy-MM-dd");
                ViewModel.LoadDataCommand.Execute(null);
                orderfiltering.IsVisible = false;
                //if (AppSettings.TitelName == "Orders History")
                //{
                    if (AppSettings.status != 1)
                    {
                        if (AppSettings.ShowPendingOrders == true)
                        {
                            if (AppSettings.orderlist != null && AppSettings.orderlist.Count != 0)
                            {
                                var orderhistery = AppSettings.orderlist.Where(i => i.UpdateStatus == "P");
                                comptlist.ItemsSource = orderhistery;
                                ToastNotification.TostMessage("Total Orders" + orderhistery.ToList().Count);
                            }
                            else
                            {
                                comptlist.ItemsSource = AppSettings.orderlist;
                            }
                        }
                        else
                        {
                            comptlist.ItemsSource = AppSettings.orderlist;
                        }
                    }
                    else
                    {
                        if (AppSettings.ShowCompleteOrders == true)
                        {
                            if (AppSettings.orderlist != null && AppSettings.orderlist.Count != 0)
                            {
                                var orderhistery = AppSettings.orderlist.Where(i => i.UpdateStatus == "C" || i.UpdateStatus == "G");
                                comptlist.ItemsSource = orderhistery;
                                ToastNotification.TostMessage("Total Orders" + orderhistery.ToList().Count);
                            }
                            else
                            {
                                comptlist.ItemsSource = AppSettings.orderlist;
                            }
                        }
                        else
                        {
                            comptlist.ItemsSource = AppSettings.orderlist;

                        }
                    }

                //}
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOrders--Btnfilter_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void search_Tapped(object sender, EventArgs e)
        {
            try
            {
                backbutton.IsVisible = false;
                ordserch.IsVisible = false;
                lbltitelname.IsVisible = false;
                searchbar.IsVisible = true;
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOrders--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
        private void ordfilter_Tapped(object sender, System.EventArgs e)
        {
            orderfiltering.IsVisible = true;
        }

        private void Btnclear_Clicked(object sender, System.EventArgs e)
        {
            orderfiltering.IsVisible = false;
        }
        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (AppSettings.orderlist != null)
                {
                    var data = ((SearchBar)sender).Text;
                    if (data == "")
                    {
                        comptlist.ItemsSource = AppSettings.orderlist;
                    }
                    else
                    {
                        var keyword = searchbar1.Text;
                        if (keyword.Length >= 1)
                        {
                            var serchitems = AppSettings.orderlist.Where(C => C.compname.Contains(keyword.ToUpper())).ToList();
                            comptlist.ItemsSource = serchitems;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOrders--Searchbar_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void EndDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime endDate = Convert.ToDateTime(endDatePicker.Date.ToString("yyyy-MM-dd"));
            startDatePicker.SetValue(DatePicker.MaximumDateProperty, endDate);
        }

        private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime strDate = Convert.ToDateTime(startDatePicker.Date.ToString("yyyy-MM-dd"));
            endDatePicker.SetValue(DatePicker.MinimumDateProperty, strDate);
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                lbltitelname.IsVisible = true;
                searchbar.IsVisible = false;
                ordserch.IsVisible = true;
                backbutton.IsVisible = true;
                comptlist.ItemsSource = AppSettings.orderlist;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOrders--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool check = e.Value;
            AppSettings.ShowPendingOrders = check;
            int status = 2;
            AppSettings.status = status;
        }

        private void CheckBox_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
        {
            bool check = e.Value;
            AppSettings.ShowCompleteOrders = check;
            int status = 1;
            AppSettings.status = status;
        }
    }
}