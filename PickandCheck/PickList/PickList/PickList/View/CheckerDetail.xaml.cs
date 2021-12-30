using PickList.Model;
using PickList.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickList.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckerDetail : ContentPage
	{
        public SQLiteAsyncConnection database;
        public CheckerDetail()
        {
            try
            {
                InitializeComponent();
                AppSettings.PickingDateDetails.Clear();
                AppSettings.PhoneCode = null;
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
                database = new SQLiteAsyncConnection(path);
                string EndTime = DateTime.Now.ToString("HH:mm:ss");
                GetAPIData.UpdatePickerEndTime(EndTime, AppSettings.Invnum);
                BackgroundImage = "BackgroundImage.png";
                if (!string.IsNullOrEmpty(AppSettings.customerDetail.ChickerId.ToString()))
                {
                    //var detail = GetAPIData.GetCheckerDetail(AppSettings.customerDetail.invdt.ToString("yyyy-MM-dd"));
                    //lblcheckerdetail.ItemsSource = detail;
                    lblbranchname.Text = AppSettings.customerDetail.custname;
                    AppSettings.PhoneCode = AppSettings.customerDetail.phone.Trim();
                }
                else
                {
                    DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }
        public ImageSource Base64StringToImageSource(string source)
        {
            var byteArray = Convert.FromBase64String(source);
            Stream stream = new MemoryStream(byteArray);
            return ImageSource.FromStream(() => stream);
        }
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                lblLoadingText.Text = "Please Wait...";
                string qry = "delete from PickingDetail";
                var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
             
               
                await Task.Delay(2);
                await Navigation.PushAsync(new ManagePickingDetail());
                popupLoadingView.IsVisible = false;
            }
            catch (Exception ex)
            {
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                lblLoadingText.Text = "Please Wait...";
                await Task.Delay(3);
                var Item = ((Button)sender).BindingContext;
                int CheckerId = Convert.ToInt32(Item.GetType().GetProperty("CheckerId").GetValue(Item, null).ToString());
                string CheckerName = Item.GetType().GetProperty("CheckerName").GetValue(Item, null).ToString();
                CheckerAssign data = new CheckerAssign();
                data.Block = AppSettings.customerDetail.Block;
                data.InvDt = AppSettings.customerDetail.invdt.ToString("yyyy-MM-dd");
                data.custCode = AppSettings.customerDetail.custcode;
                data.Invnum = AppSettings.customerDetail.Invnum.ToString();
                data.CheckerId = CheckerId;
                data.CheckerName = CheckerName;
                GetAPIData.App_Picking_CheckerAssign(data);
                string qry = "delete from PickingDetail";
                var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
                stcksubmit.IsVisible = true;
                //lblcheckername.Text = CheckerId + "-" + CheckerName;
                //lblcheckerdetail.IsVisible = false;
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }
    }
}
