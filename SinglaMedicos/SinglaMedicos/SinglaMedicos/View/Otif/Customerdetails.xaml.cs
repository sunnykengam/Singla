using OTIF.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaMedicos.Common;
using SinglaMedicos.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaMedicos.View.Otif
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Customerdetails : ContentPage
	{

    
        public SQLiteAsyncConnection database;
        double pagewith = 0;
        List<DeliveryItems> listitems = new List<DeliveryItems>();
        public Customerdetails()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetBackButtonTitle(this, "");
                SinglaMedicos.Model.CustomerMaster customerdetail = new SinglaMedicos.Model.CustomerMaster();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("VAHPLHYD.Sqlite");
                database = new SQLiteAsyncConnection(path);
                string showcode = AppSettings.Customercode;
                try
                {
                    customerdetail = AppSettings.custdetail;
                }
                catch (Exception ex)
                {
                }
                customecode.Text = customerdetail.customercode.ToString();
                customername.Text = customerdetail.customername;
                if (!string.IsNullOrEmpty(customerdetail.customeraddress))
                {
                    addres.IsVisible = true;
                   
                }
                address.Text = customerdetail.customeraddress;
                if (!string.IsNullOrEmpty(customerdetail.telephone))
                {
                    SLPhone.IsVisible = true;
                    telephonenumber.Text = customerdetail.telephone;
                }
                if (!string.IsNullOrEmpty(customerdetail.mobile))
                {
                    SLmobilenumber.IsVisible = true;
                    Mobilenumber.Text = customerdetail.mobile;
                }
                DruglicenceNo.Text = customerdetail.dlnumber;
            }
            catch (Exception ex)
            {
            }
        }

        private async void cancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayAlert("Exit?", "Are you sure? Do you want to Logout", "Yes", "No");
                if (action)
                {
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                    await Navigation.PushAsync(new MasterDetail());
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected override bool OnBackButtonPressed()
        {
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            try
            {
                var action = await DisplayAlert("Exit?", "Are you sure? Do you want to Logout", "Yes", "No");
                if (action)
                {
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                    await Navigation.PushAsync(new MasterDetail());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void next_Clicked(object sender, EventArgs e)
        {
            popupLoadingView.IsVisible = true;
            progessbar.IsRunning = true;
            await Task.Delay(8);
            try
            {
                try
                {
                    await Navigation.PushAsync(new InvoiceDetails(), true);
                }
                finally
                {
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
            }
        }
    }
}