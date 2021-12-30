using PickList.Common;
using PickList.Model;
using PickList.View;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PickList.ViewModel
{
    public class CheckerDetailViewModel : BaseViewModel
    {
        public class CustomerDetailViewModel : BaseViewModel
        {
            public static SQLiteAsyncConnection database;
            public CustomerDetailViewModel()
            {
                try
                {
                    GetLocalDBData();
                    UnPickedclickCommand = new Command(UnPickedclickCommandExecuted);
                    LogoutClickedCommand = new Command(LogoutClickedCommandExecuted);
                    CustomerDetailRefreshCommand = new Command(CustomerDetailRefreshCommandExecuted);
                    GetBtnClickedCommand = new Command<object>((item) => GetBtnClickedCommandExecuted(item));
                }
                catch (Exception ex)
                {
                }
            }

            #region Commands
            public ICommand UnPickedclickCommand { get; private set; }
            private async void UnPickedclickCommandExecuted()
            {
                try
                {
                    ProgressBarPopupVisble = true;
                    await Task.Delay(20);
                    await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new UnPickedList());
                }
                catch (Exception ex)
                {
                }
                ProgressBarPopupVisble = false;
            }
            public ICommand LogoutClickedCommand { get; private set; }
            private async void LogoutClickedCommandExecuted()
            {
                try
                {
                    ProgressBarPopupVisble = true;
                    await Task.Delay(20);
                    var action = await Application.Current.MainPage.DisplayAlert("Exit?", "Are you sure to Logout", "Yes", "No");
                    if (action)
                    {
                        GetAPIData.UpdateLoginStatus("LoggedOut");
                        await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                        AppSettings.PhoneCode = null;
                    }
                }
                catch (Exception ex)
                {
                }
                ProgressBarPopupVisble = false;
            }
            public ICommand CustomerDetailRefreshCommand { get; private set; }
            private async void CustomerDetailRefreshCommandExecuted()
            {
                try
                {
                    ProgressBarPopupVisble = true;
                    await Task.Delay(20);
                    RefreshData();
                }
                catch (Exception ex)
                {
                }
                ProgressBarPopupVisble = false;
            }
            public ICommand GetBtnClickedCommand { get; private set; }
            private async void GetBtnClickedCommandExecuted(object Item)
            {
                try
                {
                    ProgressBarPopupVisble = true;
                    await Task.Delay(20);
                    var ItemDetail = (CustomerDetail)Item;
                    AppSettings.Block = ItemDetail.Block.Trim();
                    AppSettings.Invnum = ItemDetail.Invnum.ToString().Trim();
                   string invnum= ItemDetail.Invnum.ToString();
                    AppSettings.customerDetail = AppSettings.customerDetails.Where(i => i.custcode.Trim() == ItemDetail.custcode.Trim() && i.Invnum == ItemDetail.Invnum).FirstOrDefault();
                    var localPickList = Task.Run(async () => await database.Table<PickingDetail>().Where(k => k.PhoneId == ItemDetail.phone && k.CustCode == ItemDetail.custcode && k.Block == ItemDetail.Block && k.Invnum == invnum).ToListAsync()).Result;
                    if (localPickList.Count == 0)
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            var PickList = GetAPIData.GetpickingMasterlist(AppSettings.customerDetail.phone.Trim(), ItemDetail.custid.ToString(), ItemDetail.invdt.ToString(), AppSettings.Block, AppSettings.Invnum);
                            string qry = "delete from PickingDetail where custcode='" + ItemDetail.custcode + "' and invdt='" + ItemDetail.invdt + "'";
                            var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
                            var daata = Task.Run(async () => await database.InsertAllAsync(PickList)).Result;
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                        }
                    }
                    await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new Detailpage());
                }
                catch (Exception ex)
                {
                }
                ProgressBarPopupVisble = false;
            }
            #endregion

            #region Methods
            private void GetLocalDBData()
            {
                try
                {
                    database = Utilities.GetDbPath();
                    LoginName = AppSettings.PhoneCode;
                    RefreshData();
                    Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                    {
                        Task.Run(() =>
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                if (AppSettings.CheckerUnpickList != null)
                                {
                                    if (AppSettings.CheckerUnpickList.Count != 0)
                                    {
                                        try
                                        {
                                            UnPickedCount = AppSettings.CheckerUnpickList.Count.ToString();
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }).ConfigureAwait(true);
                        return true;
                    });
                }
                catch (Exception ex)
                {
                }
            }
            public void RefreshData()
            {
                try
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        var CustInfo = GetAPIData.GetPhoneCustomerDetail(AppSettings.PhoneCode);
                        if (CustInfo != null)
                        {
                            if (CustInfo.Count != 0)
                            {
                                AppSettings.customerDetails = CustInfo;
                                //foreach (var dat in CustInfo)
                                //{
                                //    var cust4List = Task.Run(async () => await database.Table<CustomerDetail>().ToListAsync()).Result;
                                //    var custList = Task.Run(async () => await database.Table<CustomerDetail>().Where(k => k.phone == AppSettings.PhoneCode && k.custcode == dat.custcode && k.invdt == dat.invdt && k.Invnum == dat.Invnum).ToListAsync()).Result;
                                //    if (custList.Count == 0)
                                //    {
                                //        var daata = Task.Run(async () => await database.InsertAsync(dat)).Result;
                                //    }
                                //}
                                //var localcustList = Task.Run(async () => await database.Table<CustomerDetail>().Where(k => k.phone == AppSettings.PhoneCode).ToListAsync()).Result;
                                //AppSettings.customerDetails = new ObservableCollection<CustomerDetail>(localcustList);
                                datagridIsVisible = false;
                                CustomerList = new ObservableCollection<CustomerDetail>();
                                CustomerList = new ObservableCollection<CustomerDetail>(AppSettings.customerDetails);
                                datagridIsVisible = true;
                            }
                        }
                        else
                        {
                            ToastNotification.TostMessage("Please contact Admin Connection");
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
            }
            #endregion
        }
    }
}
