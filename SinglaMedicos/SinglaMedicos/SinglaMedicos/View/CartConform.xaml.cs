using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CartConform : ContentPage
    {
        double Grandtotal = 0.00;
        int orderId = 1;
        int orderid;
        ObservableCollection<ItemMaster> chkboxes = new ObservableCollection<ItemMaster>();
        ObservableCollection<Orders> ptrlist = new ObservableCollection<Orders>();
        List<Orders> ordProductList = new List<Orders>();
        List<orderitems> pendList = new List<orderitems>();
        public SQLiteAsyncConnection database;
        public CartConform()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                if (AppSettings.PendingOrderStatus == true)
                {
                    ordProductList = AppSettings.PendingOrderList.Where(i => i.IsChecked == true).ToList();
                }
                else
                {
                    ordProductList = Task.Run(async () => await database.Table<Orders>().Where(i => i.customer_id == AppSettings.customerid && i.AzureStatus == "Pending.").ToListAsync()).Result;
                }
                if (ordProductList.Count != 0)
                {
                    var orderidlist = Task.Run(async () => await database.Table<Orders>().OrderByDescending(i => i.srno).FirstOrDefaultAsync()).Result;
                    if (orderidlist != null)
                    {
                        orderid = orderidlist.srno;
                    }
                    foreach (var itemData in ordProductList)
                    {
                        try
                        {
                            Orders orderlist = new Orders();
                            double Total = 0.00;
                            pendList = Task.Run(async () => await database.Table<orderitems>().Where(i => i.customer_id == itemData.customer_id && i.AzureStatus == "Pending.").ToListAsync()).Result;
                            foreach (orderitems item in pendList)
                            {
                                Total = Total + Convert.ToDouble((item.orderqty)) * Convert.ToDouble((item.Rate));
                            }
                            if (orderidlist == null)
                            {
                                orderlist.Temporder_id = "SM-" + DateTime.Now.ToString("yyMMdd") + orderId;
                                orderId++;
                                Guid obj = Guid.NewGuid();
                                orderlist.OrderGuid = obj.ToString();
                            }
                            else
                            {
                                orderid++;
                                orderlist.Temporder_id = "SM-" + DateTime.Now.ToString("yyMMdd") + orderid;
                                Guid obj = Guid.NewGuid();
                                orderlist.OrderGuid = obj.ToString();
                            }
                            Grandtotal = Grandtotal + Total;
                            lbgrandtotal.Text = Grandtotal.ToString("0.##");
                            orderlist.customer_id = itemData.customer_id;
                            orderlist.customername = itemData.customername;
                            orderlist.NoOfItems = itemData.NoOfItems;
                            orderlist.partnercode = itemData.partnercode;
                            orderlist.totalamount = Total.ToString("0.##");
                            ptrlist.Add(orderlist);
                            conformList.ItemsSource = ptrlist;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CartConform--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Btncancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                var popup = await DisplayAlert("Order", "Cancel the Order", "YES", "NO");
                if (popup)
                {
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CartConform--Btncancel_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private async void Btnconfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                var data = ptrlist.Where(i => i.ChallanaChecked == false && i.InvoiceChecked == false).FirstOrDefault();
                if (data == null)
                {
                    btnconfirm.IsEnabled = false;
                    var action = await DisplayAlert("Order", "Do you want to confirm your Order?", "OK", "CANCEL");
                    if (action)
                    {
                        OnOrdersInsert();
                        Navigation.InsertPageBefore(new MasterDetail(), this);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        var popup = await DisplayAlert("Order", "Cancel the Order", "YES", "NO");
                        if (popup)
                        {
                            await Navigation.PopAsync();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Order", "Please check Challena (Or) Invoice ", "YES", "NO");
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CartConform--Btnconform_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            btnconfirm.IsEnabled = true;
        }

        public async void OnOrdersInsert()
        {
            foreach (Orders PTRitem in ptrlist)
            {
                try
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    var qry = "update Orders set Temporder_id='" + PTRitem.Temporder_id + "'," +
                        "ordtaketime='" + DateTime.Now.ToString("HH:mm:ss") + "'," +
                        "Order_date='" + date + "'," +
                        "AzureStatus='Complete.'," +
                        "OrderGuid='" + PTRitem.OrderGuid + "'," +
                        "Amt='" + PTRitem.totalamount + "'," + 
                        "INVTYPE='" + PTRitem.INVTYPE+ "'" +
                        "where customer_id ='" + PTRitem.customer_id + "' and " +
                        "AzureStatus='Pending.'";
                    var productList = Task.Run(async () => await database.Table<orderitems>().Where(i => i.customer_id == PTRitem.customer_id && i.AzureStatus == "Pending.").ToListAsync()).Result;
                    foreach (var Item in productList)
                    {
                        var Qry = "update orderitems set order_id='" + PTRitem.Temporder_id + "',Order_date = '" + date + "',CreatedDate='" + DateTime.Now
                        + "',AzureStatus='Complete.',OrderGuid='" + PTRitem.OrderGuid + "' where customer_id ='" + PTRitem.customer_id + "' and AzureStatus='Pending.'";
                        var DATA2 = Task.Run(async () => await database.QueryAsync<orderitems>(Qry)).Result;
                    }
                    var DATA = Task.Run(async () => await database.QueryAsync<Orders>(qry)).Result;
                }
                catch (Exception ex)
                {
                    Analytics.TrackEvent("CartConform--OnOrdersInsert--Error : " + AppSettings.salesmancode + "--" + ex.Message);
                }
            }
        }
        
        private void CHALLAN_Clicked(object sender, EventArgs e)
        {
            var Item = (Orders)((RadioButton)sender).BindingContext;
            try
            {
                if (Item != null)
                {
                    var Additemdetail = ptrlist.Where(i => i.customer_id == Item.customer_id).FirstOrDefault();
                    if (Item.ChallanaChecked == true)
                    {
                        Additemdetail.ChallanaChecked = true;
                        Additemdetail.InvoiceChecked = false;
                        Additemdetail.INVTYPE = "Y";
                    }
                    else
                    {
                        Additemdetail.ChallanaChecked = false;
                        Additemdetail.INVTYPE = "";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void INVOICE_Clicked(object sender, EventArgs e)
        {
            var Item = (Orders)((RadioButton)sender).BindingContext;
            try
            {
                if (Item != null)
                {
                    var Additemdetail = ptrlist.Where(i => i.customer_id == Item.customer_id).FirstOrDefault();
                    if (Item.InvoiceChecked == true)
                    {
                        Additemdetail.InvoiceChecked = true;
                        Additemdetail.ChallanaChecked = false;
                        Additemdetail.INVTYPE = " ";
                    }
                    else
                    {
                        Additemdetail.InvoiceChecked = false;
                        Additemdetail.INVTYPE = "";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}