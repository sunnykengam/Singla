using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using SinglaApp.Model.Company;
using SinglaApp.ViewModel.Company;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanyList : ContentPage
    {
        public ObservableRangeCollection<MGCompleteViewModel> completelist { get; private set; }
          = new ObservableRangeCollection<MGCompleteViewModel>();
        public SQLiteAsyncConnection database;
        List<ItemMaster> itemdata = new List<ItemMaster>();
        string SearchBtnName = null;
        bool serchisvisible = true;

        private MGCompleteViewModel ViewModel
        {
            get { return (MGCompleteViewModel)BindingContext; }
            set { BindingContext = value; }
        }

        public CompanyList(MGCompleteViewModel viewModel)
        {
            try
            {
                InitializeComponent();
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                this.ViewModel = viewModel;
                ViewModel.LoadDataCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                lblLoadingText.Text = "Please Wait...";
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Navigation.PopAsync();
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
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;

            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void search_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (serchisvisible)
                {
                    stacksearch.IsVisible = true;
                    serchisvisible = false;
                    searchbar.IsVisible = false;
                }
                else
                {
                    stacksearch.IsVisible = false;
                    serchisvisible = true;
                    searchbar.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
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

        private async void SearchByCompany_Clicked(object sender, EventArgs e)
        {
            try
            {
                SearchBtnName = "Company";
                searchbar.IsVisible = true;
                stacksearch.IsVisible = false;
                searchbar1.Placeholder = "Enter CompanyName";
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--SearchByCompany_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void SearchByItemName_Clicked(object sender, EventArgs e)
        {
            try
            {
                SearchBtnName = "Product";
                searchbar.IsVisible = true;
                stacksearch.IsVisible = false;
                searchbar1.Placeholder = "Enter ItemName";
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--SearchByItemName_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Searchbar1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var data = ((SearchBar)sender).Text;
                if (data == "")
                {
                    comptlist.ItemsSource = AppSettings.MGroup_List;
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        if (SearchBtnName == "Product")
                        {
                            var serchitems1 = AppSettings.MGroup_List.Where(o => o.Product_List.Any(p => p.itemname.StartsWith(keyword.ToUpper()))).ToList();
                            comptlist.ItemsSource = null;
                            comptlist.ItemsSource = serchitems1;
                        }
                        else
                        {
                            var serchitems = AppSettings.MGroup_List.Where(C => C.manufacturer.StartsWith(keyword.ToUpper())).ToList();
                            comptlist.ItemsSource = null;
                            comptlist.ItemsSource = serchitems;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--Searchbar1_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                stacksearch.IsVisible = true;
                comptlist.ItemsSource = AppSettings.MGroup_List;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompanyList--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
    }
}