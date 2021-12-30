
using SQLite;
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

namespace SinglaMedicos.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Inventory : ContentPage
	{
        List<ItemMaster> listitems = new List<ItemMaster>();
        List<ItemMaster> serchitems = new List<ItemMaster>();
        public SQLiteAsyncConnection database;
        public Inventory ()
		{
			InitializeComponent ();
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
            database = new SQLiteAsyncConnection(path);
            try
            {
                endDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Now);
                startDatePicker.SetValue(DatePicker.MaximumDateProperty, DateTime.Today.AddDays(-7));
                if (AppSettings.AllItemdeatial == null)
                {
                    AppSettings.AllItemdeatial = Task.Run(async () => await database.Table<ItemMaster>().ToListAsync()).Result;
                }
                listitems = AppSettings.AllItemdeatial;
                AppSettings.Itemdeatial = listitems;
                if (AppSettings.loginname == "Customer Login")
                {
                    var list = listitems.Where(i => i.stock > 0).Select(i => { i.stock1 = "Y"; return i; }).ToList();
                    var items = listitems.Where(i => i.stock <= 0).Select(i => { i.stock1 = "N"; return i; }).ToList();
                    list.AddRange(items);
                    listitems = list;
                }
                else
                {
                    listitems = listitems.Select(i => { i.stock1 = i.stock.ToString(); return i; }).ToList();
                }
                if (AppSettings.ProductName== "New Products")
                {
                    Title = "New Products";
                    invstk.IsVisible = true;
                    var frsdate = startDatePicker.Date;
                    var lstdate = endDatePicker.Date;
                    listitems = listitems.Where(i => i.creationdate >= frsdate && i.creationdate <= lstdate).ToList();
                }
                ToastNotification.TostMessage("Total Products " + listitems.Count);
                inventoryList.ItemsSource = listitems;
                AppSettings.AllItem_list= listitems;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Inventory--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected async override void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();
                await Task.Delay(1);
                searchbar1.Unfocus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Inventory--OnDisappearing--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                Analytics.TrackEvent("Inventory--search_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                Analytics.TrackEvent("Inventory--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var data = ((SearchBar)sender).Text;
                if (data == "")
                {
                    inventoryList.ItemsSource = listitems;
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        string qry = string.Empty;
                        bool status = System.Text.RegularExpressions.Regex.IsMatch(searchbar1.Text, @"\D+");
                        if (status)
                        {
                            qry = "Select itemname,stock,manufacturer,packsize,Scheme,mrp,boxsize,ptr,frmstockcolor From Itemmaster where itemnamesearch like'" + keyword.Replace(' ', '%') + "%' order by itemname";
                        }
                        else
                        {
                            qry = "Select itemname,stock,manufacturer,packsize,Scheme,mrp,boxsize,ptr,frmstockcolor From Itemmaster where itemcode like'" + keyword + "%' order by itemname";
                        }
                        serchitems = Task.Run(async () => await database.QueryAsync<ItemMaster>(qry)).Result;
                        inventoryList.ItemsSource = serchitems;
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Inventory--Searchbar_TextChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                Analytics.TrackEvent("Inventory--StartDatePicker_DateSelected--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                Analytics.TrackEvent("Inventory--EndDatePicker_DateSelected--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Getdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                await getdate.ScaleTo(3, 100);
                await getdate.ScaleTo(1, 500, Easing.SpringOut);
                DateTime strDate = startDatePicker.Date;
                DateTime endDate = endDatePicker.Date;
                listitems = AppSettings.AllItemdeatial.Where(i => i.creationdate >= strDate && i.creationdate <= endDate).ToList();
                if (AppSettings.loginname == "Customer Login")
                {
                    var list = listitems.Where(i => i.stock > 0).Select(i => { i.stock1 = "Y"; return i; }).ToList();
                    var items = listitems.Where(i => i.stock <= 0).Select(i => { i.stock1 = "N"; return i; }).ToList();
                    list.AddRange(items);
                    listitems = list;
                }
                else
                {
                    listitems = listitems.Select(i => { i.stock1 = i.stock.ToString(); return i; }).ToList();
                }
                ToastNotification.TostMessage("Total Products " + listitems.Count);
                inventoryList.ItemsSource = listitems;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Inventory--Getdate_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }


        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                inventoryList.ItemsSource = listitems;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Inventory--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }
    }
}