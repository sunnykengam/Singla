using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace InvoiceManagerAPI
{
    public static class Exceptions
    {
        private static LogWriter writer;
        private static string errorLogCategory;
        static Exceptions()
        {
            //writer = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            errorLogCategory = Config.Category.InvoiceManagerApiError;
        }
        public static void SetLogCategory(string categoryName)
        {
            errorLogCategory = categoryName;
        }
        public static void Log(string message)
        {
            //writer.Write(message, Config.Category.InvoiceManagerApiError, Config.Priority.High);
            Console.WriteLine(message);
        }
        public static void HandleException(Exception ex)
        {
            //writer.Write(ex.Message, Config.Category.InvoiceManagerApiError, Config.Priority.High);
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
            {
                //writer.Write(ex.InnerException.Message, Config.Category.InvoiceManagerApiError, Config.Priority.High);
                Console.WriteLine(ex.InnerException.Message);
            }
            //writer.Write(ex.StackTrace, Config.Category.InvoiceManagerApiError, Config.Priority.Highest);
        }

        public static void HandleContentException(Exception e, string customMessage)
        {
            //writer.Write(customMessage + " : " + e.Message, Config.Category.InvoiceManagerApiError, Config.Priority.High);
            //Console.WriteLine(customMessage + " : " + e.Message);
            //if (e.InnerException != null)
            //{
            //    writer.Write(customMessage + " : " + e.InnerException.Message, Config.Category.InvoiceManagerApiError, Config.Priority.High);
            //    Console.WriteLine(customMessage + " : " + e.InnerException.Message);
            //}
            //writer.Write(customMessage + " - " + e.StackTrace, Config.Category.InvoiceManagerApiError, Config.Priority.Highest);
        }
       
    }
}
