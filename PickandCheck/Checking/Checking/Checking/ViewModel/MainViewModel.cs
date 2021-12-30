using Checking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Checking.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region fields
        private ObservableCollection<InvoiceItem> teams;
        private InvoiceItem selectedItem;
        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<InvoiceItem> Listitems
        {
            get { return teams; }
            set { teams = value; OnPropertyChanged(nameof(Listitems)); }
        }

        public InvoiceItem SelectedTeam
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                System.Diagnostics.Debug.WriteLine("Team Selected : " + value?.Location);
            }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        public ICommand RefreshCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            Listitems = GetAPIData.Getinvoicelist(AppSettings.InvoiceNo);
            RefreshCommand = new Command(CmdRefresh);
        }

        private async void CmdRefresh()
        {
            IsRefreshing = true;
            // wait 3 secs for demo
            await Task.Delay(3000);
            IsRefreshing = false;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
