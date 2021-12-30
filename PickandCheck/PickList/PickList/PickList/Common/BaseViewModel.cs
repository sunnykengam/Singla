using PickList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PickList.Common
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _progressBarPopupVisble { get; set; }
        public bool ProgressBarPopupVisble
        {
            get { return _progressBarPopupVisble; }
            set
            {
                _progressBarPopupVisble = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<CustomerDetail> _customerList { get; set; }
        public ObservableCollection<CustomerDetail> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                OnPropertyChanged();
            }
        }
        private bool _DatagridIsVisible { get; set; }
        public bool datagridIsVisible
        {
            get { return _DatagridIsVisible; }
            set
            {
                _DatagridIsVisible = value;
                OnPropertyChanged();
            }
        }
        private string _unPickedCount { get; set; }
        public string UnPickedCount
        {
            get { return _unPickedCount; }
            set
            {
                _unPickedCount = value;
                OnPropertyChanged();
            }
        }
        private string _loginName { get; set; }
        public string LoginName
        {
            get { return _loginName; }
            set
            {
                _loginName = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
