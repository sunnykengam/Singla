using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SinglaApp.Common;
using SinglaApp.Model;
using Xamarin.Forms;
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.ViewModel
{
    public class CompleteViewModel
    {
        public SQLiteAsyncConnection database;
        public ObservableRangeCollection<OrdersViewModel> completelist { get; private set; }
            = new ObservableRangeCollection<OrdersViewModel>();
        public ICommand LoadDataCommand { get; private set; }
        public ICommand HeaderClickCommand { get; private set; }

        public  CompleteViewModel()
        {
            try
            {
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                this.LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
                this.HeaderClickCommand = new Command<OrdersViewModel>((item) => ExecuteHeaderClickCommand(item));
                  
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteViewModel--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async Task ExecuteLoadDataCommand()
        {
            try
            {
                completelist.Clear();
                if (AppSettings.TitelName != "Orders History")
                {
                    if (AppSettings.loginname == "Salesman Login")
                    {
                        if (AppSettings.singleCustpendingbill == true)
                        {
                            string quary = "select * from orders where customer_id='" + AppSettings.CustID + "' and AzureStatus='Complete.' and order_date between'" + AppSettings.Fromdate + "' and '" + AppSettings.Todate + "' order by srno desc";
                            var cmplist = database.QueryAsync<Orders>(quary).Result;
                            OrdersCompleatListBinding(cmplist);

                        }
                        else
                        {
                            string quary = "select * from orders where Sman='" + AppSettings.salesmanid + "' and AzureStatus='Complete.' and order_date between'" + AppSettings.Fromdate + "' and '" + AppSettings.Todate + "' order by srno desc";
                            var cmplist = database.QueryAsync<Orders>(quary).Result;
                            OrdersCompleatListBinding(cmplist);
                        }
                    }
                    else
                    {
                        string quary = "select * from orders where customer_id='" + AppSettings.customerid + "' and AzureStatus='Complete.' and order_date between'" + AppSettings.Fromdate + "' and '" + AppSettings.Todate + "' order by srno desc";
                        var cmplist = database.QueryAsync<Orders>(quary).Result;
                        OrdersCompleatListBinding(cmplist);
                    }
                }
                else
                {
                    var Orederdetails = App.Database.OrderHistory();
                    OrdersCompleatListBinding(Orederdetails);
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("ExecuteLoadDataCommand--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void OrdersCompleatListBinding(List<Orders> cmplist)
        {
            foreach (Orders compitem in cmplist.ToList())
            {
                Orders orderdata = new Orders();
                var PTRorders = Task.Run(async () => await database.Table<Partner>().Where(i => i.Code == Convert.ToInt32(compitem.partnercode)).FirstOrDefaultAsync());
                if (PTRorders != null)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        orderdata.compname = compitem.customername.Length <= 34 ? compitem.customername : compitem.customername.Substring(0, 34) + ".."; ;
                    }
                    else
                    {
                        orderdata.compname = compitem.customername;
                    }
                    if (compitem.UpdateStatus == "P")
                    {
                        //Order Processed.=P
                        orderdata.order_status = "#FF8C00";
                    }
                    else if (compitem.UpdateStatus == "G")
                    {
                        //Invoice is Generated.=G
                        orderdata.order_status = "#FF69B4";
                    }
                    else if (compitem.UpdateStatus == "C")
                    {
                        orderdata.order_status = "#FF69B4";
                    }
                    else if (compitem.UpdateStatus == "F")
                    {
                        //Do Not Process.&All Items Not Found.=F
                        orderdata.order_status = "#FF0000";
                    }
                    else
                    {
                        orderdata.order_status = "Transparent";
                    }
                    if (compitem.order_id == null)
                    {
                        orderdata.order_id = "Please Wait...";
                    }
                    else
                    {
                        orderdata.order_id = compitem.order_id;
                        orderdata.ordupldtime = compitem.ordupldtime;
                    }
                    orderdata.UpdateStatus = compitem.UpdateStatus;
                    orderdata.order_date = compitem.order_date;
                    orderdata.NoOfItems = compitem.NoOfItems;
                    orderdata.totalamount = compitem.Amt.ToString("0.##");
                    orderdata.ordtaketime = compitem.ordtaketime;
                    orderdata.srno = compitem.srno;
                    List<orderitems> orderresult = new List<orderitems>();
                    if (AppSettings.TitelName != "Orders History")
                    {
                        orderresult = Task.Run(async () => await database.Table<orderitems>().Where(i => i.OrderGuid == compitem.OrderGuid).ToListAsync()).Result;
                    }
                    else
                    {
                        orderresult = compitem.orderitemlist.ToList();
                    }
                    foreach (var order in orderresult)
                    {
                        ItemMaster orderdatalist = new ItemMaster();
                        orderdatalist.itemname = order.itemname;
                        orderdatalist.packsize = order.pack;
                        orderdatalist.Scheme = order.scm1;
                        orderdatalist.Halfscheme = order.scm2;
                        orderdatalist.qty = order.orderqty;
                        orderdatalist.ptr = order.Rate;
                        orderdatalist.mrp = (float)Convert.ToDouble(order.MRP);
                        orderdata.itemlist.Add(orderdatalist);
                    }
                }
                var orderview = new OrdersViewModel(orderdata);
                completelist.Add(orderview);
                AppSettings.orderlist = completelist;
               
            }
            if (completelist.Count != 0)
                ToastNotification.TostMessage("Total Orders " + completelist.Count);
            else
                ToastNotification.TostMessage("No Orders");
        }
        private void ExecuteHeaderClickCommand(OrdersViewModel item)
        {
            item.Expanded = !item.Expanded;
        }
    }
}
