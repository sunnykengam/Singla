using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using CheckBox = XLabs.Forms.Controls.CheckBox;

namespace SinglaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingOrders : ContentPage
    {
        List<Orders> pendingList = new List<Orders>();
        List<CustomerMaster> custdata = new List<CustomerMaster>();
        List<Orders> chkboxes = new List<Orders>();
        string custCode;
        string custName;
        int smanCode ;
        public SQLiteAsyncConnection database;

        public PendingOrders()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                pndlist.RefreshCommand = new Command(() =>
                {
                    GetPendingOrders();
                    pndlist.IsRefreshing = false;
                });
                GetPendingOrders();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void GetPendingOrders()
        {
            try
            {
                pendingList.Clear();
                custdata.Clear();
                List<CustomerMaster> list = new List<CustomerMaster>();
                if (AppSettings.PendingOrderList != null)
                    AppSettings.PendingOrderList.Clear();
                if (AppSettings.customerdetail == null)
                {
                    list = Task.Run(async () => await database.Table<CustomerMaster>().ToListAsync()).Result;
                    AppSettings.customerdetail = new ObservableCollection<CustomerMaster>(list);
                }
                list = AppSettings.customerdetail.ToList();
                var pendlist = Task.Run(async () => await database.Table<Orders>().Where(i => i.AzureStatus == "Pending.").ToListAsync()).Result;
                foreach (var dta in list)
                {
                    CustomerMaster ite = new CustomerMaster();
                    var deta = pendlist.Where(i => i.customer_id == dta.customerid.ToString()).FirstOrDefault();
                    if (deta == null)
                    {
                        ite.customername = dta.customercode + "-" + dta.customername;
                        ite.salesmancode = dta.salesmancode;
                        custdata.Add(ite);
                    }
                }
                if (pendlist.Count != 0)
                {
                    foreach (var pendord in pendlist)
                    {
                        var custdate = AppSettings.customerdetail.Where(i => i.customerid == pendord.customer_id).FirstOrDefault();
                        if (AppSettings.loginname == "Salesman Login")
                        {
                            var sales = Task.Run(async () => await database.Table<SalesmanMaster>().Where(i => i.salesmanid == pendord.Sman).FirstOrDefaultAsync()).Result;
                            if (sales != null)
                            {
                                Orders ord = new Orders();
                                ord.customername = custdate.customercode + "-" + pendord.customername;
                                ord.isenable = false;
                                ord.editisenable = true;
                                ord.editisvisible = false;
                                ord.editisvisible = true;
                                ord.saveisvisible = false;
                                ord.bgmcolor = "WhiteSmoke";
                                ord.frmitemcolor = "#FFD700";
                                ord.pickcustomer = custdata;
                                ord.NoOfItems = pendord.NoOfItems;
                                ord.customer_id = pendord.customer_id;
                                ord.partnercode = pendord.partnercode;
                                ord.Sman = pendord.Sman;
                                var selecteditem = chkboxes.Where(i => i.customer_id == pendord.customer_id).FirstOrDefault();
                                if (selecteditem != null)
                                    ord.IsChecked = true;
                                pendingList.Add(ord);
                            }
                        }
                        else
                        {
                            if (AppSettings.salesmanid == pendord.Sman && AppSettings.customerid == pendord.customer_id)
                            {
                                Orders ord = new Orders();
                                ord.customername = custdate.customercode + "-" + pendord.customername;
                                ord.isenable = false;
                                ord.editisvisible = true;
                                ord.saveisvisible = false;
                                ord.editisenable = true;
                                ord.editisvisible = false;
                                ord.bgmcolor = "WhiteSmoke";
                                ord.frmitemcolor = "#FFD700";
                                ord.pickcustomer = custdata;
                                ord.NoOfItems = pendord.NoOfItems;
                                ord.customer_id = pendord.customer_id;
                                ord.partnercode = pendord.partnercode;
                                ord.Sman = pendord.Sman;
                                var selecteditem = chkboxes.Where(i => i.customer_id == pendord.customer_id).FirstOrDefault();
                                if (selecteditem != null)
                                    ord.IsChecked = true;
                                pendingList.Add(ord);
                            }
                        }
                    }
                    pndlist.ItemsSource = null;
                    pndlist.ItemsSource = pendingList;
                    AppSettings.PendingOrderList = pendingList;
                }
                else
                {
                    btnremovie.IsVisible = false;
                    btnconform.IsVisible = false;
                }
                ToastNotification.TostMessage("There are " + pendingList.Count + " PendingOrders");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--GetPendingOrders--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                GetPendingOrders();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--OnAppearing--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected async override void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();
                pndlist.ItemsSource = null;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--OnDisappearing--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected virtual bool OnBackButtonPressed()
        {
            try
            {
                Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--OnBackButtonPressed--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            return true;
        }

        private void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                var Item =(Orders)((Xamarin.Forms.CheckBox)sender).BindingContext;
                if (Item != null)
                {
                    string customercode = Item.customer_id;
                    var chkitem = chkboxes.Where(i => i.customer_id == customercode).FirstOrDefault();
                    var selecteditem = AppSettings.PendingOrderList.Where(i => i.customer_id == customercode).FirstOrDefault();
                    if (Item.IsChecked == true)
                    {
                        if(chkitem==null)
                            chkboxes.Add(selecteditem);
                    }
                    else
                    {
                        chkboxes.Remove(chkitem);
                    }
                }

            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--CheckBox_CheckedChanged--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void  Btnremovie_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (chkboxes.Count != 0)
                {
                    var action = await DisplayAlert("DELETE", "Are you sure? Do you want to remove", "YES", "NO");
                    if (action)
                    {
                        foreach (Orders prdData in chkboxes)
                        {
                            if (AppSettings.CartItems != null)
                                AppSettings.CartItems.Clear();
                            string productQry = "delete from orderitems where customer_id='" + prdData.customer_id + "' and AzureStatus='Pending.'";
                            var productData = Task.Run(async () => await database.QueryAsync<orderitems>(productQry));
                            string custQry = "delete from orders where customer_id='" + prdData.customer_id + "' and AzureStatus='Pending.'";
                            var custData = Task.Run(async () => await database.QueryAsync<Orders>(custQry));
                            var additem = AppSettings.PendingOrderList.Where(i => i.customer_id == prdData.customer_id).FirstOrDefault();
                            AppSettings.PendingOrderList.Remove(additem);
                        }
                        if (chkboxes.Count == 1)
                        {
                            ToastNotification.TostMessage(chkboxes.Count + " Order Removed");
                        }
                        else
                        {
                            ToastNotification.TostMessage(chkboxes.Count + " Orders Removed");
                        }
                        if (AppSettings.PendingOrderList.Count != 0)
                        {
                            pndlist.ItemsSource = null;
                            pndlist.ItemsSource = AppSettings.PendingOrderList;
                        }
                        else
                        {
                            ToastNotification.TostMessage("No PendingOrders");
                            pndlist.ItemsSource = null;
                            pndlist.ItemsSource = AppSettings.PendingOrderList;
                        }
                        chkboxes.Clear();
                    }
                }
                else
                {
                    ToastNotification.TostMessage("Please Select");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--Btnremovie_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Btnconform_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (chkboxes.Count != 0)
                {
                    btnconform.IsEnabled = false;
                    AppSettings.PendingOrderStatus = true;
                    await Navigation.PushAsync(new CartConform());
                }
                else
                {
                    ToastNotification.TostMessage("Please Select");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--Btnconform_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            btnconform.IsEnabled = true;
        }

        private void custedit_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = ((Button)sender).BindingContext;
                var edititem = pendingList.Where(i => i.isenable == true).FirstOrDefault();
                bool editisenable = Convert.ToBoolean(item.GetType().GetProperty("editisenable").GetValue(item, null));
                editisenable = false;
                if (edititem != null)
                {
                    GetPendingOrders();
                }
                string customername = Convert.ToString(item.GetType().GetProperty("customername").GetValue(item, null));
                custCode = Convert.ToString(item.GetType().GetProperty("customer_id").GetValue(item, null));
                string[] custdata = customername.Split('-');
                AppSettings.customercode = custdata[0];
                custName = custdata[1];
                var Additem = pendingList.Where(i => i.customer_id == custCode).FirstOrDefault();
                smanCode = Additem.Sman;
                Additem.editisenable = false;
                Additem.Saveisenable = true;
                Additem.editisvisible = false;
                Additem.saveisvisible = true;
                Additem.isenable = true;
                Additem.bgmcolor = "White";
                pndlist.ItemsSource = null;
                pndlist.ItemsSource = pendingList;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--custedit_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void custSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = ((Button)sender).BindingContext;
                string customercode = Convert.ToString(item.GetType().GetProperty("customer_id").GetValue(item, null));
                var qry = "update Orders set customer_id='" + custCode + "',customername='" + custName + "',sman='" + smanCode + "' where customer_id ='" + customercode + "'";
                var DATA = Task.Run(async () => await database.QueryAsync<Orders>(qry)).Result;
                var qry1 = "update orderitems set customer_id='" + custCode + "',sman='" + smanCode + "' where customer_id ='" + customercode + "'";
                var DATA1 = Task.Run(async () => await database.QueryAsync<orderitems>(qry1)).Result;
                GetPendingOrders();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--custSave_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void Pickcustomer_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var selectedItem = ((Picker)sender).SelectedItem;
                if (selectedItem != null)
                {
                    string customername = Convert.ToString(selectedItem.GetType().GetProperty("customername").GetValue(selectedItem, null));
                    string[] custdata = customername.Split('-');
                    AppSettings.customercode = custdata[0];
                    custName = custdata[1];
                    string custcode = custdata[0];
                    int custid = Convert.ToInt32(custCode);
                    var custdate = AppSettings.customerdetail.Where(i => i.customercode == custcode).FirstOrDefault();
                    custCode = custdate.customerid.ToString();
                    smanCode = Convert.ToInt32(selectedItem.GetType().GetProperty("salesmancode").GetValue(selectedItem, null));
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--Pickcustomer_Unfocused--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void onStackPndlistTapped(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(1);
                var item = ((StackLayout)sender).BindingContext;
                AppSettings.customerid = Convert.ToString(item.GetType().GetProperty("customer_id").GetValue(item, null));
                var customername = Convert.ToString(item.GetType().GetProperty("customername").GetValue(item, null));
                string[] custdata = customername.Split('-');
                AppSettings.customercode = custdata[0];
                AppSettings.customername = custdata[1];
                await Navigation.PushAsync(new TakeOrder());
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("PendingOrders--onStackPndlistTapped--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
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
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }
    }
}