    using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SinglaApp.Model.Company;
using Microsoft.AppCenter.Analytics;
using SinglaApp.Common;

namespace SinglaApp.ViewModel.Company
{
    public class MGroupViewModel :ObservableRangeCollection<ProductViewModel>, INotifyPropertyChanged
    {
        private MGroupName _mgname;
        private ObservableRangeCollection<ProductViewModel> Item_List
           = new ObservableRangeCollection<ProductViewModel>();
        public MGroupViewModel(MGroupName mgname, bool expanded = true)
        {
            try
            {
                this._mgname = mgname;
                foreach (ItemName item in mgname.Product_List)
                {
                    Item_List.Add(new ProductViewModel(item));
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("MGroupViewModel--Error : " + AppSettings.salesmancode + "--" + ex.Message);
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
                        this.AddRange(Item_List);
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
        public string manufacturer { get { return _mgname.manufacturer; } }
        public List<ItemName> Product_List { get { return _mgname.Product_List; } }
    }
}
