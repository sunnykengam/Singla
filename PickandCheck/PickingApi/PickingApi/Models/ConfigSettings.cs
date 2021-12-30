using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PickingApi
{
    public static class Config
    {
        private static LogWriter writer;
        private static string logCategory;
        static Config()
        {
            writer = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            //logCategory = "";
        }
        public struct Category
        {
            public const string pickingApi = "PickingAPI";
            public const string pickingApiError = "PickingAPI Error";
        }
        public static void SetLogCategory(string categoryName)
        {
            logCategory = categoryName;
        }
        public static void Log(string message)
        {
            writer.Write(message, Category.pickingApi, Priority.High);
            Console.WriteLine(message);
           
        }
        public struct Priority
        {
            public const int Lowest = 0;
            public const int Low = 1;
            public const int Normal = 2;
            public const int High = 3;
            public const int Highest = 4;
        }
        public static string MailToAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["MailToAddress"].ToString();
            }
        }
        public static void dispose()
        {
            writer.Dispose();

        }
        private static void ValidationErrorMessages(List<string> errorMessages, MailMessage mailMessage)
        {
            if (errorMessages != null)
            {
                mailMessage.Body += "Import process failed because of following validation errors, Please fix those errors and resubmit" + "<br /><br/>";
                foreach (var productmessage in errorMessages)
                {
                    mailMessage.Body += "Validation error: " + productmessage + "<br />";
                }
            }
        }
        public static string CurrentExecutablePath
        {
            get
            {
                string szCurrentPath = string.Empty;
                int binIndex = Environment.CurrentDirectory.LastIndexOf("bin");
                if (binIndex <= 0)
                    szCurrentPath = Environment.CurrentDirectory + "\\";
                else
                    szCurrentPath = Environment.CurrentDirectory.Remove(binIndex);
                return szCurrentPath;
            }
        }
        //public static void ConfigFileUpdate(string key, string value)
        //{
        //    Log("Updating Config Setting - " + key + ":" + value);
        //    string appPath = CurrentExecutablePath;
        //    string configFile = System.IO.Path.Combine(appPath, "App.config");
        //    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
        //    configFileMap.ExeConfigFilename = configFile;
        //    Log("Opening Config file - " + configFile);
        //    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        //    config.AppSettings.Settings[key].Value = value;
        //    config.Save(ConfigurationSaveMode.Full);
        //    ConfigurationManager.RefreshSection("appSettings");
        //}
        //public static void ServerConfigFileUpdate(string key, string value)
        //{
        //    Log("Updating Config Setting - " + key + ":" + value);
        //    string appPath = CurrentExecutablePath;
        //    string configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\", "SyncAppDeliverDB.exe.config");
        //    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
        //    configFileMap.ExeConfigFilename = configFile;
        //    Log("Opening Config file - " + configFile);
        //    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        //    config.AppSettings.Settings[key].Value = value;
        //    config.Save(ConfigurationSaveMode.Full);
        //    ConfigurationManager.RefreshSection("appSettings");
        //}
        //public static void ConnStringUpdate(string connectionString)
        //{
        //    string appPath = Config.CurrentExecutablePath; ;
        //    string configFile = System.IO.Path.Combine(appPath, "App.config");
        //    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
        //    configFileMap.ExeConfigFilename = configFile;
        //    Log("Opening Config file - " + configFile);
        //    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        //    config.ConnectionStrings.ConnectionStrings["KeimedMasterDB"].ConnectionString = connectionString;
        //    config.Save(ConfigurationSaveMode.Modified,true);
        //    ConfigurationManager.RefreshSection("connectionStrings");
        //}
        //public static void ServerConnStringUpdate(string connectionString)
        //{
        //    string appPath = CurrentExecutablePath;
        //    string configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "SyncAppDeliverDB.exe.config");
        //    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
        //    configFileMap.ExeConfigFilename = configFile;
        //    Log("Opening Config file - " + configFile);
        //    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        //    config.ConnectionStrings.ConnectionStrings["KeimedMasterDB"].ConnectionString = connectionString;
        //    config.Save(ConfigurationSaveMode.Modified, true);
        //    ConfigurationManager.RefreshSection("connectionStrings");
        //}
        //public static void ConnectionStringUpdate(out string connectionString, out string partnerCode,out string password,bool serverpath)
        //{
        //    connectionString = string.Empty;
        //    partnerCode= string.Empty;
        //    password = string.Empty;
        //    string configFile = string.Empty;
        //    if (serverpath==true)
        //    {
        //        configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\", "ConfigHelper\\ConnectionStrings.txt");
        //    }
        //    else
        //    {
        //        string appPath = Config.CurrentExecutablePath;
        //        configFile = appPath + "ConfigHelper\\ConnectionStrings.txt";
        //    }
        //    using (var streamReader = File.OpenText(configFile))
        //    {
        //        try
        //        {
        //            var lines = streamReader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //            connectionString = lines[0].ToString().Split(':')[1].Trim();
        //            partnerCode = lines[1].ToString().Split(':')[1].Trim();
        //            password= lines[2].ToString().Split(':')[1].Trim();
        //            //ConnStringUpdate(connectionString);
        //            ServerConnStringUpdate(connectionString);
        //        }
        //        catch (Exception ex)
        //        {
        //            Exceptions.SetLogCategory(Config.Category.CustomerErrorImport);
        //            Exceptions.HandleContentException(ex, "Unable to read data from connection string text file");
        //        }
        //    }
        //}
    }
}
