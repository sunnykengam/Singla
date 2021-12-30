using Plugin.Connectivity;
using SinglaApp.Common;
using SinglaApp.View;
using SinglaApp.ViewModel;
using SQLite;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp
{
    public partial class App : Application
    {
        string version = string.Empty;
        public static ItemsDatabase database;
        public SQLiteAsyncConnection localdatabase;
        public static string AppName { get { return "Singla Medicos"; } }
        bool DbStatus = false;
        static CultureInfo provider = CultureInfo.InvariantCulture;
        static string format = "dd-MM-yyyy HH:mm:ss";
        public App()
        {
            InitializeComponent();
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
            localdatabase = new SQLiteAsyncConnection(path);
            try
            {
                bool Timer = true;
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (Timer)
                    {
                        Task.Run(() =>
                        {
                            Timer = false;
                            App.Database.ItemMasterinsertDB();
                        }).ConfigureAwait(true);
                    }
                    return true;
                });
                GetLatestVersionNumber();
                if (string.IsNullOrEmpty(AppSettings.username))
                {
                    MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    MainPage = new NavigationPage(new MasterDetail());
                }
            }
            catch (Exception ex)
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        public async void GetLatestVersionNumber()
        {
            bool isLatestVersion = await DependencyService.Get<ILatest>().IsUsingLatestVersion();
            if (!isLatestVersion)
            {
                AppSettings.versionupdate = true;
            }
        }

        protected override void OnStart()
        {
            try
            {
                ItemsDatabase setting = new ItemsDatabase();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                version = setting.getAppvarsions("App_Version").Result;
                var presentversion = DependencyService.Get<IFileHelper>().Version;
                if (Application.Current.Properties.ContainsKey("App_Version"))
                {
                    var asDbStatus = Application.Current.Properties["App_Version"];
                    version = asDbStatus.ToString();
                }
                if (version != presentversion)
                {
                    AppSettings.publishdate = DateTime.ParseExact("29-08-2019 00:00:00", format, provider);
                    Application.Current.Properties["App_Version"] = presentversion;
                    copydatabase();
                    setting.UpdateAppvarsions("App_Version", presentversion);
                }
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromMinutes(10), () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (AppSettings.LoginSuccess)
                        {
                            Task.Run(() =>
                            {
                                App.Database.ItemMasterinsertDB();

                            }).ConfigureAwait(true);
                        }
                    }
                    return true;
                });
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (!AppSettings.UpdateazureDB)
                        {
                            AppSettings.UpdateazureDB = true;
                            Task.Run(() =>
                            {
                                App.Database.AppOrdersInsertdata();
                            }).ConfigureAwait(true);
                        }
                    }
                    return true;
                });
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                {
                    Task.Run(() =>
                    {
                        App.Database.UpdateOrderIdandStatus();
                    }).ConfigureAwait(true);
                    return true;
                });
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
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
