using Acr.UserDialogs;
using Android.Widget;
using Checking.Models;
using Checking.ViewModel;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using CheckBox = XLabs.Forms.Controls.CheckBox;


namespace Checking.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detailpage : ContentPage
    {
        int i = 1;
        public List<object> chkboxes = new List<object>();
        public Xamarin.Forms.BindingBase ItemDisplayBinding { get; set; }
        ObservableCollection<InvoiceItem> products = new ObservableCollection<InvoiceItem>();
        List<InvoiceItem> serchitems = new List<InvoiceItem>();
        InvoiceItem chickeditem = new InvoiceItem();
        InvoiceItem Details = new InvoiceItem();
        PickerList selectedItem = new PickerList();
        string Entry_Expiry = string.Empty;
        string BatchQty = "0";
        public SQLiteAsyncConnection database;


        public Detailpage()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Invoiceitems.sqlite");
                database = new SQLiteAsyncConnection(path);
                if (CrossConnectivity.Current.IsConnected)
                {
                    //AppSettings.InvoiceNo = null;
                    var items = GetAPIData.Getinvoicelist(AppSettings.InvoiceNo);
                    AppSettings.Picked_List = items;
                    string StartTime = DateTime.Now.ToString("HH:mm:ss");
                    GetAPIData.UpdateCheckerStartTime(StartTime, AppSettings.InvoiceNo);
                    foreach (InvoiceItem item in items)
                    {
                        item.rowcolor = "Transparent";
                        item.rowPickercolor = "Transparent";
                        item.rowUpdatecolor = "White";
                        item.Isdelete = true;
                        if (item.NewQty != "0")
                        {
                            item.rowcolor = "Green";
                            item.rowUpdatecolor = "Green";
                        }
                        else
                        {
                            item.rowcolor = "White";
                            item.NewQty = "";
                        }
                        if (item.isqty == "y")
                        {
                            item.rowcolor = "Green";
                            item.rowUpdatecolor = "Green";
                        }
                        item.DisPlayQty = item.Qty.ToString();
                        if (item.FQty != 0)
                        {
                            item.DisPlayQty = item.Qty.ToString() + "+" + item.FQty.ToString();
                            item.Isqtyenable = true;
                            item.rowqtyUpdatecolor = "#3374FF";
                        }

                        if (item.Picked == null)
                            item.rowcolor = "Yellow";

                        if (item.isUnpick == "y")
                            item.rowcolor = "LightPink";

                        if (item.isdelete == "y")
                        {
                            item.rowcolor = "Red";
                            item.Isdelete = false;
                            item.Chicked = true;
                        }
                        item.DisplayBatch = item.Batch;
                        if (item.isbatch == "y")
                        {
                            if(!string.IsNullOrEmpty(item.NewBatch))
                                item.DisplayBatch = item.NewBatch;
                            else
                            {
                                string[] batcstr = item.ManualBatch.Split(',');
                                item.DisplayBatch = batcstr[0];
                            }

                            if (!string.IsNullOrEmpty(item.NewBatch) && !string.IsNullOrEmpty(item.ManualBatch))
                            {
                                string[] batcstr = item.ManualBatch.Split(',');
                                item.DisplayBatch = item.NewBatch +","+ batcstr[0];
                            }

                            item.rowcolor = "Orange";
                            item.rowPickercolor = "Orange";
                        }
                        item.CheckerName = AppSettings.UserDetail.checkername;
                        item.CheckerId = AppSettings.UserDetail.checkercode;

                        item.Block = AppSettings.Block;
                        products.Add(item);
                    }
                    var itemlist = products.OrderBy(i => i.Chicked).ToList();
                    items = new ObservableCollection<InvoiceItem>(itemlist);
                    datagrid.ItemsSource = items;
                    AppSettings.CheckerPickedList = items;
                    ToastNotification.TostMessage("Total Lines =" + AppSettings.CheckerPickedList.Count);
                    if (AppSettings.CheckerPickedList.Count != 0)
                    {
                        lblcustcode.Text = Convert.ToString(AppSettings.CheckerPickedList[0].CustCode);
                        lblcheckerid.Text = AppSettings.CheckerPickedList[0].CheckerId;
                        lblinvnum.Text = AppSettings.InvoiceNo;
                        invoicedate.Text = Convert.ToDateTime(AppSettings.CheckerPickedList[0].Invdt).ToString("dd/MM/yyyy");
                        if (AppSettings.CheckerPickedList[0].CustomerName.Length > 30)
                            lblcustname.Text = AppSettings.CheckerPickedList[0].CustomerName.Substring(0, 20) + "..";
                        else
                            lblcustname.Text = AppSettings.CheckerPickedList[0].CustomerName;

                        if (AppSettings.PickedDetail.pickername.Length > 15)
                            lblpickername.Text = AppSettings.PickedDetail.pickername.Substring(0, 15) + "..";
                        else
                            lblpickername.Text = AppSettings.PickedDetail.pickername;


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

        private async void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    chickeditem = (InvoiceItem)((CheckBox)sender).BindingContext;
                    if (chickeditem.Chicked == true)
                    {
                        UpdateQty objdty = new UpdateQty();
                        var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == chickeditem.Itemid && i.Batchid == chickeditem.Batchid && i.Invnum == chickeditem.Invnum && i.Id == chickeditem.Id).FirstOrDefault();
                        chkboxes.Add(chickeditem);
                        AddItem.Chicked = true;
                        AddItem.CheckedTime = DateTime.Now.ToString("HH:mm:ss");
                        objdty.Checked = "y";
                        UpdateandDeleteCheckedStatus(objdty, AddItem);
                    }
                    else
                    {
                        unpick.IsVisible = true;
                    }
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
            }
        }

        private async void CheckYes_Tapped(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    UpdateQty objdty = new UpdateQty();
                    unpick.IsVisible = false;
                    var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == chickeditem.Itemid && i.Batchid == chickeditem.Batchid && i.Invnum == chickeditem.Invnum && i.Id == chickeditem.Id).FirstOrDefault();
                    chkboxes.Remove(chickeditem);
                    AddItem.Chicked = false;
                    objdty.Checked = null;
                    UpdateandDeleteCheckedStatus(objdty, AddItem);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
        }

        private async void CheckNo_Tapped(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    CheckNo.IsEnabled = false;
                    unpick.IsVisible = false;
                    popupLoadingView.IsVisible = true;
                    await Task.Delay(2);
                    var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == chickeditem.Itemid && i.Batchid == chickeditem.Batchid && i.Invnum == chickeditem.Invnum && i.Id == chickeditem.Id).FirstOrDefault();
                    AddItem.Chicked = true;
                    datagrid.ItemsSource = null;
                    datagrid.ItemsSource = AppSettings.CheckerPickedList.OrderBy(i => i.Ischecked == true);
                }
                catch (Exception ex)
                {
                }
                popupLoadingView.IsVisible = false;
                CheckNo.IsEnabled = true;
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
        }

        private async void UpdateandDeleteCheckedStatus(UpdateQty objdty, InvoiceItem AddItem)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    objdty.Invdt = AddItem.Invdt;
                    objdty.custCode = AddItem.CustCode;
                    objdty.itemId = AddItem.Itemid.ToString();
                    objdty.Batchid = AddItem.Batchid.ToString();
                    objdty.Invnum = AddItem.Invnum;
                    if (!string.IsNullOrEmpty(AddItem.NewQty))
                        objdty.NewQuenty = Convert.ToDecimal(AddItem.NewQty);
                    objdty.CheckedTime = DateTime.Now.ToString("HH:mm:ss");
                    objdty.Id = AddItem.Id;
                    GetAPIData.UpdatePickList(objdty);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
        }

        private async void rowdelete_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var CheckedItem = (InvoiceItem)((Image)sender).BindingContext;
                    CheckingDetail objDetails = new CheckingDetail();
                    UpdateQty objdty = new UpdateQty();

                    var Deletepopup = await DisplayAlert("Alert", "Do you want to Delete?", "Yes", "No");
                    if (!Deletepopup)
                    {
                        // Reset the search-options
                    }
                    else
                    {
                        popupLoadingView.IsVisible = true;
                        await Task.Delay(2);
                        var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == CheckedItem.Itemid && i.Batchid == CheckedItem.Batchid && i.Invnum == CheckedItem.Invnum && i.Id == CheckedItem.Id).FirstOrDefault();
                        AddItem.Isdelete = false;
                        AddItem.isdelete = "y";
                        AddItem.rowcolor = "Red";
                        AddItem.Chicked = true;
                        var lst = GetAPIData.GetBatchwiseItem(AppSettings.CustCode, AddItem.Invdt, CheckedItem.Itemid, AddItem.Batch, AddItem.Invnum);
                        foreach (var data in lst)
                        {
                            App.Database.UpdateandInsertDatainAppinvoicechangesApp_picklist(objdty, objDetails, data);
                        }

                        datagrid.ItemsSource = null;
                        if (AppSettings.Basketserchstatus == true)
                        {
                            var listit = AppSettings.CheckerPickedList.Where(k => k.BasketNo == AppSettings.Basket_No).ToList();
                            if (listit.Count != 0)
                                datagrid.ItemsSource = listit;
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else if (AppSettings.serchstatus == true)
                        {
                            if (AppSettings.Serch_items.Count != 0)
                                datagrid.ItemsSource = AppSettings.Serch_items;
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else
                            datagrid.ItemsSource = AppSettings.CheckerPickedList;
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

        private async void Next_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UpdateQty objdty = new UpdateQty();
                    CheckingDetail objDetails = new CheckingDetail();
                    popupLoadingView.IsVisible = true;
                    await Next.ScaleTo(3, 100);
                    await Next.ScaleTo(1, 500, Easing.SpringOut);
                    var item = AppSettings.CheckerPickedList.Where(i => i.Chicked == false).FirstOrDefault();
                    if (item == null)
                    {
                        if (await DisplayAlert("Alert:", "Are You Sure Submit", "Ok", "cancel"))
                        {
                            AppSettings.serchstatus = false;
                            if (AppSettings.Serch_items != null)
                                AppSettings.Serch_items.Clear();
                            AppSettings.Basketserchstatus = false;
                            popupLoadingView.IsVisible = true;
                            await Task.Delay(10);
                            GetAPIData.UpdateCkickingDetail(AppSettings.InvoiceNo);
                            string invnums = GetAPIData.GetInvoiceNuminAppInvoiceChanges(AppSettings.CustCode, AppSettings.InvoiceDate, AppSettings.InvoiceNo);
                            var invnum = AppSettings.CheckerPickedList.Select(x => x.Invnum).Distinct();
                            foreach (var items in invnum)
                            {
                                var inv = invnums.Contains(items);
                                if (inv == false)
                                {
                                    var data = AppSettings.CheckerPickedList.Where(x => x.Invnum == items).FirstOrDefault();
                                    App.Database.UpdateandInsertqtyinAppinvoicechangesApp_picklist(objdty, objDetails, data.Qty.ToString(), Convert.ToInt32(data.Qty), data, data, data.FQty, data.FQty);
                                }
                            }
                            InvoiceStatus status = GetAPIData.GetInvoiceStatus(AppSettings.CustCode, Convert.ToDateTime
                                (AppSettings.InvoiceDate), AppSettings.InvoiceNo);

                            if (status.TotalBlocks == status.checkedlines)
                            {
                                var list = AppSettings.CheckerPickedList;
                                await Navigation.PushAsync(new InvoiceSummary());
                            }
                            else
                            {
                                await Navigation.PushAsync(new InvoiceDeatilpage());
                            }
                            string endTime = DateTime.Now.ToString("HH:mm:ss");
                            GetAPIData.UpdateCheckerEndTime(endTime, AppSettings.InvoiceNo);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alert:", "Please Select All Lines", "Ok");
                        datagrid.ItemsSource = AppSettings.CheckerPickedList.Where(i => i.Chicked == false);
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

        private async void DatagridQty_Completed(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UpdateQty objdty = new UpdateQty();
                    CheckingDetail objDetails = new CheckingDetail();
                    var CheckedItem = (InvoiceItem)((Entry)sender).BindingContext;
                    if (CheckedItem != null)
                    {
                        string qty = ((Entry)sender).Text;
                        string NewQty = CheckedItem.NewQty;
                        int OldQty = (int)CheckedItem.Qty;
                        var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == CheckedItem.Itemid && i.Batchid == CheckedItem.Batchid && i.Invnum == CheckedItem.Invnum && i.Id == CheckedItem.Id).FirstOrDefault();
                        if (string.IsNullOrEmpty(NewQty))
                        {
                            NewQty = "0";
                        }
                        if (OldQty < Convert.ToInt32(NewQty))
                        {
                            await DisplayAlert("Alert", "Please Add Qty Below or equal " + OldQty, "OK");
                            ((Entry)sender).Text = AddItem.dummyQty;
                        }
                        else
                        {
                            AddItem.NewQty = qty;
                            AddItem.isqty = "y";
                            AddItem.Chicked = true;
                            AddItem.rowcolor = "Green";
                            AddItem.rowUpdatecolor = "Green";
                            if (string.IsNullOrEmpty(AddItem.NewQty))
                            {
                                AddItem.rowcolor = "Transparent";
                                AddItem.rowUpdatecolor = "White";
                            }
                            var lst = GetAPIData.GetBatchwiseItem(AppSettings.CustCode, AddItem.Invdt, CheckedItem.Itemid, AddItem.Batch, AddItem.Invnum);
                            double Qty = Convert.ToDouble(AddItem.NewQty);
                            foreach (var data in lst)
                            {
                                if (Qty > 0)
                                {
                                    NewQty = Qty.ToString();
                                    if (Qty <= data.Qty)
                                    {
                                        App.Database.UpdateandInsertqtyinAppinvoicechangesApp_picklist(objdty, objDetails, NewQty, OldQty, AddItem, data, AddItem.FQty, AddItem.NewFQty);
                                    }
                                    else if (Qty > data.Qty)
                                    {
                                        NewQty = data.Qty.ToString();
                                        App.Database.UpdateandInsertqtyinAppinvoicechangesApp_picklist(objdty, objDetails, NewQty, OldQty, AddItem, data, AddItem.FQty, AddItem.NewFQty);
                                    }
                                    Qty = Qty - data.Qty;
                                }
                                else
                                {
                                    App.Database.UpdateandInsertDatainAppinvoicechangesApp_picklist(objdty, objDetails, data);
                                }
                              
                            }
                        }
                        datagrid.ItemsSource = null;
                        if (AppSettings.Basketserchstatus == true)
                        {
                            var listit = AppSettings.CheckerPickedList.Where(k => k.BasketNo == AppSettings.Basket_No).ToList();
                            if (listit.Count != 0)
                                datagrid.ItemsSource = listit;
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else if (AppSettings.serchstatus == true)
                        {
                            if (AppSettings.Serch_items.Count != 0)
                                datagrid.ItemsSource = AppSettings.Serch_items;
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else
                            datagrid.ItemsSource = AppSettings.CheckerPickedList;
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

        private async void Pckbatch_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var CheckedItem = (InvoiceItem)((Picker)sender).BindingContext;
                    string title = ((Picker)sender).Title;
                    ((Picker)sender).Title = "Old Batch: " + title + "   Old MRP:  " + CheckedItem.mrp;
                    var ddd = ((Picker)sender).Items;
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

        private async void Pckbatch_Unfocused(object sender, FocusEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                chickeditem = (InvoiceItem)((Picker)sender).BindingContext;
                selectedItem = (PickerList)((Picker)sender).SelectedItem;
                //Batchqtypopup.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
            }
            popupLoadingView.IsVisible = false;
        }
        private void BtnBatchqtyCancle_Clicked(object sender, EventArgs e)
        {
            //Batchqtypopup.IsVisible = false;
        }

        private async void BtnBatchqty_clicked(object sender, EventArgs e)
        {
            try
            {
                UpdateQty objdty = new UpdateQty();
                UpdateBatchStatus objnewBatch = new UpdateBatchStatus();
                CheckingDetail objDetails = new CheckingDetail();
                PickerList pl = new PickerList();
            }
            catch (Exception ex)
            {
            }
        }

        private async void EntryUnpick_Textchanged(object sender, TextChangedEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    BatchQty = ((Entry)sender).Text;
                    if (!string.IsNullOrEmpty(BatchQty))
                    {
                        if (!BatchQty.Contains(".") && !BatchQty.Contains("-") && !BatchQty.Contains(",") && !BatchQty.StartsWith("0"))
                        {
                            int UnpicQty = Convert.ToInt32(BatchQty);
                            int NormalQty = Convert.ToInt32(chickeditem.Qty);
                            if (UnpicQty <= NormalQty)
                            {

                            }
                            else
                            {
                                await DisplayAlert("Alert", "Can't enter more then : " + NormalQty, "Ok");
                                unpickentry.Text = string.Empty;
                                //batchqtyentry.Text = string.Empty;
                            }
                        }
                        else
                        {
                            ((Entry)sender).Text = null;
                            ToastNotification.TostMessage("Please Enter Valid Number");
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
        }

        private async void BtnUnpicked_clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    if (!string.IsNullOrEmpty(unpickentry.Text))
                    {
                        unpickview.IsVisible = false;
                        popupLoadingView.IsVisible = true;
                        await Task.Delay(3);
                        UnpickedDetail objDetail = new UnpickedDetail();
                        var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == chickeditem.Itemid && i.Batchid == chickeditem.Batchid && i.Invnum == chickeditem.Invnum && i.Id == chickeditem.Id).FirstOrDefault();
                        objDetail.CustCode = AddItem.CustCode;
                        objDetail.CustomerName = AddItem.CustomerName;
                        objDetail.Invdt = AddItem.Invdt;
                        objDetail.Qty = Convert.ToDecimal(unpickentry.Text);
                        objDetail.CustId = Convert.ToInt32(AddItem.CustId);
                        objDetail.Itemname = AddItem.Itemname;
                        objDetail.Pack = AddItem.Pack;
                        objDetail.Location = AddItem.Location;
                        objDetail.Status = AddItem.Status;
                        if (!string.IsNullOrEmpty(AddItem.NewQty))
                            objDetail.NewQty = Convert.ToDecimal(AddItem.NewQty);

                        objDetail.Invnum = AddItem.Invnum;
                        objDetail.Itemid = AddItem.Itemid;
                        objDetail.Batchid = AddItem.Batchid;
                        objDetail.Batch = AddItem.Batch;
                        objDetail.expiry = AddItem.expiry;
                        objDetail.mrp = AddItem.mrp.ToString();
                        if (!string.IsNullOrEmpty(AddItem.itemcode))
                            objDetail.itemcode = AddItem.itemcode;
                        else
                            objDetail.itemcode = "0";
                        objDetail.route = AppSettings.PickedDetail.route;
                        objDetail.routeid = AppSettings.PickedDetail.routeid;
                        objDetail.Block = AppSettings.Block;
                        objDetail.checkername = AppSettings.UserDetail.checkername;
                        objDetail.CheckerId = AppSettings.UserDetail.checkercode;
                        objDetail.Phoneid = AppSettings.PickedDetail.phone.Trim();
                        objDetail.PickerName = AppSettings.PickedDetail.pickername;
                        AddItem.rowcolor = "LightPink";
                        UpdateQty obj = new UpdateQty();
                        obj.Batchid = AddItem.Batchid.ToString();
                        obj.custCode = AddItem.CustCode;
                        obj.Invdt = AddItem.Invdt;
                        obj.Invnum = AddItem.Invnum;
                        obj.itemId = AddItem.Itemid.ToString();
                        GetAPIData.InsertUnpickedChanges(objDetail);
                        GetAPIData.UpdateUnpickChange(obj);
                        datagrid.ItemsSource = null;
                        if (AppSettings.Basketserchstatus == true)
                        {
                            var listit = AppSettings.CheckerPickedList.Where(k => k.BasketNo == AppSettings.Basket_No).ToList();
                            if (listit.Count != 0)
                                datagrid.ItemsSource = listit;
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else if (AppSettings.serchstatus == true)
                        {
                            if (AppSettings.Serch_items.Count != 0)
                                datagrid.ItemsSource = AppSettings.Serch_items;
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else
                            datagrid.ItemsSource = AppSettings.CheckerPickedList;

                        unpickentry.Text = null;
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Please enter qty", "Ok");
                        unpickentry.Focus();
                        await Task.Delay(3);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
            popupLoadingView.IsVisible = false;
        }

        private async void Unpickedrow_Tapped(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    unpickview.IsVisible = true;
                    var item = ((Image)sender).BindingContext;
                    chickeditem = (InvoiceItem)item;
                    unpickentry.Focus();
                    await Task.Delay(3);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ToastNotification.TostMessage("Please Check Your InterNet Connection");
            }
        }

        private void Cancle_Clicked(object sender, EventArgs e)
        {
            unpickview.IsVisible = false;
        }

        private async void Draft_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Hold.ScaleTo(3, 100);
                await Hold.ScaleTo(1, 500, Easing.SpringOut);
                BackButton();
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            BackButton();
            return true;
        }

        public async void BackButton()
        {
            try
            {
                if (await DisplayAlert("Alert", "Are you sure you want to exit this page ?", "Yes", "No"))
                {
                    AppSettings.serchstatus = false;
                    if (AppSettings.Serch_items != null)
                        AppSettings.Serch_items.Clear();
                    AppSettings.Basketserchstatus = false;
                    AppSettings.InsertStatus = true;
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void search_Tapped(object sender, EventArgs e)
        {
            try
            {
                AppSettings.serchstatus = true;
                search.IsVisible = false;
                searchbar.IsVisible = true;
                stkcustdetail.IsVisible = false;
                Basketicon.IsVisible = false;
                searchbar1.Focus();
                await Task.Delay(3);
            }
            catch (Exception ex)
            {
            }
        }

        private void Searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var data = ((SearchBar)sender).Text;
                if (data == "")
                {
                    datagrid.ItemsSource = AppSettings.CheckerPickedList;
                    AppSettings.serchstatus = false;
                    if (AppSettings.Serch_items != null)
                        AppSettings.Serch_items.Clear();
                }
                else
                {
                    var keyword = searchbar1.Text;
                    if (keyword.Length >= 1)
                    {
                        serchitems = AppSettings.CheckerPickedList.Where(C => C.Itemname.Contains(keyword.ToUpper()) || C.Location.Contains(keyword.ToUpper())).ToList();
                        AppSettings.Serch_items = serchitems;
                        datagrid.ItemsSource = serchitems;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void Backbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                AppSettings.serchstatus = false;
                if (AppSettings.Serch_items != null)
                    AppSettings.Serch_items.Clear();
                popupLoadingView.IsVisible = true;
                await Task.Delay(1);
                stkcustdetail.IsVisible = true;
                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                searchbar.IsVisible = false;
                search.IsVisible = true;
                Basketicon.IsVisible = true;
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        private async void Basketicon_Tapped(object sender, EventArgs e)
        {
            try
            {
                etybasket.Text = null;
                AppSettings.Basketserchstatus = true;
                stkcustdetail.IsVisible = false;
                FrBasket.IsVisible = true;
                search.IsVisible = false;
                Basketicon.IsVisible = false;
                etybasket.Focus();
                await Task.Delay(3);
            }
            catch (Exception ex)
            {
            }
        }

        private async void FrBasketBackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                await Task.Delay(1);
                AppSettings.Basketserchstatus = false;
                stkcustdetail.IsVisible = true;
                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                FrBasket.IsVisible = false;
                search.IsVisible = true;
                Basketicon.IsVisible = true;
            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
            }
            popupLoadingView.IsVisible = false;
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            try
            {
                AppSettings.Basket_No = ((Entry)sender).Text;
                if (!string.IsNullOrEmpty(AppSettings.Basket_No))
                {
                    var lstitems = AppSettings.CheckerPickedList.Where(k => k.BasketNo == AppSettings.Basket_No).ToList();
                    datagrid.ItemsSource = lstitems.OrderBy(i => i.Chicked).OrderByDescending(j => j.isUnpick).ToList();
                    ToastNotification.TostMessage("Total Lines" + lstitems.Count);
                }
                else
                {
                    ToastNotification.TostMessage("Please Enter BasketNo");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void qtychange_Tapped(object sender, EventArgs e)
        {
            try
            {
                await qtychange.ScaleTo(3, 100);
                await qtychange.ScaleTo(1, 500, Easing.SpringOut);
                datagrid.ItemsSource = null;
                var qtychangeitems = AppSettings.CheckerPickedList.OrderByDescending(i => i.isqty).ToList();
                datagrid.ItemsSource = qtychangeitems;
            }
            catch (Exception ex)
            {
            }
        }

        private async void ITEMDELETE_Tapped(object sender, EventArgs e)
        {
            try
            {
                await ITEMDELETE.ScaleTo(3, 100);
                await ITEMDELETE.ScaleTo(1, 500, Easing.SpringOut);
                datagrid.ItemsSource = null;
                var isdeleteitems = AppSettings.CheckerPickedList.OrderByDescending(i => i.isdelete).ToList();
                datagrid.ItemsSource = isdeleteitems;
            }
            catch (Exception ex)
            {
            }
        }

        private async void BATCHCHANGE_Tapped(object sender, EventArgs e)
        {
            try
            {
                await BATCHCHANGE.ScaleTo(3, 100);
                await BATCHCHANGE.ScaleTo(1, 500, Easing.SpringOut);
                datagrid.ItemsSource = null;
                var isbatchitems = AppSettings.CheckerPickedList.OrderByDescending(i => i.isbatch).ToList();
                datagrid.ItemsSource = isbatchitems;
            }
            catch (Exception ex)
            {
            }
        }

        private async void NOTPICKED_Tapped(object sender, EventArgs e)
        {
            try
            {
                await NOTPICKED.ScaleTo(3, 100);
                await NOTPICKED.ScaleTo(1, 500, Easing.SpringOut);
                var notpickeditems = AppSettings.CheckerPickedList.OrderBy(i => i.Picked).ToList();
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = notpickeditems;
            }
            catch (Exception ex)
            {
            }
        }

        private void Qty1_TextChanged(object sender, TextChangedEventArgs e)
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
                    ToastNotification.TostMessage("Please Enter Valid Number");
                }
            }
            else
            {
                ((Entry)sender).Text = "";
            }
        }

        private async void Qty_Tapped(object sender, EventArgs e)
        {
            try
            {
                var item = ((StackLayout)sender).BindingContext;
                Details = (InvoiceItem)item;
                freeQty.IsVisible = true;
                lblScheme.Text = Details.Scheme;
                lblqty.Text = Details.Qty.ToString();
                lblfreeqty.Text = Details.FQty.ToString();
                lblnewqty.Text = Details.NewQty;
                if (Details.NewFQty == 0)
                    lblfreenewqty.Text = " ";
                else
                    lblfreenewqty.Text = Details.NewFQty.ToString();

                lblnewqty.Focus();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
            }
        }

        private void FreeqtyCancle_Clicked(object sender, EventArgs e)
        {
            freeQty.IsVisible = false;
        }

        private async void BtnFreeqty_clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UpdateQty objdty = new UpdateQty();
                    CheckingDetail objDetails = new CheckingDetail();
                    freeQty.IsVisible = false;
                    if (!string.IsNullOrEmpty(lblnewqty.Text))
                    {
                        if (!string.IsNullOrEmpty(lblfreenewqty.Text))
                        {
                            string qty = lblnewqty.Text;
                            string NewQty = lblnewqty.Text;
                            int FQty = Convert.ToInt32(lblfreeqty.Text);
                            int NewFQty = Convert.ToInt32(lblfreenewqty.Text);
                            int OldQty = Convert.ToInt32(lblqty.Text);
                            var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == Details.Itemid && i.Batchid == Details.Batchid && i.Invnum == Details.Invnum && i.Id == Details.Id).FirstOrDefault();
                            if (string.IsNullOrEmpty(NewQty))
                            {
                                NewQty = "0";
                            }
                            if (OldQty < Convert.ToInt32(NewQty))
                            {
                                await DisplayAlert("Alert", "Please Add Qty Below or equal " + OldQty, "OK");
                                ((Entry)sender).Text = AddItem.dummyQty;
                            }
                            else
                            {
                                AddItem.NewQty = qty;
                                AddItem.isqty = "y";
                                AddItem.Chicked = true;
                                AddItem.rowcolor = "Green";
                                AddItem.rowUpdatecolor = "Green";
                                if (string.IsNullOrEmpty(AddItem.NewQty))
                                {
                                    AddItem.rowcolor = "Transparent";
                                    AddItem.rowUpdatecolor = "White";
                                }
                                var lst = GetAPIData.GetBatchwiseItem(AppSettings.CustCode, AddItem.Invdt, Details.Itemid, AddItem.Batch, AddItem.Invnum);
                                double Qty = Convert.ToDouble(AddItem.NewQty);
                                foreach (var data in lst)
                                {
                                    if (Qty > 0)
                                    {
                                        NewQty = Qty.ToString();
                                        if (Qty <= data.Qty)
                                        {
                                            App.Database.UpdateandInsertqtyinAppinvoicechangesApp_picklist(objdty, objDetails, NewQty, OldQty, AddItem, data, FQty, NewFQty);
                                        }
                                        else if (Qty > data.Qty)
                                        {
                                            NewQty = data.Qty.ToString();
                                            App.Database.UpdateandInsertqtyinAppinvoicechangesApp_picklist(objdty, objDetails, NewQty, OldQty, AddItem, data, FQty, NewFQty);
                                        }
                                        Qty = Qty - data.Qty;
                                    }
                                    else
                                    {
                                        App.Database.UpdateandInsertDatainAppinvoicechangesApp_picklist(objdty, objDetails, data);
                                    }
                                    GetAPIData.InsertInvoiceChanges(objDetails);
                                }
                            }
                            datagrid.ItemsSource = null;
                            if (AppSettings.Basketserchstatus == true)
                            {
                                var listit = AppSettings.CheckerPickedList.Where(k => k.BasketNo == AppSettings.Basket_No).ToList();
                                if (listit.Count != 0)
                                    datagrid.ItemsSource = listit;
                                else
                                    datagrid.ItemsSource = AppSettings.CheckerPickedList;
                            }
                            else if (AppSettings.serchstatus == true)
                            {
                                if (AppSettings.Serch_items.Count != 0)
                                    datagrid.ItemsSource = AppSettings.Serch_items;
                                else
                                    datagrid.ItemsSource = AppSettings.CheckerPickedList;

                                searchbar1.Text = null;
                                searchbar1.Focus();
                            }
                            else
                                datagrid.ItemsSource = AppSettings.CheckerPickedList;
                        }
                        else
                        {
                            ToastNotification.TostMessage("Please Enter Qty");
                            lblnewqty.Focus();
                            await Task.Delay(1);
                        }
                    }
                    else
                    {
                        ToastNotification.TostMessage("Please Enter FreeQty");
                        lblfreenewqty.Focus();
                        await Task.Delay(1);
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

        private async void Lblnewqty_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string qty = ((Entry)sender).Text;
                if (!string.IsNullOrEmpty(qty))
                {
                    if (!qty.Contains(".") && !qty.Contains("-") && !qty.Contains(","))
                    {
                        if (Details.Qty < Convert.ToInt32(qty))
                        {
                            await DisplayAlert("Alert", "Please Enter below this value : " + Details.Qty, "Ok");
                            ((Entry)sender).Text = "";
                            lblfreenewqty.Text = "";
                        }
                    }
                    else
                    {
                        ((Entry)sender).Text = null;
                        ToastNotification.TostMessage("Please Enter Valid Number");
                    }
                }
                else
                {
                    ((Entry)sender).Text = "";
                    lblfreenewqty.Text = "";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void lblfreenewqty_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string qty = ((Entry)sender).Text;
                if (!string.IsNullOrEmpty(qty))
                {
                    if (!qty.Contains(".") && !qty.Contains("-") && !qty.Contains(","))
                    {
                        if (Details.FQty < Convert.ToInt32(qty))
                        {
                            await DisplayAlert("Alert", "Please Enter below this value : " + Details.FQty, "Ok");
                            ((Entry)sender).Text = "";
                        }
                    }
                    else
                    {
                        ((Entry)sender).Text = null;
                        ToastNotification.TostMessage("Please Enter Valid Number");
                    }
                }
                else
                {
                    ((Entry)sender).Text = "";
                    lblfreenewqty.Text = "";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void pckbatch_Tapped(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(SearchBatch.Text))
                    SearchBatch.Text = string.Empty;
                Batchpoppup.IsVisible = true;
                chickeditem = (InvoiceItem)((Label)sender).BindingContext;
                if (!string.IsNullOrEmpty(chickeditem.NewBatch))
                {
                    string[] batcstr = chickeditem.NewBatch.Split(',');
                    string[] batcstrQTY = chickeditem.BatchQty.Split(',');
                    var BatchandQty = batcstr.Zip(batcstrQTY, (batc, qty) => batc + "," + qty);
                    foreach (var BQ in BatchandQty)
                    {
                        string[] batcandQtystr = BQ.Split(',');
                        var Addlst = chickeditem.BatchList.Where(i => i.batch == batcandQtystr[0]).FirstOrDefault();
                        Addlst.IsChecked = true;
                        Addlst.BatchentryIsVisible = true;
                        Addlst.Batchentrytext = batcandQtystr[1];
                    }
                }
                if (!string.IsNullOrEmpty(chickeditem.ManualBatch))
                {
                    string [] btcqty=chickeditem.ManualBatch.Split(',');
                   var check= chickeditem.BatchList.Where(i => i.batch == btcqty[0]).FirstOrDefault();
                    if (check == null)
                    {
                        PickerList dt = new PickerList();
                        dt.batch = btcqty[0];
                        dt.PickerDisply = "New Batch : " + btcqty[0] + "   MRP: " + chickeditem.mrp;
                        dt.batchid = 0;
                        dt.MRP = Convert.ToDouble(chickeditem.mrp);
                        dt.IsChecked = true;
                        dt.Batchentrytext = btcqty[1];
                        dt.Rate = Convert.ToDouble(chickeditem.Rate);
                        dt.BatchStatus = true;
                        dt.BatchentryIsVisible = true;
                        chickeditem.BatchList.RemoveAll(i => i.WhichBatch != "ManualBatch");
                        chickeditem.BatchList.Add(dt);
                        //AddNewBatch.IsVisible = false;
                        //stknewbatch.IsVisible = false;
                    }
                }
                //var data2 = chickeditem.BatchList.Where(i => i.IsChecked == true).FirstOrDefault();
                //if (data2 == null)
                //    AddNewBatch.IsVisible = true;
                OldBatchDisplay.Text = "Old Batch: " + chickeditem.DisplayBatch + "   Old MRP:  " + chickeditem.mrp;
                Batchlst.ItemsSource = chickeditem.BatchList.OrderByDescending(i=>i.IsChecked);
            }
            catch (Exception ex)
            {
            }
        }

        private async void Batch_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                int BatchQty = 0;
                selectedItem = (PickerList)((CheckBox)sender).BindingContext;
                var Additem = chickeditem.BatchList.Where(i => i.batchid == selectedItem.batchid).FirstOrDefault();
                if (selectedItem.IsChecked == true)
                {
                    var item = chickeditem.BatchList.Where(i => !string.IsNullOrEmpty(i.Batchentrytext));
                    foreach (var data in item)
                    {
                        BatchQty = Convert.ToInt32(BatchQty) + Convert.ToInt32(data.Batchentrytext);
                    }
                    int NormalQty = Convert.ToInt32(chickeditem.Qty + chickeditem.FQty);
                    if (BatchQty != NormalQty)
                    {
                        var itemdata = chickeditem.BatchList.Where(i => i.BatchentryIsVisible == true && string.IsNullOrEmpty(i.Batchentrytext)).FirstOrDefault();
                        if (itemdata != null)
                        {
                            selectedItem.IsChecked = false;
                            Additem.IsChecked = false;
                            ToastNotification.TostMessage("Please Enter Qty");
                        }
                        else
                        {
                            Additem.BatchentryIsVisible = true;
                            await Task.Delay(100);
                            selectedItem.BatchIsFocus = true;
                            Additem.IsChecked = true;
                            //AddNewBatch.IsVisible = false;
                        }

                    }
                    else
                    {
                        selectedItem.IsChecked = false;
                        ToastNotification.TostMessage("Already your total Qty was Added ");
                    }
                }
                else
                {
                    Additem.BatchentryIsVisible = false;
                    selectedItem.BatchIsFocus = false;
                    Additem.IsChecked = false;
                    Additem.Batchentrytext = string.Empty;
                    //AddNewBatch.IsVisible = true;
                }
                //var data2 =chickeditem.BatchList.Where(i => i.IsChecked == true).FirstOrDefault();
                //if(data2!=null)
                //    AddNewBatch.IsVisible = false;

                Additem.BatchStatus = false;
                //stknewbatch.IsVisible = false;
                Batchlst.ItemsSource = null;
                Batchlst.ItemsSource = chickeditem.BatchList.OrderByDescending(i => i.IsChecked);

            }
            catch (Exception ex)
            {
            }
        }

        private void BatchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int BatchQty = 0;
                string qty = ((Entry)sender).Text;
                if (!string.IsNullOrEmpty(qty))
                {
                    if (!qty.Contains(".") && !qty.Contains("-") && !qty.Contains(",") && !qty.StartsWith("0"))
                    {
                        var item = chickeditem.BatchList.Where(i => i.IsChecked == true);
                        //if (stknewbatch.IsVisible == true)
                        //{
                        //    if (string.IsNullOrEmpty(EntryQTY.Text))
                        //    {
                        //        BatchQty = 0;
                        //    }
                        //    else
                        //        BatchQty = Convert.ToInt32(EntryQTY.Text);
                        //}

                        foreach (var data in item)
                        {
                            BatchQty = Convert.ToInt32(BatchQty) + Convert.ToInt32(data.Batchentrytext);
                        }

                        int NormalQty = Convert.ToInt32(chickeditem.Qty + chickeditem.FQty);
                        if (BatchQty <= NormalQty)
                        {

                        }
                        else
                        {
                            ((Entry)sender).Text = null;
                            ToastNotification.TostMessage("Please Enter below " + (NormalQty - (BatchQty - Convert.ToInt32(qty))) + " Qty");
                        }
                    }
                    else
                    {
                        ((Entry)sender).Text = null;
                        ToastNotification.TostMessage("Please Enter Valid Number");
                    }
                }
                else
                {
                    ((Entry)sender).Text = "";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BatchCancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                Batchpoppup.IsVisible = false;
                //AddNewBatch.IsVisible = true;
                //stknewbatch.IsVisible = false;
            }
            catch (Exception ex)
            {
            }
        }

        //private void AddNewBatch_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        AddNewBatch.IsVisible = false;
        //        stknewbatch.IsVisible = true;
        //        EntryBatch.Focus();
        //        EntryBatch.Text = string.Empty;
        //        EntryExpiry.Text = string.Empty;
        //        EntryMRP.Text = string.Empty;
        //        EntryQTY.Text = string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void EntryExpiry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                string exp = ((Entry)sender).Text;
                if (!exp.Contains(".") && !exp.Contains("-") && !exp.Contains(","))
                {
                    if (exp.Length == 2)
                    {
                        if (Entry_Expiry.Length == 3)
                        {
                            ((Entry)sender).Text = exp.Remove(exp.Length - 1, 1);
                        }
                        else
                            ((Entry)sender).Text = exp + "/";
                    }
                    Entry_Expiry = ((Entry)sender).Text;
                }
                else
                {
                    ((Entry)sender).Text = exp.Remove(exp.Length - 1, 1);
                    ToastNotification.TostMessage("Please Enter Valid Number");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task BatchUpdate()
        {
            try
            {
                UpdateQty objdty = new UpdateQty();
                UpdateBatchStatus objnewBatch = new UpdateBatchStatus();
                CheckingDetail objDetails = new CheckingDetail();
                List<PickerList> batclst = new List<PickerList>();
                string Batch = string.Empty;
                string Batchid = string.Empty;
                string BatchQty = string.Empty;
                string MRP = string.Empty;
                string Rate = string.Empty;

                if (chickeditem != null)
                {
                    int newfreeqty = 0;
                    string oldbatch = chickeditem.Batch;
           

                    batclst = chickeditem.BatchList.Where(i => i.IsChecked == true).ToList();
                    foreach (var selectedItem in batclst)
                    {
                        string newbatch = selectedItem.batch;
                        string newbatchid = selectedItem.batchid.ToString();

                        string NewQty = "0";
                        var AddItem = AppSettings.CheckerPickedList.Where(i => i.Itemid == chickeditem.Itemid && i.Batchid == chickeditem.Batchid && i.Invnum == chickeditem.Invnum && i.Id == chickeditem.Id).FirstOrDefault();
                        if (!string.IsNullOrEmpty(selectedItem.Scheme) && selectedItem.Scheme != " ")
                        {
                            var Splititem = selectedItem.Scheme.Split('+');
                            if (Convert.ToDouble(Splititem[0]) > chickeditem.Qty)
                            {
                                AddItem.Isqtyenable = false;
                                AddItem.rowqtyUpdatecolor = "Transparent";
                            }
                            else if (Convert.ToDouble(Splititem[0]) < chickeditem.Qty || Convert.ToDouble(Splititem[0]) == chickeditem.Qty)
                            {
                                AddItem.Isqtyenable = true;
                                AddItem.rowqtyUpdatecolor = "#3374FF";
                                newfreeqty = Convert.ToInt32(Splititem[0]) / Convert.ToInt32(chickeditem.Qty);
                            }
                        }
                        else
                        {
                            AddItem.Isqtyenable = false;
                            AddItem.rowqtyUpdatecolor = "Transparent";
                        }
                        AddItem.isbatch = "y";
                        AddItem.rowcolor = "Orange";
                        AddItem.Chicked = true;
                        if (string.IsNullOrEmpty(Batch))
                        {
                            AddItem.DisplayBatch = newbatch;
                            Batch = newbatch;
                        }
                        else
                        {
                            AddItem.DisplayBatch = Batch + "," + newbatch;
                            Batch = AddItem.DisplayBatch;
                        }

                        if (string.IsNullOrEmpty(BatchQty))
                        {
                            AddItem.BatchQty = selectedItem.Batchentrytext;
                            BatchQty = selectedItem.Batchentrytext;
                        }
                        else
                        {
                            AddItem.BatchQty = BatchQty + "," + selectedItem.Batchentrytext;
                            BatchQty = AddItem.BatchQty;
                        }

                        if (string.IsNullOrEmpty(Batchid))
                        {
                            AddItem.NewBatchid = newbatchid;
                            Batchid = newbatchid;
                        }
                        else
                        {
                            AddItem.NewBatchid = Batchid + "," + newbatchid;
                            Batchid = AddItem.NewBatchid;
                        }

                        AddItem.rowPickercolor = "Orange";
                        var lst = GetAPIData.GetBatchwiseItem(AppSettings.CustCode, AddItem.Invdt, chickeditem.Itemid, AddItem.Batch, AddItem.Invnum);
                        foreach (var data in lst)
                        {
                            //Update App_Picklist Table //
                            objdty.Invdt = data.Invdt;
                            objdty.custCode = data.CustCode;
                            objdty.itemId = data.Itemid.ToString();
                            objdty.Batchid = data.Batchid.ToString();
                            if (!string.IsNullOrEmpty(NewQty))
                                objdty.NewQuenty = Convert.ToDecimal(NewQty);
                            objdty.Invnum = data.Invnum;
                            objdty.CheckedTime = DateTime.Now.ToString("HH:mm:ss");
                            objdty.Id = data.Id;
                            if (selectedItem.BatchStatus != true)
                            {
                                objdty.NewBatch = AddItem.DisplayBatch;
                                objdty.NewBatchid = AddItem.NewBatchid;
                            }
                            else
                            {
                                objdty.ManualBatch = selectedItem.batch + "," + selectedItem.Batchentrytext;
                            }
                            objdty.NewFQty = newfreeqty;
                            objdty.BatchQty = AddItem.BatchQty;


                            // Insert AppInvoiceChanges Table //
                            objDetails.Invdt = data.Invdt;
                            objDetails.Invnum = Convert.ToInt32(data.Invnum);
                            objDetails.CustId = Convert.ToInt32(data.CustId);
                            objDetails.CustCode = data.CustCode;
                            objDetails.CustomerName = data.CustomerName;
                            objDetails.Itemid = data.Itemid;
                            if (!string.IsNullOrEmpty(data.itemcode))
                                objDetails.Itemcode = data.itemcode;
                            else
                                objDetails.Itemcode = "0";

                            objDetails.Itemname = data.Itemname;
                            objDetails.Remarks = "Item Batch Changed old Batch: " + data.Batch + "- New Batch:" + newbatch;
                            objDetails.Qty = data.Qty.ToString();
                            objDetails.NewQty = "0";
                            objDetails.batch = oldbatch;
                            objDetails.NewBatch = newbatch;
                            objDetails.CheckerName = AppSettings.UserDetail.checkername;
                            objDetails.ChickedDate = Convert.ToDateTime(DateTime.Today.Date.ToString("yyyy-MM-dd"));
                            objDetails.Isdelete = false;
                            objDetails.NewPsrlno = Convert.ToInt32(newbatchid);
                            objDetails.InvType = data.InvType;
                            objDetails.Routename = data.route;
                            objDetails.Batchid = data.Batchid;
                            objDetails.Id = data.Id;
                            objDetails.NewFQty = newfreeqty;
                            objDetails.mrp = data.mrp;
                            objDetails.Rate = data.Rate;
                            objDetails.FQty = AddItem.FQty;
                            var lsst = batclst.Where(i => i.batch == oldbatch).FirstOrDefault();
                            if (batclst[0].batch == selectedItem.batch  && lsst == null)
                            {
                                if (batclst.Count == 2 && lsst != null)
                                {
                                    objDetails.NewQty = selectedItem.Batchentrytext;
                                    objDetails.Description = "NEW BATCH";
                                    objDetails.IsStatus = 3;
                                    
                                }
                                else
                                {
                                    objDetails.NewQty = "0";
                                    objDetails.Description = "BATCH CHANGE";
                                    objDetails.IsStatus = 0;
                                }
                            }
                            else
                            {
                                objDetails.NewQty = selectedItem.Batchentrytext;
                                objDetails.Description = "NEW BATCH";
                                objDetails.IsStatus = 3;
                            }
                         
                            if (oldbatch != newbatch)
                            {
                                GetAPIData.InsertInvoiceChanges(objDetails);
                                GetAPIData.UpdateBatchStatus(objdty);
                                
                                if(objDetails.Description == "BATCH CHANGE" && objDetails.Qty != selectedItem.Batchentrytext)
                                {
                                    objDetails.Description = "QTY Change";
                                    objDetails.IsStatus = 2;
                                    objDetails.NewQty = selectedItem.Batchentrytext;
                                    objdty.NewQuenty = batclst.Sum(i => Convert.ToDecimal(i.Batchentrytext));
                                    objDetails.NewBatch = "";
                                    objDetails.NewPsrlno = data.Batchid;
                                    objDetails.Remarks = "Item Updated old Qty: " + data.Qty + "- New Qty: " + objDetails.NewQty;
                                    GetAPIData.InsertInvoiceChanges(objDetails);
                                    GetAPIData.UpdateQtyChange(objdty);
                                }
                            }
                            else
                            {
                                objDetails.Description = "QTY Change";
                                objDetails.IsStatus = 2;
                                objDetails.NewQty = selectedItem.Batchentrytext;
                                objdty.NewQuenty = batclst.Sum(i => Convert.ToDecimal(i.Batchentrytext));
                                objDetails.NewBatch = "";
                                objDetails.NewPsrlno = data.Batchid;
                                objDetails.Remarks = "Item Updated old Qty: " + data.Qty + "- New Qty: " + objDetails.NewQty;
                                GetAPIData.InsertInvoiceChanges(objDetails);
                                GetAPIData.UpdateQtyChange(objdty);
                            }
                        }
                    }
                    await DisplayAlert("Alert", "Batch Updated Successfully ", "Ok");
                    popupLoadingView.IsVisible = true;
                    await Task.Delay(5);
                    datagrid.ItemsSource = null;
                    if (AppSettings.Basketserchstatus == true)
                    {
                        var listit = AppSettings.CheckerPickedList.Where(k => k.BasketNo == AppSettings.Basket_No).ToList();
                        if (listit.Count != 0)
                            datagrid.ItemsSource = listit;
                        else
                            datagrid.ItemsSource = AppSettings.CheckerPickedList;
                    }
                    else if (AppSettings.serchstatus == true)
                    {
                        if (AppSettings.Serch_items.Count != 0)
                            datagrid.ItemsSource = AppSettings.Serch_items;
                        else
                            datagrid.ItemsSource = AppSettings.CheckerPickedList;
                    }
                    else
                        datagrid.ItemsSource = AppSettings.CheckerPickedList;
                }

            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        //private async void BatchAdd_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        popupLoadingView.IsVisible = true;
        //        await Task.Delay(2);
        //        if (string.IsNullOrEmpty(EntryBatch.Text))
        //        {
        //            ToastNotification.TostMessage("Please enter the NewBatch");
        //            EntryBatch.Focus();
        //            await Task.Delay(10);
        //        }
        //        else if (string.IsNullOrEmpty(EntryExpiry.Text))
        //        {
        //            ToastNotification.TostMessage("Please enter the Expiry");
        //            EntryExpiry.Focus();
        //            await Task.Delay(10);
        //        }
        //        else if (string.IsNullOrEmpty(EntryMRP.Text))
        //        {
        //            ToastNotification.TostMessage("Please enter the MRP");
        //            EntryMRP.Focus();
        //            await Task.Delay(10);
        //        }
        //        else if (string.IsNullOrEmpty(EntryQTY.Text))
        //        {
        //            ToastNotification.TostMessage("Please enter the QTY");
        //            EntryQTY.Focus();
        //            await Task.Delay(10);
        //        }
        //        else
        //        {
        //            var check = chickeditem.BatchList.Where(i => i.batch == EntryBatch.Text).FirstOrDefault();
        //            if (check == null)
        //            {
        //                if (!string.IsNullOrEmpty(chickeditem.NewBatch))
        //                {
        //                    string[] batcstr = chickeditem.NewBatch.Split(',');
        //                    string[] batcstrQTY = chickeditem.BatchQty.Split(',');
        //                    var BatchandQty = batcstr.Zip(batcstrQTY, (batc, qty) => batc + "," + qty);
        //                    foreach (var BQ in BatchandQty)
        //                    {
        //                        string[] batcandQtystr = BQ.Split(',');
        //                        var Addlst = chickeditem.BatchList.Where(i => i.batch == batcandQtystr[0]).FirstOrDefault();
        //                        Addlst.IsChecked = true;
        //                        Addlst.BatchentryIsVisible = true;
        //                        Addlst.Batchentrytext = batcandQtystr[1];
        //                    }
        //                }
        //                OldBatchDisplay.Text = "Old Batch: " + chickeditem.DisplayBatch + "   Old MRP:  " + chickeditem.mrp;
        //                var BatchMrpdata = GetAPIData.GetBatchwiseMrpandRate(EntryBatch.Text, EntryExpiry.Text, EntryMRP.Text);
        //                //if (BatchMrpdata.itemid == chickeditem.Itemid)
        //                //{
        //                    PickerList dt = new PickerList();
        //                    dt.batch = EntryBatch.Text;
        //                    dt.PickerDisply = "New Batch : " + EntryBatch.Text + "   MRP: " + EntryMRP.Text;
        //                    dt.batchid = 0;
        //                    dt.MRP = Convert.ToDouble(EntryMRP.Text);
        //                    dt.IsChecked = true;
        //                    dt.Batchentrytext = EntryQTY.Text;
        //                    dt.Rate = BatchMrpdata.Rate;
        //                    dt.BatchStatus = true;
        //                    dt.WhichBatch = "ManualBatch";
        //                    dt.BatchentryIsVisible = true;
        //                    chickeditem.BatchList.RemoveAll(i => i.WhichBatch != "ManualBatch");
        //                    chickeditem.BatchList.Add(dt);
        //                    AddNewBatch.IsVisible = false;
        //                    stknewbatch.IsVisible = false;
        //                //}
        //                Batchlst.ItemsSource = null;
        //                Batchlst.ItemsSource = chickeditem.BatchList.OrderByDescending(i => i.IsChecked);
        //            }
        //            else
        //            {
        //                ToastNotification.TostMessage("This Batch Already added");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    popupLoadingView.IsVisible = false;
        //}

        private async void BatchSubmit_Clicked(object sender, EventArgs e)
        {
            try
            {
               var batclst = chickeditem.BatchList.Where(i => i.IsChecked == true).FirstOrDefault();
                if(batclst!=null)
                {
                    Batchpoppup.IsVisible = false;
                    await BatchUpdate();
                }
                else
                {
                    ToastNotification.TostMessage("Please select the Batch");
                }
              
            }
            catch (Exception ex)
            {
            }
        }

        private void SearchBatch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBatch.Text))
            {
                if (SearchBatch.Text.Length >= 2)
                {
                    Batchlst.ItemsSource = chickeditem.BatchList.Where(i => i.batch.StartsWith(SearchBatch.Text.ToUpper())).ToList();
                }
            }
            else
            {
                Batchlst.ItemsSource = chickeditem.BatchList.OrderBy(i=>i.IsChecked);
            }
        }

        
    }
}