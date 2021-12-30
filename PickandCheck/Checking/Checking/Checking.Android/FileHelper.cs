using System;
using System.IO;

using Android.App;
using Checking.Droid;
using Xamarin.Forms;
using static Checking.ViewModel.GetAPIData;

[assembly: Dependency(typeof(FileHelper))]
namespace Checking.Droid
{
    class FileHelper : Activity, IFileHelper
    {
        public FileHelper()
        {
        }
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
        public void CopyDatabase()
        {
            try
            {
                var DataBaseStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Checking.Droid.Invoiceitems.sqlite");

                var destinationPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Invoiceitems.sqlite");
                using (Stream source = DataBaseStream)
                {
                    using (var destination = System.IO.File.Create(destinationPath))
                    {
                        source.CopyTo(destination);
                    }
                }
            }
            catch (Exception ex)
            {
                //  Exceptions.SaveOrSendExceptions("Exception in CopyDatabase method In FileHelper.cs", ex);
            }
        }
        public string GetIdentifier()
        {
            //    Android.Telephony.TelephonyManager mTelephonyMgr;
            //    mTelephonyMgr = (Android.Telephony.TelephonyManager)GetSystemService(TelephonyService);
            //    //IMEI number  
            //    String m_deviceId = mTelephonyMgr.DeviceId;
            return null;
        }
        public string Version
        {
            get
            {
                var context = Forms.Context;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            }
        }
        public void closeApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
        Dialog dialog;

    }
}