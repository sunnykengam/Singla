using OTIF.Model;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp.View.Otif
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Delivery : ContentPage
	{
       
        public SQLiteAsyncConnection database;
        public Delivery ()
		{
			InitializeComponent ();
             var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("VAHPLHYD.Sqlite");
        database = new SQLiteAsyncConnection(path);
		}

        private async void Login_Clicked(object sender, EventArgs e)
        {
            popupLoadingView.IsVisible = true;
            progessbar.IsRunning = true;
            await Task.Delay(5);
            if (CrossConnectivity.Current.IsConnected)
            {
                SinglaApp.Model.CustomerMaster customerdetail = new SinglaApp.Model.CustomerMaster();
                Common.AppSettings.Customercode = username.Text.ToUpper();
                if (!string.IsNullOrEmpty(username.Text))
                {
                    try
                    {
                       
                        //customerdetail = Task.Run(async () => await App.ServiceClient.GetTable<SinglaApp.Model.CustomerMaster>().Where(i => i.customercode == AppSettings.Customercode).ToListAsync()).Result.ToList().FirstOrDefault();
                        //if (customerdetail != null)
                        //{
                        //    Common.AppSettings.custdetail = customerdetail;
                        //    await Navigation.PushAsync(new Customerdetails());
                        //}
                    }
                    catch (Exception ex)
                    {
                    }
                  
                }
                else
                {
                    try
                    {
                        await DisplayAlert("Alert : ", " Please enter the user name and password  Credentials", "ok");
                    }
                    finally
                    {
                        popupLoadingView.IsVisible = false;
                        progessbar.IsRunning = false;
                    }
                }
            }
            else
            {
                try
                {
                    await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                }
                finally
                {
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                }
            }
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
        }
    }
}