using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace SinglaApi.Models
{
    public class DataManager
    {
        /* Get Order&orderitems */

        public List<OrderDetails> GetOrderDetailsByOrderdate(string partnercode, SqlConnection myConnection)
        {
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                myConnection.Open();
                string dbOrder = "select order_id,customer_id,partnercode,NoOfItems,order_date,InvType,Amt,ORDERFROM from Orders where UpdateStatus ='1' and  partnercode='" + partnercode + "' ";
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(dbOrder, myConnection);
                sda.Fill(dt1);
                myConnection.Close();
                orderDetails = (from DataRow row in dt1.Rows
                                select new OrderDetails
                                {
                                    order_id = row["order_id"].ToString(),
                                    customer_id = row["customer_id"].ToString(),
                                    partnercode = row["partnercode"].ToString(),
                                    NoOfItem = Convert.ToInt32(row["NoOfItems"]),
                                    order_date = Convert.ToDateTime(row["order_date"].ToString()),
                                    Amt = Convert.ToDouble(row["Amt"]),
                                    ORDERFROM = row["ORDERFROM"].ToString(),
                                    IsChallan = row["InvType"].ToString(),
                                    orderItemList = GetOrderItemDetailByOrderId(row["order_id"].ToString(), myConnection),
                                }).ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetOrders--GetOrderDetailsByOrderdate--Error : " + partnercode + "--" + ex.Message);
            }
            return orderDetails;
        }

        public List<OrderItems> GetOrderItemDetailByOrderId(string orderid, SqlConnection myConnection)
        {
            List<OrderItems> orderitemDetails = new List<OrderItems>();
            try
            {
                myConnection.Open();
                string dbOrderitems = "select itemname,itemcode,orderqty from orderitems where order_id ='" + orderid + "'";
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(dbOrderitems, myConnection);
                sda.Fill(dt1);
                myConnection.Close();
                orderitemDetails = (from DataRow row in dt1.Rows
                                    select new OrderItems
                                    {
                                        itemname = row["itemname"].ToString(),
                                        itemcode = Convert.ToInt32(row["itemcode"]),
                                        orderqty = row["orderqty"].ToString(),
                                    }).ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetOrders--GetOrderDetailsByOrderdate--GetOrderItemDetailByOrderId--Error : " + orderid + "--" + ex.Message);
            }
            return orderitemDetails;
        }

        /* Update OrderStatus */

        public string UpdateOrderStatus(List<UpdatedOrderStatus> orderStatus, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                if (orderStatus != null)
                {
                    myConnection.Open();
                    foreach (UpdatedOrderStatus updateorderdata in orderStatus)
                    {
                        try
                        {
                            string delyvdate = ConvertDate(updateorderdata);
                            string sql = "UPDATE Orders SET UpdateStatus = @UpdateStatus,order_status =@order_status,delivery_date = @delivery_date Where order_id ='" + updateorderdata.order_id + "' ";
                            SqlCommand cmd = new SqlCommand(sql, myConnection);
                            if (string.IsNullOrEmpty(updateorderdata.UpdateStatus))
                            {
                                updateorderdata.UpdateStatus = "";
                            }
                            if (updateorderdata.order_status == "Delivered")
                                cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = "C";
                            else if (updateorderdata.order_status == "Entire Order is Deleted.")
                                cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = "F";
                            else
                                cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = updateorderdata.UpdateStatus;

                            if (string.IsNullOrEmpty(updateorderdata.order_status))
                                cmd.Parameters.Add("@order_status", SqlDbType.VarChar).Value = "";
                            else
                                cmd.Parameters.Add("@order_status", SqlDbType.VarChar).Value = updateorderdata.order_status;

                            cmd.Parameters.Add("@delivery_date", SqlDbType.VarChar).Value = delyvdate;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            response = "success";
                        }
                        catch (Exception ex)
                        {
                            response = ex.Message;
                            var invoicedetails = JsonConvert.SerializeObject(orderStatus);
                            Trace.TraceInformation("UpdateOrders--UpdateOrderStatus--json : " + invoicedetails + "--" + ex.Message);
                            Trace.TraceInformation("UpdateOrders--UpdateOrderStatus--Error : " + updateorderdata.order_id + "--" + ex.Message);
                        }
                    }
                }
                else
                {
                    response = "Data NotAvailable";
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("UpdateOrders--UpdateOrderStatus--CompleteError : " + ex.Message);
            }
            myConnection.Close();
            return response;
        }

        private static string ConvertDate(UpdatedOrderStatus updateorderdata)
        {
            string dldate = string.Empty;
            try
            {
                string[] date = updateorderdata.delivery_date.Split('-');
                string year = date[0].ToString();
                string month = date[2].ToString().Replace("00", string.Empty).Replace(":", string.Empty).Replace(".", string.Empty).Trim();
                string day = date[1].ToString().Replace("00", string.Empty).Replace(":", string.Empty).Replace(".", string.Empty).Trim();
                return dldate = year + "-" + month + "-" + day;
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("UpdateOrders--UpdateOrderStatus--ConvertDate--Error : " + updateorderdata.order_id + "--" + ex.Message);
                return dldate;
            }
        }

        /* Insert & Update ProductDetails */

        ////public string InsertProductDetailsToAzureDB(List<ItemMaster> itemMaster, SqlConnection myConnection)
        ////{
        ////    string response = string.Empty;
        ////    try
        ////    {
        ////        if (itemMaster != null)
        ////        {
        ////            myConnection.Open();
        ////            foreach (ItemMaster itemdata in itemMaster)
        ////            {
        ////                string itemn = itemdata.itemname.FirstOrDefault().ToString();
        ////                bool match = Regex.IsMatch(itemn, @"[^0-9a-zA-Z\._]");
        ////                if (!match)
        ////                {
        ////                    string ItemCode = GetItemdetails(itemdata, myConnection);
        ////                    string ItemCode = "";
        ////                    if (string.IsNullOrEmpty(ItemCode))
        ////                    {
        ////                        try
        ////                        {
        ////                            string sql = "INSERT INTO ItemMaster(partnercode,itemcode,itemname,manufacturer,packsize,pts,ptr,mrp,stock,"
        ////                                        + "Halfscheme,Scheme,itemstatus,itemid,boxsize,creationdate,substitute,manufacturergroup,itemnamesearch,itemremarks,syncdatetime,syncdtime,PartItemid)"
        ////                                        + "VALUES (@partnercode,@itemcode,@itemname,@manufacturer,@packsize,@pts,@ptr,@mrp,@stock,@Halfscheme,"
        ////                                        + "@Scheme,@itemstatus,@itemid,@boxsize,@creationdate,@substitute,@manufacturergroup,@itemnamesearch,@itemremarks,@syncdatetime,@syncdtime,@PartItemid)";
        ////                            SqlCommand cmd = new SqlCommand(sql, myConnection);
        ////                            cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = itemdata.partnercode;
        ////                            cmd.Parameters.Add("@itemcode", SqlDbType.VarChar).Value = itemdata.itemcode;
        ////                            cmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = itemdata.itemname;
        ////                            cmd.Parameters.Add("@manufacturer", SqlDbType.VarChar).Value = itemdata.manufacturer.Trim();
        ////                            cmd.Parameters.Add("@packsize", SqlDbType.VarChar).Value = itemdata.packsize;
        ////                            cmd.Parameters.Add("@pts", SqlDbType.Float).Value = itemdata.pts;
        ////                            cmd.Parameters.Add("@ptr", SqlDbType.Float).Value = itemdata.ptr;
        ////                            cmd.Parameters.Add("@mrp", SqlDbType.Float).Value = itemdata.mrp;
        ////                            cmd.Parameters.Add("@stock", SqlDbType.Float).Value = itemdata.stock;
        ////                            cmd.Parameters.Add("@Halfscheme", SqlDbType.VarChar).Value = itemdata.Halfscheme;
        ////                            cmd.Parameters.Add("@Scheme", SqlDbType.VarChar).Value = itemdata.Scheme;
        ////                            cmd.Parameters.Add("@itemstatus", SqlDbType.VarChar).Value = itemdata.itemstatus;
        ////                            cmd.Parameters.Add("@itemid", SqlDbType.Int).Value = itemdata.itemid;
        ////                            cmd.Parameters.Add("@boxsize", SqlDbType.Float).Value = itemdata.boxsize;
        ////                            cmd.Parameters.Add("@creationdate", SqlDbType.Date).Value = itemdata.creationdate;
        ////                            cmd.Parameters.Add("@substitute", SqlDbType.VarChar).Value = itemdata.substitute;
        ////                            cmd.Parameters.Add("@manufacturergroup", SqlDbType.VarChar).Value = itemdata.manufacturergroup;
        ////                            cmd.Parameters.Add("@itemnamesearch", SqlDbType.VarChar).Value = itemdata.itemnamesearch;
        ////                            if (string.IsNullOrEmpty(itemdata.itemremarks))
        ////                                cmd.Parameters.Add("@itemremarks", SqlDbType.VarChar).Value = "";
        ////                            else
        ////                                cmd.Parameters.Add("@itemremarks", SqlDbType.VarChar).Value = itemdata.itemremarks;

        ////                            cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = itemdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        ////                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ////                            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        ////                            cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        ////                            cmd.Parameters.Add("@PartItemid", SqlDbType.VarChar).Value = string.Concat(itemdata.partnercode, itemdata.itemid);
        ////                            cmd.CommandType = CommandType.Text;
        ////                            cmd.ExecuteNonQuery();
        ////                            response = "success";
        ////                        }
        ////                        catch (Exception ex)
        ////                        {
        ////                            var invoicedetails = JsonConvert.SerializeObject(itemMaster);
        ////                            Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--Json" + invoicedetails);
        ////                            Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--InsertError : " + itemdata.partnercode + "--" + itemdata.itemcode + "--" + ex.Message);
        ////                            response = ex.Message;
        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        try
        ////                        {
        ////                            string sql = "UPDATE ItemMaster SET itemname = @itemname,manufacturer=@manufacturer,manufacturergroup=@manufacturergroup," +
        ////                                         "packsize=@packsize,pts=@pts,ptr=@ptr,mrp=@mrp,stock=@stock,Halfscheme=@Halfscheme,Scheme=@Scheme,itemstatus=@itemstatus," +
        ////                                         "boxsize=@boxsize,itemremarks=@itemremarks,syncdatetime=@syncdatetime,itemnamesearch=@itemnamesearch,syncdtime=@syncdtime,PartItemid=@PartItemid Where itemid ='" + itemdata.itemid + "' and partnercode='" + itemdata.partnercode + "'";
        ////                            SqlCommand cmd = new SqlCommand(sql, myConnection);
        ////                            cmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = itemdata.itemname;
        ////                            cmd.Parameters.Add("@manufacturer", SqlDbType.VarChar).Value = itemdata.manufacturer;
        ////                            cmd.Parameters.Add("@manufacturergroup", SqlDbType.VarChar).Value = itemdata.manufacturergroup;
        ////                            cmd.Parameters.Add("@packsize", SqlDbType.VarChar).Value = itemdata.packsize;
        ////                            cmd.Parameters.Add("@pts", SqlDbType.Float).Value = itemdata.pts;
        ////                            cmd.Parameters.Add("@ptr", SqlDbType.Float).Value = itemdata.ptr;
        ////                            cmd.Parameters.Add("@mrp", SqlDbType.Float).Value = itemdata.mrp;
        ////                            cmd.Parameters.Add("@stock", SqlDbType.Float).Value = itemdata.stock;
        ////                            cmd.Parameters.Add("@Halfscheme", SqlDbType.VarChar).Value = itemdata.Halfscheme;
        ////                            cmd.Parameters.Add("@Scheme", SqlDbType.VarChar).Value = itemdata.Scheme;
        ////                            cmd.Parameters.Add("@itemstatus", SqlDbType.VarChar).Value = itemdata.itemstatus;
        ////                            cmd.Parameters.Add("@boxsize", SqlDbType.Float).Value = itemdata.boxsize;

        ////                            if (!string.IsNullOrEmpty(itemdata.itemremarks))
        ////                                cmd.Parameters.Add("@itemremarks", SqlDbType.VarChar).Value = itemdata.itemremarks;
        ////                            else
        ////                                cmd.Parameters.Add("@itemremarks", SqlDbType.VarChar).Value = DBNull.Value;

        ////                            cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = itemdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        ////                            cmd.Parameters.Add("@itemnamesearch", SqlDbType.VarChar).Value = itemdata.itemnamesearch;
        ////                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ////                            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        ////                            cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        ////                            cmd.Parameters.Add("@PartItemid", SqlDbType.VarChar).Value = string.Concat(itemdata.partnercode, itemdata.itemid);
        ////                            cmd.CommandType = CommandType.Text;
        ////                            cmd.ExecuteNonQuery();
        ////                            response = "success";
        ////                        }
        ////                        catch (Exception ex)
        ////                        {
        ////                            var invoicedetails = JsonConvert.SerializeObject(itemMaster);
        ////                            Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--Json" + invoicedetails);
        ////                            Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--UpdateError : " + itemdata.partnercode + "--" + itemdata.itemcode + "--" + ex.Message);
        ////                        }
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            response = "Items Not available for this Partner";
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--CompleteError : " + ex.Message);
        ////        response = ex.Message;
        ////    }
        ////    myConnection.Close();
        ////    return response;
        ////}
        public string InsertProductDetailsToAzureDB(List<ItemMaster> itemMaster)
        {
            string response = string.Empty;
            try
            {
                if (itemMaster != null)
                {
                    using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                    {
                        Connection.Open();
                        foreach (ItemMaster itemdata in itemMaster)
                    {
                        string itemn = itemdata.itemname.FirstOrDefault().ToString();
                        bool match = Regex.IsMatch(itemn, @"[^0-9a-zA-Z\._]");
                            if (!match)
                            {

                                //string ItemCode = "";

                                try
                                {
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.CommandText = "[dbo].[InsertandUpdateProductMaster]";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Connection = Connection;
                                        cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = itemdata.partnercode;
                                        cmd.Parameters.Add("@itemcode", SqlDbType.VarChar).Value = itemdata.itemcode;
                                        cmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = itemdata.itemname;
                                        cmd.Parameters.Add("@manufacturer", SqlDbType.VarChar).Value = itemdata.manufacturer.Trim();
                                        cmd.Parameters.Add("@packsize", SqlDbType.VarChar).Value = itemdata.packsize;
                                        cmd.Parameters.Add("@pts", SqlDbType.Float).Value = itemdata.pts;
                                        cmd.Parameters.Add("@ptr", SqlDbType.Float).Value = itemdata.ptr;
                                        cmd.Parameters.Add("@mrp", SqlDbType.Float).Value = itemdata.mrp;
                                        cmd.Parameters.Add("@stock", SqlDbType.Float).Value = itemdata.stock;
                                        cmd.Parameters.Add("@Halfscheme", SqlDbType.VarChar).Value = itemdata.Halfscheme;
                                        cmd.Parameters.Add("@Scheme", SqlDbType.VarChar).Value = itemdata.Scheme;
                                        cmd.Parameters.Add("@itemstatus", SqlDbType.VarChar).Value = itemdata.itemstatus;
                                        cmd.Parameters.Add("@itemid", SqlDbType.Int).Value = itemdata.itemid;
                                        cmd.Parameters.Add("@boxsize", SqlDbType.Float).Value = itemdata.boxsize;
                                        cmd.Parameters.Add("@creationdate", SqlDbType.Date).Value = itemdata.creationdate;
                                        cmd.Parameters.Add("@substitute", SqlDbType.VarChar).Value = itemdata.substitute;
                                        cmd.Parameters.Add("@manufacturergroup", SqlDbType.VarChar).Value = itemdata.manufacturergroup;
                                        cmd.Parameters.Add("@itemnamesearch", SqlDbType.VarChar).Value = itemdata.itemnamesearch;
                                        if (string.IsNullOrEmpty(itemdata.itemremarks))
                                            cmd.Parameters.Add("@itemremarks", SqlDbType.VarChar).Value = "";
                                        else
                                            cmd.Parameters.Add("@itemremarks", SqlDbType.VarChar).Value = itemdata.itemremarks;

                                        cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = itemdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
                                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                                        cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
                                        cmd.Parameters.Add("@PartItemid", SqlDbType.VarChar).Value = string.Concat(itemdata.partnercode, itemdata.itemid);
                                        
                                        cmd.ExecuteNonQuery();
                                        response = "success";
                                            }
                                    }
                                catch (Exception ex)
                                {
                                    var invoicedetails = JsonConvert.SerializeObject(itemMaster);
                                    Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--Json" + invoicedetails);
                                    Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--InsertError : " + itemdata.partnercode + "--" + itemdata.itemcode + "--" + ex.Message);
                                    response = ex.Message;
                                }
                            }
                        }
                        Connection.Close();
                    }
                }
                else
                {
                    response = "Items Not available for this Partner";
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--CompleteError : " + ex.Message);
                response = ex.Message;
            }
           
            return response;
        }
        public int GetItemdetailsCount(int partnercode, SqlConnection myConnection)
        {
            int response = 0;
            try
            {
                myConnection.Open();
                string sql = "select Count(*) from ItemMaster where Partnercode='" + partnercode + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                response = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            myConnection.Close();
            return response;
        }

        public int GetCustomerdetailsCount(int partnercode, SqlConnection myConnection)
        {
            int response = 0;
            try
            {
                myConnection.Open();
                string sql = "select Count(*) from CustomerMaster where Partnercode='" + partnercode + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                response = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            myConnection.Close();
            return response;
        }

        public string GetItemdetails(ItemMaster itemdata, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                string PartItemid= string.Concat(itemdata.partnercode, itemdata.itemid);
                string sql = "select id from ItemMaster where PartItemid='"+PartItemid+"'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                response = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                var invoicedetails = JsonConvert.SerializeObject(itemdata);
                Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--GetItemdetails--Json" + invoicedetails);
                Trace.TraceInformation("SyncProduct--InsertProductDetailsToAzureDB--GetItemdetails : " + itemdata.partnercode + "--" + itemdata.itemcode + "--" + ex.Message);
            }
            return response;
        }

        public  string BulkInsertProducts(List<ItemMaster> orderitemList)
        {
            string responce = null;
            try
            {
                using (SqlConnection azureConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {

                    azureConnection.Open();
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    orderitemList = orderitemList.Select(i => { i.PartItemid = string.Concat(i.partnercode, i.itemid); return i; }).ToList();
                    orderitemList = orderitemList.Select(i => { i.syncdtime = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff"); return i; }).ToList();
                    var pickingDetails = orderitemList.ToList();
                    DataTable dt = ToDataTable(pickingDetails);
                    using (var bulkCopy = new SqlBulkCopy(azureConnection))
                    {
                        bulkCopy.BatchSize = 990;
                        bulkCopy.DestinationTableName = "ItemMaster";
                        bulkCopy.ColumnMappings.Clear();
                        bulkCopy.ColumnMappings.Add("partnercode", "partnercode");
                        bulkCopy.ColumnMappings.Add("itemid", "itemid");
                        bulkCopy.ColumnMappings.Add("itemcode", "itemcode");
                        bulkCopy.ColumnMappings.Add("itemname", "itemname");
                        bulkCopy.ColumnMappings.Add("manufacturer", "manufacturer");
                        bulkCopy.ColumnMappings.Add("packsize", "packsize");
                        bulkCopy.ColumnMappings.Add("pts", "pts");
                        bulkCopy.ColumnMappings.Add("ptr", "ptr");
                        bulkCopy.ColumnMappings.Add("mrp", "mrp");
                        bulkCopy.ColumnMappings.Add("boxsize", "boxsize");
                        bulkCopy.ColumnMappings.Add("stock", "stock");
                        bulkCopy.ColumnMappings.Add("Halfscheme", "Halfscheme");
                        bulkCopy.ColumnMappings.Add("Scheme", "Scheme");
                        bulkCopy.ColumnMappings.Add("itemstatus", "itemstatus");
                        bulkCopy.ColumnMappings.Add("creationdate", "creationdate");
                        bulkCopy.ColumnMappings.Add("substitute", "substitute");
                        bulkCopy.ColumnMappings.Add("manufacturergroup", "manufacturergroup");
                        bulkCopy.ColumnMappings.Add("syncdatetime", "syncdatetime");
                        bulkCopy.ColumnMappings.Add("itemnamesearch", "itemnamesearch");
                        bulkCopy.ColumnMappings.Add("itemremarks", "itemremarks");
                        bulkCopy.ColumnMappings.Add("caseqty", "caseqty");
                        bulkCopy.ColumnMappings.Add("maxinvqty", "maxinvqty");
                        bulkCopy.ColumnMappings.Add("PartItemid", "PartItemid");
                        bulkCopy.ColumnMappings.Add("syncdtime", "syncdtime");
                        bulkCopy.WriteToServer(dt);
                        responce = "success";
                    }
                    azureConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("InsertOrderItems - Bulk copy error: " + ex.Message);
            }
            
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

        public string BulkInsertCustomers(List<CustomerMaster> custMaster, SqlConnection azureConnection)
        {
            string responce = null;
            try
            {
                azureConnection.Open();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                custMaster.Where(i => string.IsNullOrEmpty(i.customercode)).Select(i => { i.customercode = string.Concat("S", i.customerid); return i; }).ToList();
                custMaster.Where(i =>( !i.customercode.StartsWith("S") && !i.customercode.StartsWith("P"))).Select(i => { i.customercode = string.Concat("S", i.customercode); return i; }).ToList();
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

        /* Insert & Update CustomerDetails */
        public string InsertCustomerDetailsToAzureDB(List<CustomerMaster> custMaster)
        {
            string response = string.Empty;
            try
            {
                if (custMaster != null)
                {
                    using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                    {
                        Connection.Open();
                        foreach (CustomerMaster custdata in custMaster)
                        {
                            if (custdata.customeraddress == null)
                                custdata.customeraddress = "";
                            if (custdata.lockreason == null)
                                custdata.lockreason = "";
                            try
                            {

                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    cmd.CommandText = "[dbo].[InsertandUpdateCustomerMaster]";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = Connection;
                                    cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = custdata.partnercode;
                                    cmd.Parameters.Add("@customerid", SqlDbType.Float).Value = custdata.customerid;
                                    cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = custdata.customername;
                                    if (string.IsNullOrEmpty(custdata.customercode))
                                    {
                                        cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = string.Concat('S', custdata.customerid);
                                    }
                                    else
                                    {
                                        if (custdata.customercode.StartsWith("S") || custdata.customercode.StartsWith("P"))
                                            cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = custdata.customercode;
                                        else
                                            cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = string.Concat('S', custdata.customercode);
                                    }
                                    cmd.Parameters.Add("@customeraddress", SqlDbType.VarChar).Value = custdata.customeraddress;
                                    cmd.Parameters.Add("@creditlimit", SqlDbType.Decimal).Value = custdata.creditlimit;
                                    cmd.Parameters.Add("@DLExpiry", SqlDbType.Date).Value = custdata.DLExpiry;
                                    cmd.Parameters.Add("@dlnumber", SqlDbType.VarChar).Value = custdata.dlnumber;
                                    cmd.Parameters.Add("@customeremail", SqlDbType.VarChar).Value = custdata.customeremail;
                                    cmd.Parameters.Add("@gstnumber", SqlDbType.VarChar).Value = custdata.gstnumber.ToString();
                                    cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = custdata.mobile;
                                    cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = custdata.telephone.ToString();
                                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = custdata.city;
                                    cmd.Parameters.Add("@pincode", SqlDbType.Int).Value = custdata.pincode;
                                    cmd.Parameters.Add("@customerstatus", SqlDbType.VarChar).Value = custdata.customerstatus;
                                    cmd.Parameters.Add("@outstandingbal", SqlDbType.Decimal).Value = custdata.outstandingbal;
                                    cmd.Parameters.Add("@CreditLockDays", SqlDbType.Int).Value = custdata.CreditLockDays;
                                    cmd.Parameters.Add("@salesmancode", SqlDbType.Int).Value = custdata.salesmancode;
                                    cmd.Parameters.Add("@salesmanarea", SqlDbType.Int).Value = custdata.salesmanarea;
                                    cmd.Parameters.Add("@salesmanroute", SqlDbType.Int).Value = custdata.salesmanroute;
                                    cmd.Parameters.Add("@Contact", SqlDbType.VarChar).Value = custdata.Contact;
                                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = custdata.State;
                                    cmd.Parameters.Add("@lock", SqlDbType.Bit).Value = custdata.Lock;
                                    cmd.Parameters.Add("@Active", SqlDbType.Int).Value = custdata.Active;
                                    cmd.Parameters.Add("@lockreason", SqlDbType.VarChar).Value = custdata.lockreason;
                                    cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = custdata.BranchCode;
                                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = custdata.customercode;
                                    cmd.Parameters.Add("@UserPassword", SqlDbType.VarChar).Value = custdata.customerid;
                                    cmd.Parameters.Add("@dlnumber1", SqlDbType.VarChar).Value = custdata.dlnumber1;
                                    cmd.Parameters.Add("@landmark", SqlDbType.VarChar).Value = custdata.landmark;
                                    cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = custdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
                                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                                    cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
                                    cmd.ExecuteNonQuery();
                                    response = "success";
                                }

                            }
                            catch (Exception ex)
                            {
                                var invoicedetails = JsonConvert.SerializeObject(custMaster);
                                Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--json : " + invoicedetails);
                                response = ex.Message;
                                Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--InsertError : " + custdata.partnercode + "--" + custdata.customercode + "--" + ex.Message);
                            }
                        }
                        Connection.Close();
                    }
                }
                else
                {
                    response = "Data NotAvailable";
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
                Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--CompleteError : " + ex.Message);
            }
            return response;
        }
        //public string InsertCustomerDetailsToAzureDB(List<CustomerMaster> custMaster, SqlConnection myConnection)
        //{
        //    string response = string.Empty;
        //    try
        //    {
        //        if (custMaster != null)
        //        {
                   
        //            foreach (CustomerMaster custdata in custMaster)
        //            {
        //                if (custdata.customeraddress == null)
        //                    custdata.customeraddress = "";
        //                if (custdata.lockreason == null)
        //                    custdata.lockreason = "";
        //                string customercode = GetCustomerdetails(custdata, myConnection);
        //                myConnection.Open();
        //                //string customercode = "";
        //                if (string.IsNullOrEmpty(customercode))
        //                {
        //                    try
        //                    {
        //                        string sql = "INSERT INTO CustomerMaster(partnercode,customerid,customername,customercode,customeraddress,"
        //                                    + "creditlimit,DLExpiry,dlnumber,customeremail,gstnumber,mobile,telephone,city,pincode,"
        //                                     + "customerstatus,outstandingbal,CreditLockDays,salesmancode,salesmanarea,salesmanroute,Contact,State,lock,lockreason,BranchCode,UserName,UserPassword,dlnumber1,landmark,syncdatetime,syncdtime)"
        //                                    + "VALUES (@partnercode,@customerid,@customername,@customercode,@customeraddress,@creditlimit,@DLExpiry,"
        //                                    + "@dlnumber,@customeremail,@gstnumber,@mobile,@telephone,@city,@pincode,"
        //                                    + "@customerstatus,@outstandingbal,@CreditLockDays,@salesmancode,@salesmanarea,@salesmanroute,@Contact,@State,@lock,@lockreason,@BranchCode,@UserName,@UserPassword,@dlnumber1,@landmark,@syncdatetime,@syncdtime)";
        //                        SqlCommand cmd = new SqlCommand(sql, myConnection);
        //                        cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = custdata.partnercode;
        //                        cmd.Parameters.Add("@customerid", SqlDbType.Float).Value = custdata.customerid;
        //                        cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = custdata.customername;
        //                        if (string.IsNullOrEmpty(custdata.customercode))
        //                        {
        //                            cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = string.Concat('S', custdata.customerid) ;
        //                        }
        //                        else
        //                        {
        //                            if(custdata.customercode.StartsWith("S")|| custdata.customercode.StartsWith("P"))
        //                                cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = custdata.customercode;
        //                            else
        //                                cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = string.Concat('S', custdata.customercode);
        //                        }
        //                        cmd.Parameters.Add("@customeraddress", SqlDbType.VarChar).Value = custdata.customeraddress;
        //                        cmd.Parameters.Add("@creditlimit", SqlDbType.Decimal).Value = custdata.creditlimit;
        //                        cmd.Parameters.Add("@DLExpiry", SqlDbType.Date).Value = custdata.DLExpiry;
        //                        cmd.Parameters.Add("@dlnumber", SqlDbType.VarChar).Value = custdata.dlnumber;
        //                        cmd.Parameters.Add("@customeremail", SqlDbType.VarChar).Value = custdata.customeremail;
        //                        cmd.Parameters.Add("@gstnumber", SqlDbType.VarChar).Value = custdata.gstnumber.ToString();
        //                        cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = custdata.mobile;
        //                        cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = custdata.telephone.ToString();
        //                        cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = custdata.city;
        //                        cmd.Parameters.Add("@pincode", SqlDbType.Int).Value = custdata.pincode;
        //                        cmd.Parameters.Add("@customerstatus", SqlDbType.VarChar).Value = custdata.customerstatus;
        //                        cmd.Parameters.Add("@outstandingbal", SqlDbType.Decimal).Value = custdata.outstandingbal;
        //                        cmd.Parameters.Add("@CreditLockDays", SqlDbType.Int).Value = custdata.CreditLockDays;
        //                        cmd.Parameters.Add("@salesmancode", SqlDbType.Int).Value = custdata.salesmancode;
        //                        cmd.Parameters.Add("@salesmanarea", SqlDbType.Int).Value = custdata.salesmanarea;
        //                        cmd.Parameters.Add("@salesmanroute", SqlDbType.Int).Value = custdata.salesmanroute;
        //                        cmd.Parameters.Add("@Contact", SqlDbType.VarChar).Value = custdata.Contact;
        //                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = custdata.State;
        //                        cmd.Parameters.Add("@lock", SqlDbType.Bit).Value = custdata.Lock;
        //                        cmd.Parameters.Add("@lockreason", SqlDbType.VarChar).Value = custdata.lockreason;
        //                        cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = custdata.BranchCode;
        //                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = custdata.customercode;
        //                        cmd.Parameters.Add("@UserPassword", SqlDbType.VarChar).Value = custdata.customerid;
        //                        cmd.Parameters.Add("@dlnumber1", SqlDbType.VarChar).Value = custdata.dlnumber1;
        //                        cmd.Parameters.Add("@landmark", SqlDbType.VarChar).Value = custdata.landmark;
        //                        cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = custdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        //                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //                        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //                        cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        //                        cmd.CommandType = CommandType.Text;
        //                        cmd.ExecuteNonQuery();
        //                        response = "success";
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var invoicedetails = JsonConvert.SerializeObject(custMaster);
        //                        Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--json : " + invoicedetails);
        //                        response = ex.Message;
        //                        Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--InsertError : " + custdata.partnercode + "--" + custdata.customercode + "--" + ex.Message);
        //                    }
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        string sql = "UPDATE  CustomerMaster SET customername=@customername,customeraddress=@customeraddress,creditlimit=@creditlimit," +
        //                                                            "DLExpiry=@DLExpiry,dlnumber=@dlnumber,customeremail=@customeremail,gstnumber=@gstnumber," +
        //                                                           "mobile=@mobile,telephone=@telephone,city=@city,pincode=@pincode,customerstatus=@customerstatus,"
        //                                                           + "outstandingbal=@outstandingbal,CreditLockDays=@CreditLockDays,salesmancode=@salesmancode,active=@Active,Contact=@Contact,State=@State," +
        //                                                           "salesmanarea=@salesmanarea,salesmanroute=@salesmanroute,lock=@lock,lockreason=@lockreason,BranchCode=@BranchCode,dlnumber1=@dlnumber1,landmark=@landmark,syncdatetime=@syncdatetime,syncdtime=@syncdtime Where customerid = '" + custdata.customerid + "' and BranchCode='" + custdata.BranchCode + "' and partnercode='" + custdata.partnercode + "'";
        //                        SqlCommand cmd = new SqlCommand(sql, myConnection);
        //                        if (string.IsNullOrEmpty(custdata.customercode))
        //                        {
        //                            cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = string.Concat('S', custdata.customerid);
        //                        }
        //                        else
        //                        {
        //                            if (custdata.customercode.StartsWith("S") || custdata.customercode.StartsWith("P"))
        //                                cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = custdata.customercode;
        //                            else
        //                                cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = string.Concat('S', custdata.customercode);
        //                        }
        //                        cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = custdata.customername;
        //                        cmd.Parameters.Add("@customeraddress", SqlDbType.VarChar).Value = custdata.customeraddress;
        //                        cmd.Parameters.Add("@creditlimit", SqlDbType.Decimal).Value = custdata.creditlimit;
        //                        cmd.Parameters.Add("@DLExpiry", SqlDbType.Date).Value = custdata.DLExpiry;
        //                        cmd.Parameters.Add("@dlnumber", SqlDbType.VarChar).Value = custdata.dlnumber;
        //                        cmd.Parameters.Add("@customeremail", SqlDbType.VarChar).Value = custdata.customeremail;
        //                        cmd.Parameters.Add("@gstnumber", SqlDbType.VarChar).Value = custdata.gstnumber.ToString();
        //                        cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = custdata.mobile;
        //                        cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = custdata.telephone.ToString();
        //                        cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = custdata.city;
        //                        cmd.Parameters.Add("@pincode", SqlDbType.Int).Value = custdata.pincode;
        //                        cmd.Parameters.Add("@customerstatus", SqlDbType.VarChar).Value = custdata.customerstatus;
        //                        cmd.Parameters.Add("@outstandingbal", SqlDbType.Decimal).Value = custdata.outstandingbal;
        //                        cmd.Parameters.Add("@CreditLockDays", SqlDbType.Int).Value = custdata.CreditLockDays;
        //                        cmd.Parameters.Add("@salesmancode", SqlDbType.Int).Value = custdata.salesmancode;
        //                        cmd.Parameters.Add("@salesmanarea", SqlDbType.Int).Value = custdata.salesmanarea;
        //                        cmd.Parameters.Add("@salesmanroute", SqlDbType.Int).Value = custdata.salesmanroute;
        //                        cmd.Parameters.Add("@Active", SqlDbType.Int).Value = custdata.Active;
        //                        cmd.Parameters.Add("@Contact", SqlDbType.VarChar).Value = custdata.Contact;
        //                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = custdata.State;
        //                        cmd.Parameters.Add("@lock", SqlDbType.Bit).Value = custdata.Lock;
        //                        cmd.Parameters.Add("@lockreason", SqlDbType.VarChar).Value = custdata.lockreason;
        //                        cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = custdata.BranchCode;
        //                        cmd.Parameters.Add("@dlnumber1", SqlDbType.VarChar).Value = custdata.dlnumber1;
        //                        cmd.Parameters.Add("@landmark", SqlDbType.VarChar).Value = custdata.landmark;
        //                        cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = custdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        //                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //                        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //                        cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
        //                        cmd.CommandType = CommandType.Text;
        //                        cmd.ExecuteNonQuery();
        //                        response = "success";
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        var invoicedetails = JsonConvert.SerializeObject(custMaster);
        //                        Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--json : " + invoicedetails);
        //                        response = ex.Message;
        //                        Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--UpdateError : " + custdata.partnercode + "--" + custdata.customercode + "--" + ex.Message);
        //                    }
        //                }
        //                myConnection.Close();
        //            }
        //        }
        //        else
        //        {
        //            response = "Data NotAvailable";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response = ex.Message;
        //        Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--CompleteError : " + ex.Message);
        //    }
           
        //    return response;
        //}

        public string GetCustomerdetails(CustomerMaster custdata, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    Connection.Open();
                    string sql = "select id from CustomerMaster where customerid=" + custdata.customerid + " and partnercode=" + custdata.partnercode + "";
                    SqlCommand cmd = new SqlCommand(sql, Connection);
                    response = (string)cmd.ExecuteScalar();
                    Connection.Close();
                }
                
            }
            catch (Exception ex)
            {
                var invoicedetails = JsonConvert.SerializeObject(custdata);
                Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--GetCustomerdetails--json : " + invoicedetails);
                Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--GetCustomerdetails : " + custdata.partnercode + "--" + custdata.customercode + "--" + ex.Message);
            }
           
            return response;
        }

        /* Insert & Update SalesmanDetails */

        public string InsertSalesmanDetailsToAzureDB(List<SalesmanMaster> salesMaster, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                if (salesMaster != null)
                {
                  
                    foreach (SalesmanMaster salesdata in salesMaster)
                    {
                        string salesmancode = GetSalesmandetails(salesdata, myConnection);
                        if (string.IsNullOrEmpty(salesmancode))
                        {
                            try
                            {
                                myConnection.Open();
                                string sql = "INSERT INTO SalesmanMaster(partnercode,username,salesmanid,"
                                            + "salesmancode,salesmanname,mobile,telephone,salesmantarget,salesmancommision,active,userpassword,syncdatetime,syncdtime)"
                                            + "VALUES (@partnercode,@username,@salesmanid,"
                                            + "@salesmancode,@salesmanname,@mobile,@telephone,@salesmantarget,@salesmancommision,@Active,@userpassword,@syncdatetime,@syncdtime)";
                                SqlCommand cmd = new SqlCommand(sql, myConnection);
                                cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = salesdata.partnercode;
                                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = salesdata.username;
                                cmd.Parameters.Add("@salesmanid", SqlDbType.Int).Value = salesdata.salesmanid;
                                cmd.Parameters.Add("@salesmancode", SqlDbType.VarChar).Value = salesdata.salesmancode;
                                cmd.Parameters.Add("@salesmanname", SqlDbType.VarChar).Value = salesdata.salesmanname;
                                cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = salesdata.mobile;
                                cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = salesdata.telephone;
                                cmd.Parameters.Add("@salesmantarget", SqlDbType.Float).Value = salesdata.salesmantarget;
                                cmd.Parameters.Add("@salesmancommision", SqlDbType.VarChar).Value = salesdata.salesmancommision;
                                cmd.Parameters.Add("@Active", SqlDbType.Int).Value = salesdata.Active;
                                cmd.Parameters.Add("@userpassword", SqlDbType.VarChar).Value = salesdata.salesmanid.ToString();
                                cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = salesdata.syncdatetime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
                                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                                cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
                                cmd.CommandType = CommandType.Text;
                                cmd.ExecuteNonQuery();
                                response = "success";
                            }
                            catch (Exception ex)
                            {
                                var invoicedetails = JsonConvert.SerializeObject(salesMaster);
                                Trace.TraceInformation("SyncSalesman--InsertSalesmanDetailsToAzureDB--json : " + invoicedetails + "--" + ex.Message);
                                Trace.TraceInformation("SyncSalesman--InsertSalesmanDetailsToAzureDB--InsertError : " + salesdata.partnercode + "--" + salesdata.salesmancode + "--" + ex.Message);
                                response = ex.Message;
                            }
                            myConnection.Close();
                        }
                        else
                        {
                            try
                            {
                                myConnection.Open();
                                string sql = "UPDATE  SalesmanMaster SET username=@username,"
                                            + "salesmanname=@salesmanname,mobile=@mobile,telephone=@telephone,active=@Active," +
                                            "salesmantarget=@salesmantarget,salesmancommision=@salesmancommision,userpassword=@userpassword,syncdatetime=@syncdatetime,syncdtime=@syncdtime Where salesmanid ='" + salesdata.salesmanid + "' and partnercode='" + salesdata.partnercode + "'";
                                SqlCommand cmd = new SqlCommand(sql, myConnection);
                                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = salesdata.username;
                                cmd.Parameters.Add("@salesmanname", SqlDbType.VarChar).Value = salesdata.salesmanname;
                                cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = salesdata.mobile;
                                cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = salesdata.telephone;
                                cmd.Parameters.Add("@salesmantarget", SqlDbType.Float).Value = salesdata.salesmantarget;
                                cmd.Parameters.Add("@salesmancommision", SqlDbType.VarChar).Value = salesdata.salesmancommision;
                                cmd.Parameters.Add("@Active", SqlDbType.Int).Value = salesdata.Active;
                                cmd.Parameters.Add("@userpassword", SqlDbType.VarChar).Value = salesdata.salesmanid.ToString();
                                cmd.Parameters.Add("@syncdatetime", SqlDbType.Date).Value = salesdata.syncdatetime.ToString("yyyy-MM-dd");
                                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                                cmd.Parameters.Add("@syncdtime", SqlDbType.DateTime).Value = indianTime.ToString("yyyy-MM-dd HH:mm:ss");
                                cmd.CommandType = CommandType.Text;
                                cmd.ExecuteNonQuery();
                                response = "success";
                            }
                            catch (Exception ex)
                            {
                                var invoicedetails = JsonConvert.SerializeObject(salesMaster);
                                Trace.TraceInformation("SyncSalesman--InsertSalesmanDetailsToAzureDB--json : " + invoicedetails + "--" + ex.Message);
                                Trace.TraceInformation("SyncSalesman--InsertSalesmanDetailsToAzureDB--UpdateError : " + salesdata.partnercode + "--" + salesdata.salesmancode + "--" + ex.Message);
                                response = ex.Message;
                            }
                            myConnection.Close();
                        }
                    }
                }
                else
                {
                    response = "Data NotAvailable";
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
                Trace.TraceInformation("SyncSalesman--InsertSalesmanDetailsToAzureDB--CompleatError : " + ex.Message);
            }
          
            return response;
        }

        public string GetSalesmandetails(SalesmanMaster salesdata, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                myConnection.Open();
                string sql = "select id from SalesmanMaster where salesmanid=" + salesdata.salesmanid + " and partnercode=" + salesdata.partnercode + "";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                response = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                var invoicedetails = JsonConvert.SerializeObject(salesdata);
                Trace.TraceInformation("SyncCustomer--InsertCustomerDetailsToAzureDB--GetSalesmandetails--json : " + invoicedetails);
                Trace.TraceInformation("SyncSalesman--InsertSalesmanDetailsToAzureDB--GetSalesmandetails : " + salesdata.partnercode + "--" + salesdata.salesmancode + "--" + ex.Message);
            }
            myConnection.Close();
            return response;
        }

        /* Update InvoiceDetails Order & OrderItems */

        public string UpdatedInvoiceStatus(List<InvoiceDetail> invoiceStatus, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                if (invoiceStatus != null)
                {
                    myConnection.Open();
                    foreach (InvoiceDetail updatainvoicedata in invoiceStatus)
                    {
                        if (updatainvoicedata.Invoice_No != null)
                        {
                            UpdateOrderDetails(updatainvoicedata, myConnection);
                            foreach (InvoiceItemDetail itemDetail in updatainvoicedata.InvoiceItemList)
                            {
                                response = UpdateOrderItemDetails(itemDetail, updatainvoicedata, myConnection);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
                var invoicedetails = JsonConvert.SerializeObject(invoiceStatus);
                Trace.TraceInformation("UpdateInvoiceDetail--UpdatedInvoiceStatus--Json : " + invoicedetails);
                Trace.TraceInformation("UpdateInvoiceDetail--UpdatedInvoiceStatus--CompleteError : " + ex.Message);
            }
            myConnection.Close();
            return response;
        }

        public string UpdateOrderDetails(InvoiceDetail updatainvoicedata, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                string sql = "UPDATE Orders SET Invoice_No=@Invoice_No,Invoice_date=@Invoice_date,order_status=@order_status,UpdateStatus=@UpdateStatus  Where order_id ='" + updatainvoicedata.order_id + "' ";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                cmd.Parameters.Add("@Invoice_No", SqlDbType.VarChar).Value = updatainvoicedata.Invoice_No;
                cmd.Parameters.Add("@Invoice_date", SqlDbType.DateTime).Value = updatainvoicedata.Invoice_date;
                cmd.Parameters.Add("@order_status", SqlDbType.VarChar).Value = updatainvoicedata.order_status;

                if (updatainvoicedata.order_status == "Delivered")
                    cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = "C";
                else if (updatainvoicedata.order_status == "Entire Order is Deleted.")
                    cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = "F";
                else
                    cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = updatainvoicedata.UpdateStatus;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                response = "success";
            }
            catch (Exception ex)
            {
                response = ex.Message;
                var invoicedetails = JsonConvert.SerializeObject(updatainvoicedata);
                Trace.TraceInformation("UpdateInvoiceDetail--UpdatedInvoiceStatus--UpdateOrderDetails--Json : " + invoicedetails);
                Trace.TraceInformation("UpdateInvoiceDetail--UpdatedInvoiceStatus--UpdateOrderDetails--Error : " + updatainvoicedata.order_id + "--" + updatainvoicedata.Invoice_No + "--" + ex.Message);
            }
            return response;
        }

        public string UpdateOrderItemDetails(InvoiceItemDetail itemDetail, InvoiceDetail updatainvoicedata, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                string sql = "UPDATE orderitems SET Invoice_No=@Invoice_No,Invoice_date=@Invoice_date,QtyRecd=@QtyRecd  Where order_id ='" + updatainvoicedata.order_id + "' and itemcode ='" + itemDetail.itemcode + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                cmd.Parameters.Add("@Invoice_No", SqlDbType.VarChar).Value = updatainvoicedata.Invoice_No;
                cmd.Parameters.Add("@Invoice_date", SqlDbType.DateTime).Value = updatainvoicedata.Invoice_date;
                cmd.Parameters.Add("@QtyRecd", SqlDbType.Float).Value = itemDetail.Qty;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                response = "success";
            }
            catch (Exception ex)
            {
                response = ex.Message;
                var invoice = JsonConvert.SerializeObject(itemDetail);
                var invoicedetails = JsonConvert.SerializeObject(updatainvoicedata);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--json : " + invoicedetails + "---" + invoice);
                Trace.TraceInformation("UpdateInvoiceDetail--UpdatedInvoiceStatus--UpdateOrderItemDetails--Error : " + updatainvoicedata.order_id + "--" + updatainvoicedata.Invoice_No + "--" + ex.Message);
            }
            return response;
        }

        /*Insert & Update Outstanding */

        public string InsertPaymentDetailsToAzureDB(List<PaymentDetail> PaymentDetails, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                if (PaymentDetails != null)
                {
                    myConnection.Open();
                    foreach (PaymentDetail salesdata in PaymentDetails)
                    {
                        if (salesdata.partnercode != 1028 && salesdata.partnercode != 1029 && salesdata.partnercode != 1030)
                        {
                            if (salesdata.BalanceAmount != 0)
                            {
                                string InvoiceNum = GetPaymentDetails(salesdata, myConnection);
                                if (string.IsNullOrEmpty(InvoiceNum))
                                {
                                    if (string.IsNullOrEmpty(salesdata.OrderNumber))
                                    {
                                        salesdata.OrderNumber = "";
                                    }
                                    try
                                    {
                                        string sql = "INSERT INTO OutStanding(custid,custcode,customername,address,telephone,mobile,partnercode,Invoice_Type,InvoiceNum,InvoiceDate,CreditDebit,OrderNumber,status,CreatedOn,CreatedBy,TotalAmount,TotalDiscount,NetAmount,BalanceAmount,BillAmount,GSTAmount,Credit_hold,Smancode,Area,Routeid,Remarks,Items,syncdatetime)VALUES (@custid,@custcode,@customername,@address,@telephone,@mobile,@partnercode,@Invoice_Type,@InvoiceNum,@InvoiceDate,@CreditDebit,@OrderNumber,@status,@CreatedOn,@CreatedBy,@TotalAmount,@TotalDiscount,@NetAmount,@BalanceAmount,@BillAmount,@GSTAmount,@Credit_hold,@Smancode,@Area,@Routeid,@Remarks,@Items,@syncdatetime)";
                                        SqlCommand cmd = new SqlCommand(sql, myConnection);
                                        cmd.Parameters.Add("@custid", SqlDbType.Int).Value = salesdata.custid;
                                        cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = salesdata.custcode;
                                        cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = salesdata.customername;
                                        cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = salesdata.address;
                                        cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = salesdata.telephone;
                                        cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = salesdata.mobile;
                                        cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = salesdata.partnercode;
                                        cmd.Parameters.Add("@Invoice_Type", SqlDbType.VarChar).Value = salesdata.Invoice_Type;
                                        cmd.Parameters.Add("@InvoiceNum", SqlDbType.VarChar).Value = salesdata.InvoiceNum;
                                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = salesdata.InvoiceDate;
                                        cmd.Parameters.Add("@CreditDebit", SqlDbType.Int).Value = salesdata.CreditDebit;
                                        cmd.Parameters.Add("@OrderNumber", SqlDbType.VarChar).Value = salesdata.OrderNumber;
                                        cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = salesdata.status;
                                        cmd.Parameters.Add("@CreatedOn", SqlDbType.Date).Value = salesdata.CreatedOn;
                                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = salesdata.CreatedBy;
                                        cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = salesdata.TotalAmount;
                                        cmd.Parameters.Add("@TotalDiscount", SqlDbType.Decimal).Value = salesdata.TotalDiscount;
                                        cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = salesdata.NetAmount;
                                        cmd.Parameters.Add("@BalanceAmount", SqlDbType.Decimal).Value = salesdata.BalanceAmount;
                                        cmd.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = salesdata.BillAmount;
                                        cmd.Parameters.Add("@GSTAmount", SqlDbType.Decimal).Value = salesdata.GSTAmount;
                                        cmd.Parameters.Add("@Credit_hold", SqlDbType.Int).Value = salesdata.Credit_hold;
                                        cmd.Parameters.Add("@Smancode", SqlDbType.Int).Value = salesdata.Smancode;
                                        cmd.Parameters.Add("@Area", SqlDbType.Int).Value = salesdata.Area;
                                        cmd.Parameters.Add("@Routeid", SqlDbType.Int).Value = salesdata.Routeid;
                                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = salesdata.Remarks;
                                        cmd.Parameters.Add("@Items", SqlDbType.Int).Value = salesdata.Items;
                                        cmd.Parameters.Add("@syncdatetime", SqlDbType.Date).Value = salesdata.syncdatetime;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        var invoicedetails = JsonConvert.SerializeObject(PaymentDetails);
                                        Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--json : " + invoicedetails);
                                        response = ex.Message;
                                        Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--InsertError : " + salesdata.partnercode + "--" + salesdata.custcode + "--" + ex.Message);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        string sql = "UPDATE OutStanding SET TotalAmount=@TotalAmount,TotalDiscount=@TotalDiscount,BillAmount=@BillAmount,GSTAmount=@GSTAmount,BalanceAmount=@BalanceAmount,NetAmount=@NetAmount,syncdatetime=@syncdatetime  Where custid ='" + salesdata.custid + "' and custcode ='" + salesdata.custcode + "' and InvoiceNum ='" + salesdata.InvoiceNum + "' and partnercode='" + salesdata.partnercode + "'";
                                        SqlCommand cmd = new SqlCommand(sql, myConnection);
                                        cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = salesdata.TotalAmount;
                                        cmd.Parameters.Add("@TotalDiscount", SqlDbType.Decimal).Value = salesdata.TotalDiscount;
                                        cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = salesdata.NetAmount;
                                        cmd.Parameters.Add("@BalanceAmount", SqlDbType.Decimal).Value = salesdata.BalanceAmount;
                                        cmd.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = salesdata.BillAmount;
                                        cmd.Parameters.Add("@GSTAmount", SqlDbType.Decimal).Value = salesdata.GSTAmount;
                                        cmd.Parameters.Add("@syncdatetime", SqlDbType.Date).Value = salesdata.syncdatetime;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        var invoicedetails = JsonConvert.SerializeObject(PaymentDetails);
                                        Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--json : " + invoicedetails);
                                        response = ex.Message;
                                        Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--UpdateError:" + salesdata.partnercode + "--" + salesdata.custcode + "--" + ex.Message);
                                    }
                                }
                           
                            }
                            else
                            {
                                string Insertstatus = InsertOutstandingUpdTable(salesdata, myConnection);
                                if (!string.IsNullOrEmpty(Insertstatus) && Insertstatus == "success")
                                    DeleteOutstanding(salesdata, myConnection);
                            }
                        }
                    }
                    response = "success";
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
                var invoicedetails = JsonConvert.SerializeObject(PaymentDetails);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--CompleteError--json : " + invoicedetails);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--CompleteError : " + ex.Message);
            }
            myConnection.Close();
            return response;
        }

        public string GetPaymentDetails(PaymentDetail salesdata, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                string sql = "select InvoiceNum from OutStanding where custcode='" + salesdata.custcode + "' and partnercode='" + salesdata.partnercode + "'and custid ='" + salesdata.custid + "' and InvoiceNum ='" + salesdata.InvoiceNum + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                response = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                response = ex.Message;
                var invoicedetails = JsonConvert.SerializeObject(salesdata);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--GetPaymentDetails --json : " + invoicedetails);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--GetPaymentDetails : " + salesdata.partnercode + "--" + ex.Message);
            }
            return response;
        }

        public string InsertOutstandingUpdTable(PaymentDetail salesdata, SqlConnection myConnection)
        {
            string Insertstatus = string.Empty;
            try
            {
                string sql = "INSERT INTO OutStandingupd(custid,custcode,customername,address,telephone,mobile,partnercode,Invoice_Type,InvoiceNum,InvoiceDate,CreditDebit,OrderNumber,status,CreatedOn,CreatedBy,TotalAmount,TotalDiscount,NetAmount,BalanceAmount,BillAmount,GSTAmount,Credit_hold,Smancode,Area,Routeid,Remarks,Items,syncdatetime)VALUES (@custid,@custcode,@customername,@address,@telephone,@mobile,@partnercode,@Invoice_Type,@InvoiceNum,@InvoiceDate,@CreditDebit,@OrderNumber,@status,@CreatedOn,@CreatedBy,@TotalAmount,@TotalDiscount,@NetAmount,@BalanceAmount,@BillAmount,@GSTAmount,@Credit_hold,@Smancode,@Area,@Routeid,@Remarks,@Items,@syncdatetime)";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                cmd.Parameters.Add("@custid", SqlDbType.Int).Value = salesdata.custid;
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = salesdata.custcode;
                cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = salesdata.customername;
                cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = salesdata.address;
                cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = salesdata.telephone;
                cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = salesdata.mobile;
                cmd.Parameters.Add("@partnercode", SqlDbType.Int).Value = salesdata.partnercode;
                cmd.Parameters.Add("@Invoice_Type", SqlDbType.VarChar).Value = salesdata.Invoice_Type;
                cmd.Parameters.Add("@InvoiceNum", SqlDbType.VarChar).Value = salesdata.InvoiceNum;
                cmd.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = salesdata.InvoiceDate;
                cmd.Parameters.Add("@CreditDebit", SqlDbType.Int).Value = salesdata.CreditDebit;
                cmd.Parameters.Add("@OrderNumber", SqlDbType.VarChar).Value = salesdata.OrderNumber;
                cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = salesdata.status;
                cmd.Parameters.Add("@CreatedOn", SqlDbType.Date).Value = salesdata.CreatedOn;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = salesdata.CreatedBy;
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = salesdata.TotalAmount;
                cmd.Parameters.Add("@TotalDiscount", SqlDbType.Decimal).Value = salesdata.TotalDiscount;
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = salesdata.NetAmount;
                cmd.Parameters.Add("@BalanceAmount", SqlDbType.Decimal).Value = salesdata.BalanceAmount;
                cmd.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = salesdata.BillAmount;
                cmd.Parameters.Add("@GSTAmount", SqlDbType.Decimal).Value = salesdata.GSTAmount;
                cmd.Parameters.Add("@Credit_hold", SqlDbType.Int).Value = salesdata.Credit_hold;
                cmd.Parameters.Add("@Smancode", SqlDbType.Int).Value = salesdata.Smancode;
                cmd.Parameters.Add("@Area", SqlDbType.Int).Value = salesdata.Area;
                cmd.Parameters.Add("@Routeid", SqlDbType.Int).Value = salesdata.Routeid;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = salesdata.Remarks;
                cmd.Parameters.Add("@Items", SqlDbType.Int).Value = salesdata.Items;
                cmd.Parameters.Add("@syncdatetime", SqlDbType.Date).Value = salesdata.syncdatetime;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                Insertstatus = "success";
            }
            catch (Exception ex)
            {
                Insertstatus = ex.Message;
                var invoicedetails = JsonConvert.SerializeObject(salesdata);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB---InsertOutstandingUpdTable --json : " + invoicedetails);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB---InsertOutstandingUpdTable-Error:" + salesdata.partnercode + "--" + salesdata.custcode + "--" + ex.Message);
            }
            return Insertstatus;
        }

        public void DeleteOutstanding(PaymentDetail salesdata, SqlConnection myConnection)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Delete from Outstanding  where InvoiceNum='" + salesdata.InvoiceNum + "' and partnercode='" + salesdata.partnercode + "'", myConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var invoicedetails = JsonConvert.SerializeObject(salesdata);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB---DeleteOutsatnding --json : " + invoicedetails);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB---DeleteOutsatnding-Error:" + salesdata.partnercode + "--" + salesdata.custcode + "--" + ex.Message);
            }
        }
        public string InsertPaymentMethodToAzureDB(PaymentMethod PaymentDetail, SqlConnection myConnection)
        {
            string response = string.Empty;
            try
            {
                if (PaymentDetail != null)
                {
                    myConnection.Open();
                    
                                    try
                                    {
                                        string sql = "INSERT INTO payment (partnercode,customerid,customername,customercode,customeremail,customermobile,salesmancode,salesmanname,salesmanmobile,paymentmethod,paymentdate,chqno,chqdate,amount,submitdate,adjustinvoicenumber,bankname,neftnumber,syncdatetime)VALUES (@partnercode,@customerid,@customername,@customercode,@customeremail,@customermobile,@salesmancode,@salesmanname,@salesmanmobile,@paymentmethod,@paymentdate,@chqno,@chqdate,@amount,@submitdate,@adjustinvoicenumber,@bankname,@neftnumber,@syncdatetime)";
                                        SqlCommand cmd = new SqlCommand(sql, myConnection);
                                        cmd.Parameters.Add("@partnercode", SqlDbType.VarChar).Value = PaymentDetail.partnercode;
                                        cmd.Parameters.Add("@customerid", SqlDbType.VarChar).Value = PaymentDetail.customerid;
                                        cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = PaymentDetail.customername;
                                        cmd.Parameters.Add("@customercode", SqlDbType.VarChar).Value = PaymentDetail.customercode;
                                        cmd.Parameters.Add("@customeremail", SqlDbType.VarChar).Value = PaymentDetail.customeremail;
                                        cmd.Parameters.Add("@customermobile", SqlDbType.VarChar).Value = PaymentDetail.customermobile;
                                        cmd.Parameters.Add("@salesmancode", SqlDbType.VarChar).Value = PaymentDetail.salesmancode;
                                        cmd.Parameters.Add("@salesmanname", SqlDbType.VarChar).Value = PaymentDetail.salesmanname;
                                        cmd.Parameters.Add("@salesmanmobile", SqlDbType.VarChar).Value = PaymentDetail.salesmanmobile;
                                        cmd.Parameters.Add("@paymentmethod", SqlDbType.VarChar).Value = PaymentDetail.paymentmethod;
                                        cmd.Parameters.Add("@paymentdate", SqlDbType.Date).Value = PaymentDetail.paymentdate;
                                        cmd.Parameters.Add("@chqno", SqlDbType.VarChar).Value = PaymentDetail.chqno;
                                        cmd.Parameters.Add("@chqdate", SqlDbType.Date).Value = PaymentDetail.chqdate;
                                        cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = PaymentDetail.amount;
                                        cmd.Parameters.Add("@submitdate", SqlDbType.Date).Value = PaymentDetail.submitdate;
                                        cmd.Parameters.Add("@adjustinvoicenumber", SqlDbType.VarChar).Value = PaymentDetail.adjustinvoicenumber;
                                        cmd.Parameters.Add("@bankname", SqlDbType.VarChar).Value = PaymentDetail.bankname;
                                        cmd.Parameters.Add("@neftnumber", SqlDbType.VarChar).Value = PaymentDetail.neftnumber;
                                        cmd.Parameters.Add("@syncdatetime", SqlDbType.DateTime).Value = PaymentDetail.syncdatetime;
                                        
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        var invoicedetails = JsonConvert.SerializeObject(PaymentDetail);
                                        Trace.TraceInformation("InsertPaymentMethodToAzureDB--InsertPaymentDetailsToAzureDB--json : " + PaymentDetail);
                                        response = ex.Message;
                                        Trace.TraceInformation("syncPaymentDetails--InsertPaymentDetailsToAzureDB--InsertError : " + PaymentDetail.partnercode + "--" + PaymentDetail.customercode + "--" + ex.Message);
                                    }
                                }
                                

                           
                  
            }
            catch (Exception ex)
            {
                response = ex.Message;
                var invoicedetails = JsonConvert.SerializeObject(PaymentDetail);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--CompleteError--json : " + invoicedetails);
                Trace.TraceInformation("syncoutstandingDetails--InsertPaymentDetailsToAzureDB--CompleteError : " + ex.Message);
            }
            myConnection.Close();
            return response;
        }

    }
}
