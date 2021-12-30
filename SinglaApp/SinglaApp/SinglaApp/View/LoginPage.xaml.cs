using Newtonsoft.Json;
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
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public SQLiteAsyncConnection database;
        public LoginPage ()
		{
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);
                lblloginname.Text = AppSettings.loginname;
                
                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (AppSettings.UpdateLocalDB)
                        {
                            AppSettings.UpdateLocalDB = false;
                            Task.Run(() =>
                            {
                                if (AppSettings.loginname == "Salesman Login")
                                {
                                    App.Database.CustomerMasterDB();
                                    App.Database.ItemMasterinsertDB();
                                }
                                else
                                {
                                    App.Database.ItemMasterinsertDB();
                                }
                            }).ConfigureAwait(true);
                        }
                    }
                    return true;
                });
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    Stmainlayout.Margin = new Thickness(10, 20, 30, 0);
                }
                else
                {
                    Stmainlayout.Margin = new Thickness(35, 100, 35, 0);
                    username.Margin = new Thickness(0, 20, 0, 0);
                    username.WidthRequest = 400;
                    Pwd.WidthRequest = 400;
                    Pwd.Margin = new Thickness(0, 20, 0, 0);
                    login.Margin = new Thickness(75, 35, 58, 0);
                    ForgotPassword.Margin = new Thickness(65, 35, 0, 0);
                    Signup.Margin = new Thickness(178, 35, 40, 0);
                }
                
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("LoginPage--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            try
            {
                base.OnSizeAllocated(width, height);
                if (height >= 764)
                {
                    Stmainlayout.Margin = new Thickness(10, 90, 30, 0);
                    login.Margin = new Thickness(80, 30, 60, 0);
                    stkpassword.Margin = new Thickness(70, 15, 20, 0);
                    Signup.Margin = new Thickness(80, 0, 0, 0);
                }
                else if (height >= 750)
                {
                    Stmainlayout.Margin = new Thickness(10, 90, 30, 0);
                    login.Margin = new Thickness(37, 20, 20, 0);
                    stkpassword.Margin = new Thickness(30, 15, 20, 0);
                    Signup.Margin = new Thickness(70, 0, 0, 0);
                }
                else if (height >= 730)
                {
                    Stmainlayout.Margin = new Thickness(10, 90, 30, 0);
                    login.Margin = new Thickness(37, 20, 20, 0);
                    stkpassword.Margin = new Thickness(30, 15, 20, 0);
                    Signup.Margin = new Thickness(70, 0, 0, 0);
                }
                else if (height >= 640)
                {
                    stkpassword.Margin = new Thickness(12, 15, 20, 0);
                    Signup.Margin = new Thickness(75, 0, 0, 0);
                }
                else if (height >= 610)
                {
                    stkpassword.Margin = new Thickness(20, 15, 20, 0);
                    Signup.Margin = new Thickness(105, 0, 0, 0);
                }
                else if (height >= 560)
                {
                    stkpassword.Margin = new Thickness(10, 15, 0, 0);
                    Signup.Margin = new Thickness(74, 0, 0, 0);
                }
                else if (height >= 450)
                {
                    username.WidthRequest = 265;
                    Pwd.WidthRequest = 265;
                    stkpassword.Margin = new Thickness(8, 15, 20, 0);
                    Signup.Margin = new Thickness(34, 0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("LoginPage--OnSizeAllocated--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async void login_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    popupLoadingView.IsVisible = true;
                    progessbar.IsRunning = true;
                    await Task.Delay(1);
                    if (!string.IsNullOrEmpty(Pwd.Text) && !string.IsNullOrEmpty(username.Text))
                    {
                        string user_name = username.Text.ToUpper();
                        if (AppSettings.loginname == "Salesman Login")
                        {
                            var Userdetails =  App.Database.GetSalesLoginCheck(user_name, Pwd.Text);
                            if (Userdetails.Count == 0)
                            {
                                await DisplayAlert("Alert : ", "Invalid Credentials", "ok");
                            }
                            else
                            {
                                try
                                {
                                    var productdata = Task.Run(async () => await database.InsertAllAsync(Userdetails)).Result;
                                }
                                catch (Exception ex)
                                {
                                }
                                AppSettings.username = username.Text.ToUpper();
                                AppSettings.password = Pwd.Text;
                                AppSettings.SaleasmanListjson = JsonConvert.SerializeObject(Userdetails[0]);
                                AppSettings.salesmanid = Userdetails[0].salesmanid;
                                AppSettings.salesmancode = Userdetails[0].salesmancode;
                                AppSettings.salesmanname = Userdetails[0].salesmanname;
                                AppSettings.partnercode = Userdetails[0].partnercode;
                                AppSettings.LoginSuccess = true;
                                AppSettings.UpdateLocalDB = true;
                                await Navigation.PushAsync(new MasterDetail());
                            }
                        }
                        else
                        {
                            var Userdetail = App.Database.GetCustLoginCheck(user_name, Pwd.Text);
                            if (Userdetail.Count == 0)
                            {
                                await DisplayAlert("Alert : ", "Invalid Credentials", "ok");
                            }
                            else
                            {
                                if (Userdetail[0].customerstatus == "Active")
                                {
                                    try
                                    {
                                        var productdata = Task.Run(async () => await database.InsertAllAsync(Userdetail)).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    AppSettings.username = username.Text.ToUpper();
                                    AppSettings.password = Pwd.Text;
                                    AppSettings.CustomerListjson = JsonConvert.SerializeObject(Userdetail);
                                    AppSettings.Customerjson = JsonConvert.SerializeObject(Userdetail[0]);
                                    AppSettings.customercode = Userdetail[0].customercode;
                                    AppSettings.customername = Userdetail[0].customername;
                                    AppSettings.customerid = Userdetail[0].customerid.ToString();
                                    AppSettings.salesmanid = Userdetail[0].salesmancode;
                                    AppSettings.partnercode = Userdetail[0].partnercode;
                                    if (!string.IsNullOrEmpty(Userdetail[0].lockreason))
                                        await DisplayAlert("Alert : ", Userdetail[0].lockreason, "ok");
                                    AppSettings.LoginSuccess = true;
                                    AppSettings.UpdateLocalDB = true;
                                    await Navigation.PushAsync(new MasterDetail());
                                }
                                else
                                    await DisplayAlert("Alert : ", "Inactive Customer", "ok");
                            }
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
                Analytics.TrackEvent("LoginPage--login_Clicked--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
            progessbar.IsRunning = false;
            popupLoadingView.IsVisible = false;
        }

        private void Username_Completed(object sender, EventArgs e)
        {
            Pwd.Focus();
        }

        private void Pwd_Completed(object sender, EventArgs e)
        {
            login.Focus();
        }

        private void Signup_Clicked(object sender, EventArgs e)
        {
            try
            {
                //progessbar.IsRunning = true;
                //popupLoadingView.IsVisible = true;
                //Navigation.PushAsync(new Registration());
                //progessbar.IsRunning = false;
                //popupLoadingView.IsVisible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());
            return true;
        }

        private void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                //progessbar.IsRunning = true;
                //popupLoadingView.IsVisible = true;
                //Navigation.PushAsync(new ChangePassword());
                //progessbar.IsRunning = false;
                //popupLoadingView.IsVisible = false;
            }
            catch (Exception ex)
            {
            }
        }
    }
}