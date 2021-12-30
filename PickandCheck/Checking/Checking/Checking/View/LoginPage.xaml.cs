using Checking.Models;
using Checking.View;
using Checking.ViewModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Checking.ViewModel.GetAPIData;

namespace Checking
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            lblversion.Text= DependencyService.Get<IFileHelper>().Version;
        }

        private async void login_Clicked(object sender, EventArgs e)
        {
            try
            {
                //App.Database.GetUrl();
                if (CrossConnectivity.Current.IsConnected)
                {
                    popupLoadingView.IsVisible = true;
                    await Task.Delay(1);
                    if (!String.IsNullOrEmpty(username.Text) && !String.IsNullOrEmpty(Pwd.Text))
                    {
                        try
                        {
                            var userdetail = GetAPIData.Getuserdetails(username.Text);
                            if (userdetail == null)
                            {
                                await DisplayAlert("Alert : ", "Invaliad creditionals", "ok");
                                username.Focus();
                            }
                            else
                            {
                                if (username.Text == Pwd.Text)
                                {
                                    AppSettings.UserName = username.Text;
                                    AppSettings.Password = Pwd.Text;
                                    AppSettings.InvoiceDate = "";
                                    await Navigation.PushAsync(new InvoiceDeatilpage());
                                }
                                else
                                {
                                    await DisplayAlert("Alert : ", "Plz Enter Correct Password", "ok");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (!string.IsNullOrEmpty(Pwd.Text) && !string.IsNullOrEmpty(username.Text))
                    {
                        ToastNotification.TostMessage(" Please enter the username and password");
                        username.Focus();
                    }
                    else if (string.IsNullOrEmpty(username.Text))
                    {
                        ToastNotification.TostMessage("Please enter the username");
                        username.Focus();
                    }
                    else if (string.IsNullOrEmpty(Pwd.Text))
                    {
                        ToastNotification.TostMessage("Please enter the password");
                        Pwd.Focus();
                    }
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
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

        private void Username_Completed(object sender, EventArgs e)
        {
            Pwd.Focus();
        }

        private void Pwd_Completed(object sender, EventArgs e)
        {
            login.Focus();
        }

        
    }
}
