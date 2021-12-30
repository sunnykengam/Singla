using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using CheckBox = XLabs.Forms.Controls.CheckBox;
using Newtonsoft.Json;
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakeOrder : ContentPage
    {
        double pagewith = 0;
        double Grandtotal = 0.00;
        string SearchBtnName =null;
        bool serchisvisible = true;
        public SQLiteAsyncConnection database;
        List<ItemMaster> listitems = new List<ItemMaster>();
        List<ItemMaster> Pickeritems = new List<ItemMaster>();
        List<ItemMaster> serchitems = new List<ItemMaster>();
        ObservableCollection<ItemMaster> chkboxes = new ObservableCollection<ItemMaster>();
        ObservableCollection<ItemMaster> productList = new ObservableCollection<ItemMaster>();
        List<Partner> Listitems = new List<Partner>();
        string itemcode = "0";
        int itemid = 0;
        int partnercode = 0;
        string Qty = "1";
        int Increment = 0;
        public TakeOrder()
        {
            InitializeComponent();
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
            database = new SQLiteAsyncConnection(path);
            try
            {
                if (AppSettings.Pendingorderstatus == false)
                    App.Database.GetPendingOrders();
                string CustName = AppSettings.customercode + "-" + AppSettings.customername;
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    lbcustomername.Text = CustName.Length <= 24 ? CustName : CustName.Substring(0, 24) + "..";
                }
                else
                {
                    lbcustomername.Text = CustName;
                }
                if (AppSettings.AllItemdeatial == null)
                {
                    AppSettings.AllItemdeatial = Task.Run(async () => await database.Table<ItemMaster>().ToListAsync()).Result;
                }
                listitems = AppSettings.AllItemdeatial;
                AppSettings.Itemdeatial = AppSettings.AllItemdeatial;
                var count = listitems.Where(i => i.partnercode == AppSettings.partnercode).FirstOrDefault();
                if (count == null)
                {
                    App.Database.ItemMasterinsertDB();
                    listitems = AppSettings.AllItemdeatial;
                }
                if (listitems.Count != 0)
                {
                    AppSettings.Itemdeatial = listitems;
                    if (AppSettings.Partnerlist == null)
                    {
                        var partlist = Task.Run(async () => await database.Table<Partner>().ToListAsync()).Result;
                        AppSettings.PartnerlistJson = JsonConvert.SerializeObject(partlist);
                    }
                    listitems = listitems.Select(i => { i.dummyqty = "0"; return i; }).ToList();
                    listitems = listitems.Select(i => { i.Ischecked = false; return i; }).ToList();
                    listitems = listitems.Select(i => { i.qty = string.Empty; return i; }).ToList();
                    if (AppSettings.CartItems != null)
                    {
                        if (AppSettings.CartItems.Count != 0)
                        {
                            chkboxes = AppSettings.CartItems;
                            foreach (var data in chkboxes)
                            {
                                var item = listitems.Where(i => i.itemid == data.itemid && i.partnercode == data.partnercode).FirstOrDefault();
                                if (item != null)
                                {
                                    item.Ischecked = true;
                                    double qtyprice = (Convert.ToDouble(data.ptr) * Convert.ToDouble(data.qty));
                                    Grandtotal = Grandtotal + qtyprice;
                                    item.qty = data.qty;
                                    item.Increment = data.Increment;
                                    data.dummyqty = data.qty;
                                }
                            }
                        }
                        else
                        {
                            chkboxes.Clear();
                        }
                    }
                    else
                    {
                        chkboxes.Clear();
                    }
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
                }
                else
                    ToastNotification.TostMessage("Please Sync Products..");

                lbgrandtotal.Text = Grandtotal.ToString("0.##");
                lbltotal.Text = "Rs: " + Grandtotal.ToString("0.##");
                labelText.SetBinding(Label.TextProperty, "CartCounter");
                labelText.Text = chkboxes.Count.ToString();
                stckgrandtotal.IsVisible = true;
                lst2.IsVisible = false;
                serchitems = listitems;
                SearchBtnName = "Product";
                searchbar.IsVisible = true;
                stacksearch.IsVisible = false;
                searchbar1.Placeholder = "Enter Item Name";
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                if (!AppSettings.EntryFocusStatus)
                {
                    if (stckbutton.IsVisible == false)
                    {
                        base.OnAppearing();
                        await Task.Delay(1);
                        searchbar1.Focus();
                    }
                }
                else
                {
                    searchbar1.Unfocus();
                    AppSettings.EntryFocusStatus = false;
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--OnAppearing--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
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
                Analytics.TrackEvent("TakeOrder--OnDisappearing--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void BtnCartIcon_Clicked(object sender, EventArgs e)
        {
            try
            {
                await relabox.ScaleTo(3, 100);
                await relabox.ScaleTo(1, 500, Easing.SpringOut);
                GotoCart();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--BtnCartIcon_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        public async void GotoCart()
        {
            try
            {
                if (chkboxes.Count != 0)
                {
                    btngotocart.IsEnabled = false;
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await Task.Delay(10);
                    stacksearch.IsVisible = false;
                    AppSettings.cartpagestatus = true;
                    AppSettings.CartItems = chkboxes;
                    itemlst.IsVisible = false;
                    searchbar.IsVisible = false;
                    stkgotocart.IsVisible = false;
                    stckbutton.IsVisible = true;
                    search.IsEnabled = false;
                    search.TextColor = Color.FromHex("#21CE99");
                    stcksublst.IsVisible = false;
                    lst2.IsVisible = true;
                    List<ItemMaster> ProducList= listitems.Where(k => k.Ischecked == true).OrderBy(i => i.Increment).ToList();
                    var LISTIN = new ObservableCollection<ItemMaster>(ProducList);
                    lst2.ItemsSource = LISTIN;
                }
                else
                    ToastNotification.TostMessage("Please Select The Product");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--GotoCart--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            btngotocart.IsEnabled = true;
        }

        private async void Btngotocart_Clicked(object sender, EventArgs e)
        {
            try
            {
                GotoCart();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Btngotocart_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private void Backbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                BackButtonPressed();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Backbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);

            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                BackButtonPressed();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--OnBackButtonPressed--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            return true;
        }

        private void Btndraft_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (chkboxes.Count != 0)
                    BackButton();
                else
                    ToastNotification.TostMessage("Please Select The Product");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Btndraft_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void BackButtonPressed()
        {
            try
            {
                if (stckbutton.IsVisible == true)
                {
                    AppSettings.cartpagestatus = false;
                    stckbutton.IsVisible = false;
                    stkgotocart.IsVisible = true;
                    search.IsEnabled = true;
                    search.TextColor = Xamarin.Forms.Color.White;
                    lst2.IsVisible = false;
                    searchbar.IsVisible = true;
                    await Task.Delay(1);
                    searchbar1.Focus();
                }
                else
                {
                    AppSettings.UpdatePendingStatus = true;
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--BackButtonPressed--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void BackButton()
        {
            try
            {
                btndraft.IsEnabled = false;
                AppSettings.UpdatePendingStatus = true;
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--BackButton--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            btndraft.IsEnabled = true;
        }

        private void Lstentry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string qty = ((Entry)sender).Text;
                Qty = qty;
                if (!string.IsNullOrEmpty(qty))
                {
                    Qty = qty;
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
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Lstentry_TextChanged--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void SearchByItemName_Clicked(object sender, EventArgs e)
        {
            try
            {
                SearchBtnName = "Product";
                searchbar.IsVisible = true;
                stacksearch.IsVisible = false;
                searchbar1.Placeholder = "Enter Item Name";
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--SearchByItemName_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void SearchBySubstitute_Clicked(object sender, EventArgs e)
        {
            try
            {
                SearchBtnName = "Molecule";
                searchbar.IsVisible = true;
                stacksearch.IsVisible = false;
                searchbar1.Placeholder = "Enter Substitute Name";
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--SearchBySubstitute_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private void searchbarbackbutton_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = false;
                stacksearch.IsVisible = true;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--searchbarbackbutton_Tapped--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void Searchbar1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await Task.Delay(1);
                stcksublst.IsVisible = false;
                if (!string.IsNullOrEmpty(searchbar1.Text))
                {
                    var data = ((SearchBar)sender).Text;
                    historysearch.IsVisible = false;
                    lst2.IsVisible = false;
                    itemlst.IsVisible = true;
                    if (data != null)
                    {
                        if (data == "")
                        {
                            itemlst.IsVisible = false;
                        }
                        else
                        {
                            var keyword = searchbar1.Text;
                            AppSettings.SearchHistory = keyword;
                            string[] dt = keyword.Split(' ');
                            if (SearchBtnName == "Product")
                            {
                                string qry = string.Empty;
                                bool status = System.Text.RegularExpressions.Regex.IsMatch(searchbar1.Text, @"\D+");
                                if (status)
                                {
                                    qry = "Select itemname,itemcode,itemid,partnercode,frmstockcolor,Scheme,packsize,manufacturer,stock,ptr,mrp From Itemmaster where itemnamesearch like'" + keyword.Replace(' ', '%') + "%' order by itemname";
                                }
                                else
                                {
                                    qry = "Select itemname,itemcode,itemid,partnercode,frmstockcolor,Scheme,packsize,manufacturer,stock,ptr,mrp From Itemmaster where itemcode like'" + keyword + "%' order by itemname";
                                }
                                serchitems = Task.Run(async () => await database.QueryAsync<ItemMaster>(qry)).Result;

                            }
                            else
                                serchitems = listitems.Where(C => C.substitute.StartsWith(keyword.ToUpper())).ToList();

                            itemlst.ItemsSource = serchitems.ToList();
                        }
                    }
                }
                else
                {
                    historysearch.IsVisible = true;
                    itemlst.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Searchbar1_TextChanged--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
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
                Analytics.TrackEvent("TakeOrder--search_Tapped--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void searchhistory_Tapped(object sender, EventArgs e)
        {
            try
            {
                searchbar.IsVisible = true;
                stacksearch.IsVisible = false;
                searchbar1.Text = AppSettings.SearchHistory;
                await Task.Run(() => Task.Delay(1));
                searchbar1.Focus();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--searchhistory_Tapped--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void Itemlst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                historysearch.IsVisible = true;
                itemlst.IsVisible = false;
                var item = (ItemMaster) ((ListView)sender).SelectedItem;
                itemid = item.itemid;
                itemcode = item.itemcode;
                partnercode = item.partnercode;
                var items = listitems.Where(B => B.itemcode == itemcode && B.itemid==itemid && B.partnercode == partnercode).FirstOrDefault();
                if (items.Ischecked != true)
                {
                    stcksublst.IsVisible = true;
                    lstentry.Text = string.Empty;
                    lblitemname.Text = items.itemname;
                    if (items.stock > 0)
                    {
                        lblfrmstockcolor.BackgroundColor = Color.Green;
                    }
                    else
                    {
                        lblfrmstockcolor.BackgroundColor = Color.Red;
                    }

                    lblstock1.Text = items.stock1;
                    lblmanufacturer.Text = items.manufacturer;
                    lblpacksize.Text = items.packsize;
                    lblScheme.Text = items.Scheme;
                    lblmrp.Text = items.mrp.ToString();
                    lblboxsize.Text = items.boxsize.ToString();
                    lblptr.Text = items.ptr.ToString();
                    searchbar1.Unfocus();
                    await Task.Delay(4);
                    lstentry.Focus();
                    await Task.Delay(4);
                }
                else
                {
                    ToastNotification.TostMessage("Product already added");
                    if (searchbar.IsVisible == true)
                    {
                        searchbar1.Text = null;
                        searchbar1.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Itemlst_ItemSelected--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void Remove_Clicked(object sender, EventArgs e)
        {
            var button = sender as Xamarin.Forms.ImageButton;
            button.IsEnabled = false;
            try
            {
                var Itemid = ((Xamarin.Forms.ImageButton)sender).CommandParameter;
                var selectitem = chkboxes.Where(i => i.itemid == Convert.ToInt32(Itemid)).FirstOrDefault();
                var listselected = listitems.Where(i => i.itemid == Convert.ToInt32(Itemid)).FirstOrDefault();
                var action = await DisplayAlert("DELETE", "Are you sure? Do you want to remove", "YES", "NO");
                if (action)
                {
                    chkboxes.Remove(selectitem);
                    lst2.ItemsSource = chkboxes;
                    double itemprice = (Convert.ToDouble(selectitem.ptr) * Convert.ToDouble(selectitem.qty));
                    Grandtotal = (Grandtotal - itemprice);
                    lbgrandtotal.Text = Grandtotal.ToString("0.##");
                    lbltotal.Text = "Rs: " + Grandtotal.ToString("0.##");
                    labelText.SetBinding(Label.TextProperty, "CartCounter");
                    labelText.Text = chkboxes.Count.ToString();
                    listselected.Ischecked = false;
                    listselected.qty = string.Empty;
                    listselected.dummyqty = "0";
                    var itemid = Convert.ToInt32(Itemid);
                    var productlist = Task.Run(async () => await database.Table<orderitems>().Where(i => i.itemcode == itemid && i.customer_id == AppSettings.customerid && i.AzureStatus == "Pending.").FirstOrDefaultAsync()).Result;
                    if (productlist != null)
                    {
                        string qry = "delete from orderitems where itemcode='" + Itemid + "' and customer_id=='" + AppSettings.customerid + "'and AzureStatus='Pending.'";
                        var DATA = Task.Run(async () => await database.QueryAsync<orderitems>(qry));
                        var ordqry = "update Orders set NoOfItems='" + AppSettings.CartItems.Count + "' where customer_id ='" + AppSettings.customerid + "' and AzureStatus == 'Pending.'";
                        var DATA2 = Task.Run(async () => await database.QueryAsync<Orders>(ordqry)).Result;
                        if (chkboxes.Count == 0)
                        {
                            string qry1 = "delete from Orders where customer_id='" + AppSettings.customerid + "' and AzureStatus == 'Pending.'";
                            var DATA1 = Task.Run(async () => await database.QueryAsync<Orders>(qry1)).Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            button.IsEnabled = true;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var item = ((Entry)sender).BindingContext;
                string qty = ((Entry)sender).Text;
                if (item != null)
                {
                    if (!qty.Contains(".") && !qty.Contains("-") && !qty.Contains(",") && !qty.StartsWith("0"))
                    {
                        int Itemid = Convert.ToInt32(item.GetType().GetProperty("itemid").GetValue(item, null));
                        int partnercode = Convert.ToInt32(item.GetType().GetProperty("partnercode").GetValue(item, null));
                        var selectitem = chkboxes.Where(i => i.itemid == Itemid && i.partnercode == partnercode).FirstOrDefault();
                        if (selectitem != null)
                        {
                            if (!string.IsNullOrEmpty(qty))
                            {
                                double olditemprice = (Convert.ToDouble(selectitem.ptr) * Convert.ToDouble(selectitem.dummyqty));
                                Grandtotal = (Grandtotal - olditemprice);
                                selectitem.qty = qty;
                                selectitem.dummyqty = qty;
                                double itemprice = (Convert.ToDouble(selectitem.ptr) * Convert.ToDouble(selectitem.dummyqty));
                                Grandtotal = (Grandtotal + itemprice);
                            }
                            else
                            {
                                double olditemprice = (Convert.ToDouble(selectitem.ptr) * Convert.ToDouble(selectitem.dummyqty));
                                Grandtotal = (Grandtotal - olditemprice);
                                selectitem.qty = "1";
                                selectitem.dummyqty = "1";
                                double itemprice = (Convert.ToDouble(selectitem.ptr) * Convert.ToDouble(selectitem.dummyqty));
                                Grandtotal = (Grandtotal + itemprice);
                            }
                            InsertPendingOrders(selectitem);
                            lbgrandtotal.Text = Grandtotal.ToString("0.##");
                            lbltotal.Text = "Rs: " + Grandtotal.ToString("0.##");
                        }
                    }
                    else
                    {
                        ((Entry)sender).Text = null;
                        ToastNotification.TostMessage("Plese Enter Valid Number");
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Entry_TextChanged--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private async void Btncancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                var popup = await DisplayAlert("Order", "Cancel the Order", "YES", "NO");
                if (popup)
                {
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Btncancel_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void Btnconfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnconfirm.IsEnabled = false;
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(1);
                if (chkboxes.Count != 0)
                {
                    AppSettings.PendingOrderStatus = false;
                    await Navigation.PushAsync(new CartConform());
                }
                else
                {
                    await DisplayAlert("Alert", "Please select the products", "OK");
                    {
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--Btnconfirm_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
            btnconfirm.IsEnabled = true;
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }

        private async void AddTocart_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (chkboxes.Count <= 899)
                {
                    var selecteditem = listitems.Where(i => i.itemcode == itemcode && i.itemid == itemid && i.partnercode == partnercode).FirstOrDefault();
                    var chkitem = chkboxes.Where(i => i.itemcode == itemcode && i.itemid == itemid && i.partnercode == partnercode).FirstOrDefault();

                    if (!string.IsNullOrEmpty(Qty))
                    {
                        double olditemprice = (Convert.ToDouble(selecteditem.ptr) * Convert.ToDouble(selecteditem.dummyqty));
                        Grandtotal = (Grandtotal - olditemprice);
                        selecteditem.qty = Qty;
                        selecteditem.dummyqty = Qty;
                        double itemprice = (Convert.ToDouble(selecteditem.ptr) * Convert.ToDouble(selecteditem.dummyqty));
                        Grandtotal = (Grandtotal + itemprice);
                    }
                    else
                    {
                        double olditemprice = (Convert.ToDouble(selecteditem.ptr) * Convert.ToDouble(selecteditem.dummyqty));
                        Grandtotal = (Grandtotal - olditemprice);
                        selecteditem.qty = "1";
                        selecteditem.dummyqty = "1";
                        double itemprice = (Convert.ToDouble(selecteditem.ptr) * Convert.ToDouble(selecteditem.dummyqty));
                        Grandtotal = (Grandtotal + itemprice);
                    }
                    if (chkitem == null)
                    {
                        Increment++;
                        selecteditem.Increment = Increment;
                        chkboxes.Add(selecteditem);
                        labelText.SetBinding(Label.TextProperty, "CartCounter");
                        labelText.Text = chkboxes.Count.ToString();
                        AppSettings.CartItems = chkboxes;
                        InsertPendingOrders(selecteditem);
                    }
                    stckgrandtotal.IsVisible = true;
                    selecteditem.Ischecked = true;
                    stcksublst.IsVisible = false;
                    lbgrandtotal.Text = Grandtotal.ToString("0.##");
                    lbltotal.Text = "Rs: " + Grandtotal.ToString("0.##");
                    if (searchbar.IsVisible == true)
                    {
                        searchbar1.Text = null;
                        searchbar1.Focus();
                    }
                }
                else
                {
                    await DisplayAlert("Alert : ", "Cannot take more than 900 items", "ok");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--AddTocart_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private void ItemEntry_Completed(object sender, EventArgs e)
        {
            try
            {
                string qty = ((Entry)sender).Text;
                var selecteditem = listitems.Where(i => i.itemcode == itemcode && i.itemid == itemid && i.partnercode == partnercode).FirstOrDefault();
                var chkitem = chkboxes.Where(i => i.itemcode == itemcode && i.itemid == itemid && i.partnercode == partnercode).FirstOrDefault();
                if (string.IsNullOrEmpty(qty))
                {
                    selecteditem.qty = "1";
                    selecteditem.dummyqty = "1";
                }
                else
                {
                    selecteditem.qty = qty;
                    selecteditem.dummyqty = qty;
                }
                stcksublst.IsVisible = false;
                if (selecteditem.Ischecked == false)
                {
                    stckgrandtotal.IsVisible = true;
                    selecteditem.Ischecked = true;
                    Increment++;
                    selecteditem.Increment = Increment;
                    chkboxes.Add(selecteditem);
                    InsertPendingOrders(selecteditem);
                    double qtyprice = (Convert.ToDouble(selecteditem.ptr) * Convert.ToDouble(selecteditem.qty));
                    Grandtotal = Grandtotal + qtyprice;
                    lbgrandtotal.Text = Grandtotal.ToString("0.##");
                    lbltotal.Text = "Rs: " + Grandtotal.ToString("0.##");
                    labelText.SetBinding(Label.TextProperty, "CartCounter");
                    labelText.Text = chkboxes.Count.ToString();
                    AppSettings.CartItems = chkboxes;
                }
                if (searchbar.IsVisible == true)
                {
                    searchbar1.Text = null;
                    searchbar1.Focus();
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--ItemEntry_Completed--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private void MinusButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string qty = lstentry.Text;
                if (qty != "")
                {
                    int QTY = Convert.ToInt32(qty);
                    if (QTY >= 2)
                    {
                        QTY--;
                        lstentry.Text = QTY.ToString();
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--MinusButton_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        private void PlusButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string qty = lstentry.Text;
                if (qty != "")
                {
                    int QTY = Convert.ToInt32(qty);
                    QTY++;
                    lstentry.Text = QTY.ToString();
                }
                else
                {
                    string QTY = qty + 1;
                    lstentry.Text = QTY;
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--PlusButton_Clicked--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }

        public async void InsertPendingOrders(ItemMaster selecteditem)
        {
            try
            {
                var pendlist = Task.Run(async () => await database.Table<Orders>().Where(i => i.customer_id == AppSettings.customerid && i.AzureStatus == "Pending.").FirstOrDefaultAsync()).Result;
                Orders ordDeta = null;
                var cust_list = Task.Run(async () => await database.Table<CustomerMaster>().FirstOrDefaultAsync()).Result;
                if (cust_list == null)
                {
                    App.Database.CustomerMasterDB();
                }
                var slclist = Task.Run(async () => await database.Table<CustomerMaster>().Where(i => i.customerid ==  AppSettings.customerid && i.partnercode == AppSettings.partnercode).FirstOrDefaultAsync()).Result;
                string custid = slclist.customerid.ToString();
                if (pendlist == null)
                {
                    try
                    {
                        ordDeta = new Orders();
                        ordDeta.customername = slclist.customername;
                        ordDeta.customer_id = custid;
                        ordDeta.partnercode = selecteditem.partnercode.ToString();
                        ordDeta.NoOfItems = AppSettings.CartItems.Count;
                        DateTime date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        ordDeta.order_date = DateTime.Now.ToString("yyyy-MM-dd");
                        ordDeta.Amt = Convert.ToDouble(selecteditem.totalamount);
                        ordDeta.Sman = AppSettings.salesmanid;
                        ordDeta.Area = Convert.ToString(slclist.salesmanarea);
                        ordDeta.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"));
                        ordDeta.AzureStatus = "Pending.";
                        orderitems orditemDeta = new orderitems();
                        orditemDeta.AzureStatus = "Pending.";
                        orditemDeta.Sman = slclist.salesmancode;
                        orditemDeta.customer_id = custid;
                        orditemDeta.order_date = DateTime.Now.ToString("yyyy-MM-dd");
                        orditemDeta.itemcode = selecteditem.itemid;
                        orditemDeta.itemaltercode = selecteditem.itemcode;
                        orditemDeta.itemname = selecteditem.itemname;
                        orditemDeta.pack = selecteditem.packsize;
                        orditemDeta.company = selecteditem.manufacturer;
                        if (!string.IsNullOrEmpty(selecteditem.qty))
                            orditemDeta.orderqty = selecteditem.qty;
                        else
                            orditemDeta.orderqty = "1";

                        orditemDeta.Rate = selecteditem.ptr;
                        orditemDeta.MRP = Convert.ToString(selecteditem.mrp);

                        if (!string.IsNullOrEmpty(selecteditem.Scheme))
                            orditemDeta.scm1 = selecteditem.Scheme;
                        else
                            orditemDeta.scm1 = "N";

                        orditemDeta.scm2 = selecteditem.Halfscheme;
                        float OrderAmount = selecteditem.ptr * Convert.ToInt32(selecteditem.qty);
                        orditemDeta.OrderAmount = OrderAmount;
                        orditemDeta.partnercode = selecteditem.partnercode;
                        orditemDeta.CreatedDate = DateTime.Now;
                        var orderitemresult = Task.Run(async () => await database.InsertAsync(orditemDeta)).Result;
                        var orderresult = Task.Run(async () => await database.InsertAsync(ordDeta)).Result;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    ordDeta = new Orders();
                    ordDeta.NoOfItems = AppSettings.CartItems.Count;
                    var qry = "update Orders set NoOfItems='" + ordDeta.NoOfItems + "' where customer_id ='" + custid + "' and AzureStatus == 'Pending.'";
                    var DATA = Task.Run(async () => await database.QueryAsync<Orders>(qry)).Result;
                    var productlist = Task.Run(async () => await database.Table<orderitems>().Where(i => i.itemcode == selecteditem.itemid && i.customer_id == custid && i.AzureStatus == "Pending.").FirstOrDefaultAsync()).Result;
                    try
                    {
                        if (productlist == null)
                        {
                            orderitems orditemDeta = new orderitems();
                            orditemDeta.AzureStatus = "Pending.";
                            orditemDeta.Sman = slclist.salesmancode;
                            orditemDeta.customer_id = custid;
                            orditemDeta.order_date = DateTime.Now.ToString("yyyy-MM-dd");
                            orditemDeta.itemcode = selecteditem.itemid;
                            orditemDeta.itemaltercode = selecteditem.itemcode;
                            orditemDeta.itemname = selecteditem.itemname;
                            orditemDeta.pack = selecteditem.packsize;
                            orditemDeta.company = selecteditem.manufacturer;
                            if (!string.IsNullOrEmpty(selecteditem.qty))
                                orditemDeta.orderqty = selecteditem.qty;
                            else
                                orditemDeta.orderqty = "1";

                            orditemDeta.Rate = selecteditem.ptr;
                            orditemDeta.MRP = Convert.ToString(selecteditem.mrp);

                            if (!string.IsNullOrEmpty(selecteditem.Scheme))
                                orditemDeta.scm1 = selecteditem.Scheme;
                            else
                                orditemDeta.scm1 = "N";

                            orditemDeta.scm2 = selecteditem.Halfscheme;
                            float OrderAmount = selecteditem.ptr * Convert.ToInt32(selecteditem.qty);
                            orditemDeta.OrderAmount = OrderAmount;
                            orditemDeta.partnercode = selecteditem.partnercode;
                            orditemDeta.CreatedDate = DateTime.Now;
                            var orderitemresult = Task.Run(async () => await database.InsertAsync(orditemDeta)).Result;
                        }
                        else
                        {
                            if (productlist.orderqty != selecteditem.qty)
                            {
                                var query = "update orderitems set orderqty='" + selecteditem.qty + "' where itemcode='" + selecteditem.itemid + "'and customer_id ='" + custid + "'";
                                var result = Task.Run(async () => await database.QueryAsync<orderitems>(query)).Result;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("TakeOrder--InsertPendingOrders--Error : " + AppSettings.salesmancode + "--" + AppSettings.customercode + "--" + ex.Message);
            }
        }
    }
}