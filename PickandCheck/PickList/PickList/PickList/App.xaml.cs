using PickList.Model;
using PickList.View;
using PickList.ViewModel;
using Plugin.Connectivity;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickList
{
    public partial class App : Application
    {
        bool DbStatus = false;
        static CultureInfo provider = CultureInfo.InvariantCulture;
        string version = string.Empty;
        public static ItemsDatabase database;
        public App()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(AppSettings.PhoneCode))
            {
                MainPage = new NavigationPage(new ManagePickingDetail());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }
        static string format = "dd-MM-yyyy HH:mm:ss";
        protected override void OnStart()
        {
            ItemsDatabase setting = new ItemsDatabase();
            try
            {
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
                version = setting.getAppvarsions("App_Version").Result;
            }
            catch (Exception ex)
            {
            }
            try
            {
                var presentversion = DependencyService.Get<IFileHelper>().Version;
                if (Application.Current.Properties.ContainsKey("App_Version"))
                {
                    var asDbStatus = Application.Current.Properties["App_Version"];
                    version = asDbStatus.ToString();
                }
                if (version != presentversion)
                {
                    Application.Current.Properties["App_Version"] = presentversion;
                    copydatabase();
                    setting.UpdateAppvarsions("App_Version", presentversion);
                }
                Device.StartTimer(TimeSpan.FromMinutes(2), () =>
                {
                    Task.Run(() =>
                    {
                        App.Database.InsertandUpdatePickListInServer();

                    }).ConfigureAwait(true);
                    AppSettings.publishdate = DateTime.Today;
                    return true;
                });
                Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                {
                    Task.Run(() =>
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            var Listitems = GetAPIData.GetUnpickinglist(AppSettings.PhoneCode);
                            AppSettings.CheckerUnpickList = Listitems;
                        }

                    }).ConfigureAwait(true);
                    AppSettings.publishdate = DateTime.Today;
                    return true;
                });

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
