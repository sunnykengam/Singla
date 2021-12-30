using Checking.Models;
using Checking.ViewModel;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using CheckBox = XLabs.Forms.Controls.CheckBox;

namespace Checking.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InvoiceDeatilpage : ContentPage
    {
        ObservableCollection<PickedDetail> pickedItems = new ObservableCollection<PickedDetail>();
        public SQLiteAsyncConnection database;

        public InvoiceDeatilpage ()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Invoiceitems.sqlite");
                database = new SQLiteAsyncConnection(path);
                custname.Text = AppSettings.UserDetail.checkername;
                if(AppSettings.InvoiceDate!= null)
                    AppSettings.InvoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(AppSettings.InvoiceDate))
                {
                    startDatePicker.Date = Convert.ToDateTime(AppSettings.InvoiceDate);
                    pickedItems = GetAPIData.GetPickedDetails(AppSettings.UserDetail.checkercode);
                    foreach (PickedDetail obj in pickedItems)
                    {
                        obj.rowcolor = "Transparent";
                        var picked = GetAPIData.GetCheckingHoldCount(obj.Block, obj.custcode.ToString(), obj.Invnum);
                        if (picked == 0)
                        {
                            obj.rowcolor = "Transparent";
                            obj.IsDisable = true;
                        }
                        else
                        {
                            obj.rowcolor = "Orange";
                            obj.IsDisable = true;
                        }
                        if (obj.Checked == "y")
                        {
                            obj.rowcolor = "Green";
                            obj.IsDisable = false;
                        }
                        else
                        {
                            obj.IsDisable = true;
                        }
                    }
                    datagrid.IsVisible = true;
                    datagrid.ItemsSource = pickedItems.OrderBy(i=>i.Checked);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void Btngetdata_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    popupLoadingView.IsVisible = true;
                    await Task.Delay(2);
                    AppSettings.InvoiceDate = startDatePicker.Date.ToString("yyyy-MM-dd");
                    pickedItems = GetAPIData.GetPickedDetails(AppSettings.UserDetail.checkercode);
                   
                    if (pickedItems.Count == 0)
                    {
                        await DisplayAlert("Alert : ", "No Invoices For Selected Date", "Plese Select Another Date");
                        datagrid.IsVisible = false;
                    }
                    else
                    {
                        foreach (PickedDetail obj in pickedItems)
                        {
                            AppSettings.Block = obj.Block;
                            var picked = GetAPIData.GetCheckingHoldCount(obj.Block, obj.custcode.ToString(), obj.Invnum);
                            if(picked == 0)
                            {
                                obj.rowcolor = "Transparent";
                                obj.IsDisable = true;
                            }
                            else
                            {
                                obj.rowcolor = "Orange";
                                obj.IsDisable = true;
                            }
                            if (obj.Checked == "y")
                            {
                                obj.rowcolor = "Green";
                                obj.IsDisable = false;
                            }
                        }
                        datagrid.IsVisible = false;
                        datagrid.ItemsSource = null;
                        datagrid.IsVisible = true;
                        datagrid.ItemsSource = pickedItems.OrderBy(i => i.Checked);
                    }
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        private async void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                await Task.Delay(1);
                PickedDetail objDetail = new PickedDetail();
                var Item = (PickedDetail) ((Button)sender).BindingContext;
                objDetail = pickedItems.Where(i => i.custcode == Item.custcode && i.Block == Item.Block&&i.Invnum== Item.Invnum).FirstOrDefault();
                AppSettings.PickedDetail = objDetail;
                AppSettings.CustCode = Item.custcode.Trim().ToString();
                AppSettings.Block = Item.Block.Trim();
                AppSettings.InvoiceNo = Item.Invnum.Trim();
                await Navigation.PushAsync(new Detailpage());
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(null, "Are you sure you want to exit this App ?", "Yes", "No"))
                {
                    base.OnBackButtonPressed();

                    var closer = DependencyService.Get<IFileHelper>();
                    if (closer != null)
                        closer.closeApplication();
                }
            });
            return true;
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayAlert("Exit?", "Are you sure to Logout", "Yes", "No");
                if (action)
                {
                    await Navigation.PushAsync(new LoginPage());
                    AppSettings.ClearAllData();
                }
            }
            catch (Exception ex)
            {
            }
        }
        
    }
}