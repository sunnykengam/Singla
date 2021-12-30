using PickList.Model;
using PickList.View;
using PickList.ViewModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickList
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                popupLoadingView.IsVisible = false;
                NavigationPage.SetBackButtonTitle(this, "");
                lblversion.Text = DependencyService.Get<IFileHelper>().Version;
            }
            catch (Exception ex)
            {
            }
        }
        private async void submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                await Task.Delay(2);
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (!string.IsNullOrEmpty(entryusername.Text) && !string.IsNullOrEmpty(entrypassword.Text))
                    {
                        AppSettings.PhoneCode = "P" + entryusername.Text;
                        var phoneinfo = GetAPIData.GetPhoneDetail(AppSettings.PhoneCode);
                        if (phoneinfo != null)
                        {
                            if (phoneinfo.LoginStatus != "LoggedIn")
                            {
                                if (entrypassword.Text == entryusername.Text)
                                {
                                    GetAPIData.UpdateLoginStatus("LoggedIn");
                                    await Navigation.PushAsync(new ManagePickingDetail());
                                }
                                else
                                {
                                    await DisplayAlert("Alert : ", "Plz Enter Correct Password", "ok");
                                }
                            }
                            else
                            {
                                AppSettings.PhoneCode = null;
                                await DisplayAlert("Alert : ", "PhoneId Already logged in another device", "ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Alert : ", "Plz Enter Correct UserName", "ok");
                        }
                    }
                    else if (string.IsNullOrEmpty(entryusername.Text))
                    {
                        await DisplayAlert("Alert : ", "Plz Enter UserName", "ok");
                    }
                    else if (string.IsNullOrEmpty(entrypassword.Text))
                    {
                        await DisplayAlert("Alert : ", "Plz Enter Password", "ok");
                    }
                }
                else
                {
                    await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(null, "Are you sure you want to exit this App ?", "Yes", "No"))
                {
                    base.OnBackButtonPressed();

                    var closer = DependencyService.Get<IFileHelper>();
                    if (closer != null)
                        closer.closeApplication();
                }
            });
            return true;
        }

        private async void Entryusername_Completed(object sender, EventArgs e)
        {
            entrypassword.Focus();
            await Task.Delay(1);
        }

        private async void Entrypassword_Completed(object sender, EventArgs e)
        {
            submit.Focus();
            await Task.Delay(1);
        }
    }
}
