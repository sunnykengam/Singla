using PickList.Model;
using PickList.ViewModel;
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

namespace PickList.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Detailpage : ContentPage
    {
        public SQLiteAsyncConnection database;
        public List<object> chkboxes = new List<object>();
        ObservableCollection<PickingDetail> PickList = new ObservableCollection<PickingDetail>();
        ObservableCollection<PickingDetail> pickingData = new ObservableCollection<PickingDetail>();
            PickingDetail pickeditem = new PickingDetail();
        int i = 1;

        public Detailpage ()
		{
			InitializeComponent ();
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
            database = new SQLiteAsyncConnection(path);
            popupLoadingView.IsVisible = false;
            BackgroundImage = "BackgroundImage.png";
            try
            {
                var custcode = AppSettings.customerDetail.custcode.Trim();
                var localPickList = Task.Run(async () => await database.Table<PickingDetail>().Where(k => k.PhoneId == AppSettings.PhoneCode && k.CustCode == custcode&&k.Block==AppSettings.Block && k.Invnum== AppSettings.Invnum).OrderBy(l => l.Ischecked).OrderBy(i => i.Location).ToListAsync()).Result;
                if (localPickList.Count != 0)
                {
                    PickList = new ObservableCollection<PickingDetail>(localPickList);
                    var lstcount = PickList.Where(i => i.Picked == "Y").ToList();
                    if (localPickList.Count != lstcount.Count)
                    {
                        BasketLoadingView.IsVisible = true;
                    }
                    AppSettings.PickingDateDetails = PickList;
                    string StartTime = DateTime.Now.ToString("HH:mm:ss");
                    GetAPIData.UpdatePickerStartTime(StartTime, AppSettings.Invnum);
                    AppSettings.PickingDateDetails = PickList;
                    foreach (var item in PickList)
                    {
                        item.PhoneId = AppSettings.PhoneCode;
                        item.rowcolor = "Transparent";
                        if (item.Picked == "Y")
                            item.Ischecked = true;

                        if (item.ITEMCATEGORY == 1)
                            item.rowcolor = "#DC143C";//#ColdRoom

                        if (item.ITEMCATEGORY == 2)
                            item.rowcolor = "#00FFFF";//#Drops

                        if (item.ITEMCATEGORY == 3)
                            item.rowcolor = "#F4A460";//#Powders 

                        if (item.ITEMCATEGORY == 4)
                            item.rowcolor = "#00FA9A";//#Syrups

                        if (item.ITEMCATEGORY == 5)
                            item.rowcolor = "#FFD700";//#Injections

                        if (item.ITEMCATEGORY == 6)
                            item.rowcolor = "#EE82EE";//#Office 

                    }
                    invoicedate.Text = Convert.ToDateTime(PickList.FirstOrDefault().Invdt).ToString("dd/MM/yyyy");
                    AppSettings.PickDate = Convert.ToDateTime(PickList.FirstOrDefault().Invdt).ToString("yyyy-MM-dd");
                    if (AppSettings.customerDetail.custname.Length > 11)
                    {
                        lblcustname.Text = AppSettings.customerDetail.custname.Substring(0, 11);
                    }
                    else
                    {
                        lblcustname.Text = AppSettings.customerDetail.custname;
                    }
                    lblinvnum.Text = AppSettings.Invnum;
                    lblCustCode.Text =AppSettings.customerDetail.custcode;
                    lblcheckname1.Text = AppSettings.customerDetail.checkername;
                    datagrid.ItemsSource = null;
                    datagrid.ItemsSource = PickList;
                    ToastNotification.TostMessage("Total Lines" + PickList.Count);
                    //Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                    //{
                    //    Task.Run(() =>
                    //    {
                    //        if (CrossConnectivity.Current.IsConnected)
                    //        {
                                if (AppSettings.CheckerUnpickList != null)
                                {
                                    if (AppSettings.CheckerUnpickList.Count != 0)
                                    {
                                        try
                                        {
                                            labelunpickcount.SetBinding(Label.TextProperty, "CartCounter");
                                            string count = AppSettings.CheckerUnpickList.Count.ToString();
                                            labelunpickcount.Text = count;
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                    //            }
                    //        }).ConfigureAwait(true);
                    //        return true;
                    //    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                await Task.Delay(4);
                App.Database.InsertandUpdatePickListInServer();
                await Navigation.PushAsync(new NotPickedPage());
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            BackButton();
            return true;
        }

        public async void BackButton()
        {
            if (await DisplayAlert("Exit?", "Are you sure? Do you want to exit page", "Yes", "No"))
            {
              await Navigation.PopAsync();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            popupLoadingView.IsVisible = false;
        }

        private void CheckYes_Tapped(object sender, EventArgs e)
        {
            try
            {
                unpick.IsVisible = false;
                int ItemId = Convert.ToInt32(pickeditem.GetType().GetProperty("Itemid").GetValue(pickeditem, null));
                var AddItem = AppSettings.PickingDateDetails.Where(i => i.Itemid == ItemId).FirstOrDefault();
                chkboxes.Remove(pickeditem);
                AddItem.Ischecked = false;
                AddItem.BasketNo = null;
                AddItem.Picked = null;
                AddItem.PickedTime = DateTime.Now.ToString("HH:mm:ss");
                string qry = "update PickingDetail set Ischecked=false,Picked=null,BasketNo=null,PickedTime='" + AddItem.PickedTime + "' where ItemId ='" + AddItem.Itemid + "'";
                var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
            }
            catch (Exception ex)
            {
            }

        }

        private async void CheckNo_Tapped(object sender, EventArgs e)
        {
            try
            {
                CheckNo.IsEnabled = false;
                unpick.IsVisible = false;
                popupLoadingView.IsVisible = true;
                await Task.Delay(2);
                int ItemId = Convert.ToInt32(pickeditem.GetType().GetProperty("Itemid").GetValue(pickeditem, null));
                var AddItem = AppSettings.PickingDateDetails.Where(i => i.Itemid == ItemId).FirstOrDefault();
                AddItem.Ischecked = true;
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = AppSettings.PickingDateDetails.OrderBy(i=>i.Ischecked==true);
            }
            catch (Exception ex)
            {

            }
            popupLoadingView.IsVisible = false;
            CheckNo.IsEnabled = true;
        }

        private async void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                var Item = ((CheckBox)sender).BindingContext;
                pickeditem = (PickingDetail)Item;
                bool Checked = ((CheckBox)sender).Checked;
                int ItemId = Convert.ToInt32(Item.GetType().GetProperty("Itemid").GetValue(Item, null));
                var AddItem = AppSettings.PickingDateDetails.Where(i => i.Itemid == ItemId ).FirstOrDefault();
                if (Checked == true)
                {
                    chkboxes.Add(Item);
                    AddItem.Ischecked = true;
                    AddItem.PickedTime = DateTime.Now.ToString("HH:mm:ss"); 
                    AddItem.BasketNo = AppSettings.CurrentBasketNo;
                    AddItem.Picked = "Y";
                    string qry = "update PickingDetail set Ischecked='" + AddItem.Ischecked + "',Picked='" + AddItem.Picked + "',BasketNo='" + AddItem.BasketNo + "',PickedTime='" + AddItem.PickedTime + "' where ItemId ='" + AddItem.Itemid + "'";
                    var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
                }
                else
                {
                    unpick.IsVisible = true;
                }

                if (AppSettings.CheckerUnpickList != null)
                {
                    if (AppSettings.CheckerUnpickList.Count != 0)
                    {
                        try
                        {
                            labelunpickcount.SetBinding(Label.TextProperty, "CartCounter");
                            string count = AppSettings.CheckerUnpickList.Count.ToString();
                            labelunpickcount.Text = count;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        private async void Hold_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    popupLoadingView.IsVisible = true;
                    await holdbtn.ScaleTo(3, 100);
                    await holdbtn.ScaleTo(1, 500, Easing.SpringOut);
                    App.Database.InsertandUpdatePickListInServer();
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
                nxtbtn.IsVisible = true;
                holdbtn.IsVisible = false;
                datagrid.IsVisible = false;
                submit.IsVisible = false;
                stkchecker.IsVisible = true;
                lblcheckername.Text = AppSettings.customerDetail.checkername;
                lblbranchname.Text = AppSettings.customerDetail.custname;
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }
       
        private async void Finished_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (string.IsNullOrEmpty(BasketNo.Text))
                    {
                        await DisplayAlert("Alert", "Plz Enter Basket no", "", "ok");
                    }
                    else
                    {
                        BasketLoadingView.IsVisible = false;
                        if (AppSettings.CurrentBasketNo != BasketNo.Text)
                        {
                            AppSettings.CurrentBasketNo = BasketNo.Text;
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void Continue_Clicked(object sender, EventArgs e)
        {
            try
            {
             
                BasketNo.Text = null;
                popupLoadingView.IsVisible = true;
                await nxtbtn.ScaleTo(3, 100);
                await nxtbtn.ScaleTo(1, 500, Easing.SpringOut);
                nxtbtn.IsVisible = false;
                holdbtn.IsVisible = true;
                var custcode = AppSettings.customerDetail.custcode.Trim();
                var localPickList = Task.Run(async () => await database.Table<PickingDetail>().Where(k => k.PhoneId == AppSettings.PhoneCode && k.CustCode == custcode).OrderBy(l => l.Ischecked).OrderBy(i => i.Location).ToListAsync()).Result;
                PickList = new ObservableCollection<PickingDetail>(localPickList);
                foreach (var item in PickList)
                {
                    if (item.Picked == "Y")
                        item.Ischecked = true;

                    item.rowcolor = "Transparent";
                    if (item.Picked == "Y")
                        item.Ischecked = true;

                    if (item.ITEMCATEGORY == 1)
                        item.rowcolor = "#DC143C";//#ColdRoom

                    if (item.ITEMCATEGORY == 2)
                        item.rowcolor = "#00FFFF";//#Drops

                    if (item.ITEMCATEGORY == 3)
                        item.rowcolor = "#F4A460";//#Powders 

                    if (item.ITEMCATEGORY == 4)
                        item.rowcolor = "#00FA9A";//#Syrups

                    if (item.ITEMCATEGORY == 5)
                        item.rowcolor = "#FFD700";//#Injections

                    if (item.ITEMCATEGORY == 6)
                        item.rowcolor = "#EE82EE";//#Office 
                }
                datagrid.IsVisible = true;
                datagrid.ItemsSource = PickList;
                BasketLoadingView.IsVisible = true;
                submit.IsVisible = true;
                stkchecker.IsVisible = false;
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        private void cancel_Clicked(object sender, EventArgs e)
        { 
            BasketLoadingView.IsVisible = false;
        }

        private async void BtnUnpicked_Clicked(object sender, EventArgs e)
        {
            await relabox.ScaleTo(3, 100);
            await relabox.ScaleTo(1, 300, Easing.SpringOut);
            await Navigation.PushAsync(new UnPickedList());
        }

        private async void Office_Tapped(object sender, EventArgs e)
        {
            await Office.ScaleTo(3, 100);
            await Office.ScaleTo(1, 300, Easing.SpringOut);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = PickList.OrderByDescending(I=>I.ITEMCATEGORY==6).ToList();
        }

        private async void ColdRoom_Tapped(object sender, EventArgs e)
        {
            await ColdRoom.ScaleTo(3, 100);
            await ColdRoom.ScaleTo(1, 300, Easing.SpringOut);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = PickList.OrderByDescending(I => I.ITEMCATEGORY == 1).ToList(); 
        }

        private async void Injections_Tapped(object sender, EventArgs e)
        {
            await Injections.ScaleTo(3, 100);
            await Injections.ScaleTo(1, 300, Easing.SpringOut);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = PickList.OrderByDescending(I => I.ITEMCATEGORY == 5).ToList();
        }

        private async void Syrups_Tapped(object sender, EventArgs e)
        {
            await Syrups.ScaleTo(3, 100);
            await Syrups.ScaleTo(1, 300, Easing.SpringOut);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = PickList.OrderByDescending(I => I.ITEMCATEGORY == 4).ToList();
        }

        private async void Powders_Tapped(object sender, EventArgs e)
        {
            await Powders.ScaleTo(3, 100);
            await Powders.ScaleTo(1, 300, Easing.SpringOut);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = PickList.OrderByDescending(I => I.ITEMCATEGORY == 3).ToList();
        }

        private async void Drops_Tapped(object sender, EventArgs e)
        {
            await Drops.ScaleTo(3, 100);
            await Drops.ScaleTo(1, 300, Easing.SpringOut);
            datagrid.ItemsSource = null;
            datagrid.ItemsSource = PickList.OrderByDescending(I => I.ITEMCATEGORY == 2).ToList();
        }

        private void BasketNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string qty = ((Entry)sender).Text;
            if (!string.IsNullOrEmpty(qty))
            {
                if (!qty.Contains(".") && !qty.Contains("-") && !qty.Contains(",") && !qty.StartsWith("0"))
                {

                }
                else
                {
                    ((Entry)sender).Text = null;
                    ToastNotification.TostMessage("Plese Enter Valid Number");
                }
            }
        }
    }
}