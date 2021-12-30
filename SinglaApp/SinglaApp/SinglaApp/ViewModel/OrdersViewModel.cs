using MvvmHelpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using SinglaApp.Model;
using Xamarin.Forms;
using Microsoft.AppCenter.Analytics;
using SinglaApp.Common;

namespace SinglaApp.ViewModel
{
    public class OrdersViewModel : ObservableRangeCollection<ItemViewModel>, INotifyPropertyChanged
    {
        private Orders _order;
        private ObservableRangeCollection<ItemViewModel> itemlist
           = new ObservableRangeCollection<ItemViewModel>();
        public OrdersViewModel(Orders order, bool expanded = true)
        {
            try
            {
                this._order = order;
                foreach (ItemMaster item in order.itemlist)
                {
                    itemlist.Add(new ItemViewModel(item));
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("OrdersViewModel--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }   
        }
        private bool _expanded;
        public bool Expanded
        {

            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    if (_expanded)
                    {
                        this.AddRange(itemlist);
                    }
                    else
                    {
                        this.Clear();
                    }
                }
            }
        }
        public string StateIcon
        {
            get { return Expanded ? "up" : "down"; }
        }

        [SQLite.Column("customername")]
        public string customername { get { return _order.customername; } }

        [SQLite.Column("partnercode")]
        public string partnercode { get { return _order.partnercode; } }

        [SQLite.Column("order_id")]
        public string order_id { get { return _order.order_id; } }

        [SQLite.Column("customer_id ")]
        public string customer_id { get { return _order.customer_id; } }

        [SQLite.Column("order_status ")]
        public string order_status { get { return _order.order_status; } }

        [SQLite.Column("order_date")]
        public string order_date { get { return _order.order_date; } }

        [SQLite.Column("Invoice_date ")]
        public DateTime Invoice_date { get { return _order.Invoice_date; } }

        [SQLite.Column("delivery_date")]
        public DateTime delivery_date { get { return _order.delivery_date; } }

        [SQLite.Column("Amt")]
        public double Amt { get { return _order.Amt; } }

        [SQLite.Column("NetAmount")]
        public double NetAmount { get { return _order.NetAmount; } }

        [SQLite.Column("TaxAmount")]
        public double TaxAmount { get { return _order.TaxAmount; } }

        [SQLite.Column("Discountamount")]
        public double Discountamount { get { return _order.Discountamount; } }

        [SQLite.Column("Sman")]
        public int Sman { get { return _order.Sman; } }

        [SQLite.Column("Area")]
        public string Area { get { return _order.Area; } }

        [SQLite.Column("route")]
        public string route { get { return _order.route; } }

        [SQLite.Column("NoOfItem")]
        public int NoOfItems { get { return _order.NoOfItems; } }

        public string totalamount { get { return _order.totalamount; } }
        public string compname { get { return _order.compname; } }
        public bool localtoazurevisible { get { return _order.localtoazurevisible; } }
        public int srno { get { return _order.srno; } }

        public string ordtaketime { get { return _order.ordtaketime; } }
        public string ordupldtime { get { return _order.ordupldtime; } }
        public string UpdateStatus { get { return _order.UpdateStatus; } }
    }
}
