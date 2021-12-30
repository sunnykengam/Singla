using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using static SinglaOrderManagementApi.Models.HelpMaster;

namespace SinglaOrderManagementApi.Models
{
    public class DataManager
    {
        public string CreateOrder_Id(Orders itemorder, SqlConnection azureConnection)
        {
            string compOrdID = string.Empty;
            int endorderID = 1;
            int SequenceOrder_id = GetSequenceOrder_idByNewOrders(azureConnection);
            if (SequenceOrder_id != 0)
            {
                string startorderID = itemorder.Temporder_id.Substring(0, 9);
                if (itemorder.INVTYPE != "Y")
                    startorderID = startorderID.Replace("SM", "SMB");
                else 
                    startorderID = startorderID.Replace("SM", "SMC");

                endorderID = SequenceOrder_id;
                compOrdID = startorderID + endorderID;
                itemorder.SequenceOrder_id = endorderID;
            }
            else
            {
                compOrdID = itemorder.Temporder_id;
                itemorder.SequenceOrder_id = 1;
            }
            return compOrdID;
        }

        public async  void BulkInsertOrderItems(List<orderitems> orderitemList, SqlConnection azureConnection, string order_id)
        {
            try
            {
                var pickingDetails = orderitemList.ToList();
                DataTable dt = ToDataTable(pickingDetails);
                Trace.WriteLine("OrderItemsBulkInsert - No of items: " + orderitemList.Count);
                using (var bulkCopy = new SqlBulkCopy(azureConnection))
                {
                    Trace.WriteLine("OrderItemsBulkInsert - BulkCopy started");
                    bulkCopy.BatchSize = 990;
                    bulkCopy.DestinationTableName = "orderitems";
                    bulkCopy.ColumnMappings.Clear();
                    bulkCopy.ColumnMappings.Add("customer_id", "customer_id");
                    bulkCopy.ColumnMappings.Add("itemcode", "itemcode");
                    bulkCopy.ColumnMappings.Add("itemaltercode", "itemaltercode");
                    bulkCopy.ColumnMappings.Add("itemname", "itemname");
                    bulkCopy.ColumnMappings.Add("pack", "pack");
                    bulkCopy.ColumnMappings.Add("company", "company");
                    bulkCopy.ColumnMappings.Add("orderqty", "orderqty");
                    bulkCopy.ColumnMappings.Add("orderfqty", "orderfqty");
                    bulkCopy.ColumnMappings.Add("order_id", "order_id");
                    bulkCopy.ColumnMappings.Add("order_date", "order_date");
                    bulkCopy.ColumnMappings.Add("QtyRecd", "QtyRecd");
                    bulkCopy.ColumnMappings.Add("FQtyRecd", "FQtyRecd");
                    bulkCopy.ColumnMappings.Add("Tag", "Tag");
                    bulkCopy.ColumnMappings.Add("Invnum", "Invoice_No");
                    bulkCopy.ColumnMappings.Add("Invoice_date", "Invoice_date");
                    bulkCopy.ColumnMappings.Add("PorderDis", "PorderDis");
                    bulkCopy.ColumnMappings.Add("Rate", "Rate");
                    bulkCopy.ColumnMappings.Add("MRP", "MRP");
                    bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                    bulkCopy.ColumnMappings.Add("scm1", "scm1");
                    bulkCopy.ColumnMappings.Add("scm2", "scm2");
                    bulkCopy.ColumnMappings.Add("OrderAmount", "OrderAmount");
                    bulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                    bulkCopy.ColumnMappings.Add("partnercode", "partnercode");
                    bulkCopy.ColumnMappings.Add("OrderGuid", "OrderGuid");
                    //bulkCopy.ColumnMappings.Add("INVTYPE", "INVTYPE");
                    bulkCopy.WriteToServer(dt);
                    Trace.WriteLine("BulkInsert---->@< OrderItems Inserted Succesfully >@<----");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("InsertOrderItems - Bulk copy error: " + ex.Message);
            }
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

        public async void InsertOrdersLocalServerToAzureDB(Orders OrderDetail, SqlConnection azureConnection)
        {
            try
            {
                if (OrderDetail != null)
                {
                    Trace.WriteLine("OrdersInsertToAzureDB- Started");

                    string sql = "INSERT INTO Orders(partnercode,customer_id,customername,SequenceOrder_id,order_id,"
                                                        + "order_date,Sman,Area,NoOfItems,Amt,ORDERFROM,appversion,ordtaketime,ordupldtime,OrderGuid,UpdateStatus,INVTYPE)"
                                                        + "VALUES (@partnercode,@customer_id,@customername,@SequenceOrder_id,@order_id,@order_date,@Sman,@Area,@NoOfItems,@Amt,@ORDERFROM,@appversion,@ordtaketime,@ordupldtime,@OrderGuid,@UpdateStatus,@INVTYPE)";
                    SqlCommand cmd = new SqlCommand(sql, azureConnection);
                    if (azureConnection == null)
                        Trace.WriteLine("OrdersInsertToAzureDB----> AzureConnection Null<---");
                    //if (transaction == null)
                    //    Trace.WriteLine("OrdersInsertToAzureDB----> Transaction Null<---");

                    cmd.Parameters.Add("@partnercode", SqlDbType.VarChar).Value = OrderDetail.partnercode;
                    cmd.Parameters.Add("@customer_id", SqlDbType.VarChar).Value = OrderDetail.customer_id;
                    cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = OrderDetail.customername;
                    cmd.Parameters.Add("@SequenceOrder_id", SqlDbType.Int).Value = OrderDetail.SequenceOrder_id;
                    cmd.Parameters.Add("@order_id", SqlDbType.VarChar).Value = OrderDetail.order_id;
                    cmd.Parameters.Add("@order_date", SqlDbType.Date).Value = OrderDetail.order_date;
                    cmd.Parameters.Add("@Sman", SqlDbType.Float).Value = OrderDetail.Sman;
                    cmd.Parameters.Add("@Area", SqlDbType.VarChar).Value = OrderDetail.Area;
                    cmd.Parameters.Add("@NoOfItems", SqlDbType.Float).Value = OrderDetail.NoOfItems;
                    cmd.Parameters.Add("@Amt", SqlDbType.Float).Value = OrderDetail.Amt.ToString();
                    cmd.Parameters.Add("@ORDERFROM", SqlDbType.VarChar).Value = OrderDetail.ORDERFROM.ToString();
                    cmd.Parameters.Add("@appversion", SqlDbType.VarChar).Value = OrderDetail.appversion.ToString();
                    cmd.Parameters.Add("@ordtaketime", SqlDbType.VarChar).Value = OrderDetail.ordtaketime;
                    cmd.Parameters.Add("@ordupldtime", SqlDbType.VarChar).Value = OrderDetail.ordupldtime;
                    cmd.Parameters.Add("@OrderGuid", SqlDbType.VarChar).Value = OrderDetail.OrderGuid;
                    cmd.Parameters.Add("@UpdateStatus", SqlDbType.VarChar).Value = "1";
                    cmd.Parameters.Add("@INVTYPE", SqlDbType.VarChar).Value = OrderDetail.INVTYPE;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Trace.TraceInformation("InsertOrders---->@< Order Insert Succefully >@<---- Order_id :  " + OrderDetail.order_id);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("InsertOrdersToAzureDB copy error: " + ex.Message);
            }
        }

        public string CheckorderInAzure(Orders OrderDetail, SqlConnection azureConnection)
        {
            string response = string.Empty;
            if (OrderDetail != null)
            {
                try
                {
                    string sql = "select order_id from orders where OrderGuid='" + OrderDetail.OrderGuid + "'";
                    SqlCommand cmd = new SqlCommand(sql, azureConnection);
                    response = (string)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("Order Insert CheckorderInAzure(): " + ex.Message);
                }
            }
            return response;
        }

        public int GetSequenceOrder_idByNewOrders(SqlConnection azureConnection)
        {
            int Maxid = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("GetLatestSequence", azureConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();

                param = cmd.Parameters.Add("@SeqName", SqlDbType.NVarChar);
                param.Direction = ParameterDirection.Input;
                param.Value = "orders";

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                Maxid = (int)returnParameter.Value;

                Trace.TraceInformation("******Update Maxnumber Succefully  : " + Maxid);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("******Update Maxnumber Faild  : " + Maxid);
            }
            return Maxid;
        }
    }
}
