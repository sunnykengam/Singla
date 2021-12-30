using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using SinglaMedicos.Model.Reports;

namespace SinglaMedicos.ViewModel
{
    public class OutstandingViewmodel : ObservableRangeCollection<OutstandingInvoicesViewmodel>, INotifyPropertyChanged
    {
        private Outstanding _customers;
        public ObservableRangeCollection<OutstandingInvoicesViewmodel> inv_List = new ObservableRangeCollection<OutstandingInvoicesViewmodel>();
        public OutstandingViewmodel(Outstanding Customers, bool expanded = true)
        {
            try
            {
                this._customers = Customers;
                foreach (OutstandingInvoices item in Customers.inv_List)
                {
                    inv_List.Add(new OutstandingInvoicesViewmodel(item));
                }
            }
            catch (Exception ex)
            {
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
                        this.AddRange(inv_List);
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

        [JsonProperty(PropertyName = "custcode")]
        public string custcode { get {return _customers.custcode; } }

        [JsonProperty(PropertyName = "customername")]
        public string customername { get { return _customers.customername; } }

        [JsonProperty(PropertyName = "custbalanceamount")]
        public decimal custbalanceamount { get { return _customers.custbalanceamount; } }

        [JsonProperty(PropertyName = "salesmancode")]
        public string salesmancode { get { return _customers.salesmancode; } }
    }
}
