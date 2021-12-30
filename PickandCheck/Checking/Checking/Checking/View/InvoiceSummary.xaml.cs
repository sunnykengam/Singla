using Checking.Models;
using Checking.ViewModel;
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

namespace Checking.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InvoiceSummary : ContentPage
    {
        SummaryDetail summaryDetails = new SummaryDetail();
        public List<object> chkboxes = new List<object>();
        public Xamarin.Forms.BindingBase ItemDisplayBinding { get; set; }
        public ObservableCollection<InvoiceItem> items;
        public SQLiteAsyncConnection database;
        public InvoiceSummary()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Invoiceitems.sqlite");
                database = new SQLiteAsyncConnection(path);
                LoadingView.IsVisible = true;
                summaryDetails = GetAPIData.GetInvoiceSummary(AppSettings.CustCode, AppSettings.InvoiceNo);
                lblcustcode1.Text = AppSettings.CustCode;
                lblcustname1.Text = AppSettings.PickedDetail.custname;
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void Finished_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoadingView.IsVisible = false;
                popupLoadingView.IsVisible = true;
                await Task.Delay(8);
                if (CrossConnectivity.Current.IsConnected)
                {
                    summaryDetails = GetAPIData.GetInvoiceSummary(AppSettings.CustCode, AppSettings.InvoiceNo);
                    string noofBoxes = Numberofboxes.Text;
                    string noofPacks = NumberofPacks.Text;
                    if (!string.IsNullOrEmpty(noofBoxes))
                        summaryDetails.Noofboxes = Convert.ToInt32(noofBoxes);
                    else
                        summaryDetails.Noofboxes = 0;
                    if (!string.IsNullOrEmpty(noofPacks))
                        summaryDetails.Noofpackets = Convert.ToInt32(noofPacks);
                    else
                        summaryDetails.Noofpackets = 0;
                    GetAPIData.InsertInvoiceSummary(summaryDetails);
                    GetAPIData.InsertSaleAutoMode(AppSettings.InvoiceNo);
                    string Result = GetAPIData.GetEdpStatus(AppSettings.CustCode, AppSettings.InvoiceDate, AppSettings.InvoiceNo);
                    if(Result=="y")
                    {
                        stkinvno.IsVisible = true;
                        string InvoiceNos = GetAPIData.GetInvoiceNumbers(AppSettings.CustCode, AppSettings.InvoiceDate, AppSettings.InvoiceNo);
                        lblinvno.Text = InvoiceNos;
                    }
                    else
                    {
                        LoadingView.IsVisible = true;
                        await DisplayAlert("Alert:", "Branch not submitted click Submit again", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                }
                popupLoadingView.IsVisible = false;
            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
            }
        }

        private async void Btnlogout_Clicked(object sender, EventArgs e)
        {
            popupLoadingView.IsVisible = true;
            await Task.Delay(8);
            await Navigation.PushAsync(new InvoiceDeatilpage());
            popupLoadingView.IsVisible = false;
        }
    }
}