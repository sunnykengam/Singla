using PickList.Model;
using PickList.ViewModel;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using static PickList.ViewModel.CheckerDetailViewModel;
using CheckBox = XLabs.Forms.Controls.CheckBox;

namespace PickList.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManagePickingDetail : ContentPage
	{
        public SQLiteAsyncConnection database;

        public ManagePickingDetail()
		{
            try
            {
                InitializeComponent();
                this.BindingContext = new CustomerDetailViewModel();
                BackgroundImage = "BackgroundImage.png";
                //var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
                //database = new SQLiteAsyncConnection(path);
                //popupLoadingView.IsVisible = false;
                //custname.Text = AppSettings.PhoneCode;
                //RefreshData();
                //Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                //{
                //    Task.Run(() =>
                //    {
                //        if (CrossConnectivity.Current.IsConnected)
                //        {
                //            if (AppSettings.CheckerUnpickList != null)
                //            {
                //                if (AppSettings.CheckerUnpickList.Count != 0)
                //                {
                //                    try
                //                    {
                //                        labelunpickcount.Text = AppSettings.CheckerUnpickList.Count.ToString();
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                    }
                //                }
                //            }
                //        }
                //    }).ConfigureAwait(true);
                //    return true;
                //});
            }
            catch (Exception ex)
            {
            }
        }

        //public void RefreshData()
        //{
        //    try
        //    {
        //        if (CrossConnectivity.Current.IsConnected)
        //        {

        //            var CheckerInfo = GetAPIData.GetPhoneCustomerDetail(AppSettings.PhoneCode);
        //            AppSettings.customerDetails = CheckerInfo;
        //            if (CheckerInfo != null)
        //            {
        //                if (CheckerInfo.Count != 0)
        //                {
        //                    AppSettings.customerDetails = CheckerInfo;
        //                    datagrid.IsVisible = false;
        //                    datagrid.ItemsSource = null;
        //                    datagrid.ItemsSource = AppSettings.customerDetails;
        //                    datagrid.IsVisible = true;
        //                }
        //            }
        //            else
        //            {
        //                ToastNotification.TostMessage("Please contact Admin Connection");
        //            }
        //            //labelunpickcount.Text = AppSettings.CheckerUnpickList.Count.ToString();
        //        }
        //        else
        //        {
        //            ToastNotification.TostMessage("Please Check Your InterNet Connection");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected override bool OnBackButtonPressed()
        {
            BackButton();
            return true;
        }

        public async void BackButton()
        {
            try
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
            }
            catch (Exception ex)
            {
            }
        }
        private async void GetBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = ((Button)sender).BindingContext;
                var vm = (CustomerDetailViewModel)BindingContext;
                vm.GetBtnClickedCommand.Execute(item);
            }
            catch (Exception ex)
            {
            }
        }

        //private async void GetBtn_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (CrossConnectivity.Current.IsConnected)
        //        {
        //            popupLoadingView.IsVisible = true;
        //            await Task.Delay(2);
        //            var Item = ((Button)sender).BindingContext;
        //            string custcode = Item.GetType().GetProperty("custcode").GetValue(Item, null).ToString();
        //            string invdt = Item.GetType().GetProperty("invdt").GetValue(Item, null).ToString();
        //            string Block = Item.GetType().GetProperty("Block").GetValue(Item, null).ToString();
        //            string Invnum = Item.GetType().GetProperty("Invnum").GetValue(Item, null).ToString();
        //            AppSettings.Block = Block.Trim();
        //            AppSettings.Invnum = Invnum.Trim();
        //            var cust_code = custcode.Trim();
        //            AppSettings.customerDetail = AppSettings.customerDetails.Where(i => i.custcode.Trim() == cust_code && i.Invnum.Trim()== AppSettings.Invnum).FirstOrDefault();
        //            var localPickList = Task.Run(async () => await database.Table<PickingDetail>().Where(k => k.PhoneId == AppSettings.PhoneCode && k.CustCode == cust_code && k.Block == AppSettings.Block && k.Invnum== AppSettings.Invnum).ToListAsync()).Result;
        //            if (localPickList.Count == 0)
        //            {
        //                if (CrossConnectivity.Current.IsConnected)
        //                {
        //                    var PickList = GetAPIData.GetpickingMasterlist(AppSettings.customerDetail.phone.Trim(), custcode.Trim(), invdt.ToString(), AppSettings.Block, AppSettings.Invnum);
        //                    string qry = "delete from PickingDetail";
        //                    var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
        //                    var daata = Task.Run(async () => await database.InsertAllAsync(PickList)).Result;
        //                }
        //                else
        //                {
        //                    await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
        //                }
        //            }
        //            await Navigation.PushAsync(new Detailpage());
        //        }
        //        else
        //        {
        //            ToastNotification.TostMessage("Please Check Your InterNet Connection");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    popupLoadingView.IsVisible = false;
        //}

        //private async void BtnUnpicked_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        await relabox.ScaleTo(3, 100);
        //        await relabox.ScaleTo(1, 500, Easing.SpringOut);
        //        await Navigation.PushAsync(new UnPickedList());
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private async void Logout_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        await Logout.ScaleTo(2, 100);
        //        await Logout.ScaleTo(1, 300, Easing.SpringOut);
        //        var action = await DisplayAlert("Exit?", "Are you sure to Logout", "Yes", "No");
        //        if (action)
        //        {
        //            GetAPIData.UpdateLoginStatus("LoggedOut");
        //            await Navigation.PushAsync(new MainPage());
        //            AppSettings.PhoneCode = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private async void Refresh_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        popupLoadingView.IsVisible = true;
        //        await Task.Delay(2);
        //        await Refresh.ScaleTo(2, 100);
        //        await Refresh.ScaleTo(1, 300, Easing.SpringOut);
        //        RefreshData();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    popupLoadingView.IsVisible = false;
        //}


    }
}