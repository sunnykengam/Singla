using Checking.Models;
using Checking.View;
using Checking.ViewModel;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Checking
{
    public partial class App : Application
    {
        private static readonly HttpClient _client = new HttpClient();
        string version = string.Empty;
        public static ItemsDatabase database;
        public static string AppName { get { return "Checking"; } }
        public App()
        {
            InitializeComponent();
            App.Database.GetUrl();
            if (string.IsNullOrEmpty(AppSettings.UserName))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new InvoiceDeatilpage());
            }
        }
        static CultureInfo provider = CultureInfo.InvariantCulture;
        static string format = "dd-MM-yyyy HH:mm:ss";
        protected override void OnStart()
        {
            try
            {
                ItemsDatabase setting = new ItemsDatabase();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Invoiceitems.sqlite");
                version = setting.getAppvarsions("App_Version").Result;
                var presentversion = DependencyService.Get<IFileHelper>().Version;
                if (Application.Current.Properties.ContainsKey("App_Version"))
                {
                    var asDbStatus = Application.Current.Properties["App_Version"];
                    version = asDbStatus.ToString();
                }
                if (version != presentversion)
                {
                    AppSettings.publishdate = DateTime.ParseExact("05-05-2019 00:00:00", format, provider);
                    Application.Current.Properties["App_Version"] = presentversion;
                    copydatabase();
                    setting.UpdateAppvarsions("App_Version", presentversion);
                }

            }
            catch (Exception ex)
            {
            }
        }

        


        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        public static ItemsDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ItemsDatabase();
                }
                return database;
            }
        }
        public void copydatabase()
        {
            DependencyService.Get<IFileHelper>().CopyDatabase();
        }

    }
}
