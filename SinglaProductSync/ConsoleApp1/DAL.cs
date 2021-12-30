using SinglaApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    class DAL
    {
        public string BulkInsertCustomers(List<CustomerMaster> custMaster, SqlConnection azureConnection)
        {
            string responce = null;
            try
            {
                azureConnection.Open();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                custMaster.Where(i => string.IsNullOrEmpty(i.customercode)).Select(i => { i.customercode = string.Concat("S", i.customerid); return i; }).ToList();
                custMaster.Where(i => (!i.customercode.StartsWith("S") && !i.customercode.StartsWith("P"))).Select(i => { i.customercode = string.Concat("S", i.customercode); return i; }).ToList();
                custMaster.Select(i => { i.UserName = i.customercode; return i; }).ToList();
                custMaster.Select(i => { i.UserPassword = i.customerid.ToString(); return i; }).ToList();
                custMaster.Select(i => { i.syncdtime = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff"); return i; }).ToList();
                var pickingDetails = custMaster.ToList();
                DataTable dt = ToDataTable(pickingDetails);
                using (var bulkCopy = new SqlBulkCopy(azureConnection))
                {
                    bulkCopy.BatchSize = 990;
                    bulkCopy.DestinationTableName = "CustomerMaster";
                    bulkCopy.ColumnMappings.Clear();
                    bulkCopy.ColumnMappings.Add("partnercode", "partnercode");
                    bulkCopy.ColumnMappings.Add("customerid", "customerid");
                    bulkCopy.ColumnMappings.Add("customername", "customername");
                    bulkCopy.ColumnMappings.Add("customercode", "customercode");
                    bulkCopy.ColumnMappings.Add("customeraddress", "customeraddress");
                    bulkCopy.ColumnMappings.Add("creditlimit", "creditlimit");
                    bulkCopy.ColumnMappings.Add("DLExpiry", "DLExpiry");
                    bulkCopy.ColumnMappings.Add("dlnumber", "dlnumber");
                    bulkCopy.ColumnMappings.Add("customeremail", "customeremail");
                    bulkCopy.ColumnMappings.Add("gstnumber", "gstnumber");
                    bulkCopy.ColumnMappings.Add("mobile", "mobile");
                    bulkCopy.ColumnMappings.Add("telephone", "telephone");
                    bulkCopy.ColumnMappings.Add("city", "city");
                    bulkCopy.ColumnMappings.Add("pincode", "pincode");
                    bulkCopy.ColumnMappings.Add("customerstatus", "customerstatus");
                    bulkCopy.ColumnMappings.Add("outstandingbal", "outstandingbal");
                    bulkCopy.ColumnMappings.Add("salesmancode", "salesmancode");
                    bulkCopy.ColumnMappings.Add("salesmanarea", "salesmanarea");
                    bulkCopy.ColumnMappings.Add("salesmanroute", "salesmanroute");
                    bulkCopy.ColumnMappings.Add("Contact", "Contact");
                    bulkCopy.ColumnMappings.Add("State", "State");
                    bulkCopy.ColumnMappings.Add("lock", "lock");
                    bulkCopy.ColumnMappings.Add("lockreason", "lockreason");
                    bulkCopy.ColumnMappings.Add("BranchCode", "BranchCode");
                    bulkCopy.ColumnMappings.Add("UserName", "UserName");
                    bulkCopy.ColumnMappings.Add("UserPassword", "UserPassword");
                    bulkCopy.ColumnMappings.Add("dlnumber1", "dlnumber1");
                    bulkCopy.ColumnMappings.Add("landmark", "landmark");
                    bulkCopy.ColumnMappings.Add("syncdatetime", "syncdatetime");
                    bulkCopy.ColumnMappings.Add("syncdtime", "syncdtime");
                    bulkCopy.WriteToServer(dt);
                    responce = "success";
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("InsertOrderItems - Bulk copy error: " + ex.Message);
            }
            azureConnection.Close();
            return responce;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

    }
}
