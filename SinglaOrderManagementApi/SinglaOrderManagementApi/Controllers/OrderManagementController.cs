using SinglaOrderManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static SinglaOrderManagementApi.Models.HelpMaster;

namespace SinglaOrderManagementApi.Controllers
{
    public class OrderManagementController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> InsertOrdersLocalToAzureDB(List<Orders> orderList)
        {
            List<updateOrders> updateOrderList = new List<updateOrders>();
            try
            {
                Trace.TraceInformation("------>@< Order Insertion Started >@<------");
                Trace.TraceInformation("Total Orders Count: " + orderList.Count);
                using (SqlConnection azureConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterDB"].ToString()))
                {
                    azureConnection.Open();

                    Trace.TraceInformation("AzureConnection Open");
                    try
                    {
                        DataManager dm = new DataManager();
                        foreach (Orders order in orderList)
                        {
                            updateOrders updateOrder = new updateOrders();
                            string OrderId = dm.CheckorderInAzure(order, azureConnection);
                            if (string.IsNullOrEmpty(OrderId))
                            {
                                Trace.TraceInformation("SManID-CustCode-CustName : " + order.Sman + "-" + order.customercode + "-" + order.customername);
                                order.order_id = dm.CreateOrder_Id(order, azureConnection);
                                Trace.TraceInformation("Generated Order_Id:" + order.order_id);
                                foreach (orderitems orderItem in order.orderItemList)
                                {
                                    orderItem.order_id = order.order_id;
                                    Trace.TraceInformation("Order Item: " + orderItem.itemname + order.SequenceOrder_id);
                                }
                                dm.BulkInsertOrderItems(order.orderItemList.ToList(), azureConnection, order.order_id);

                                dm.InsertOrdersLocalServerToAzureDB(order, azureConnection);
                                updateOrder.OrderGuid = order.OrderGuid;
                                updateOrder.order_id = order.order_id;
                                updateOrder.ordupldtime = order.ordupldtime;
                                updateOrderList.Add(updateOrder);
                                Trace.TraceInformation("Order_Id " + order.order_id);
                            }
                            else
                            {
                                updateOrder.OrderGuid = order.OrderGuid;
                                updateOrder.order_id = OrderId;
                                updateOrder.ordupldtime = order.ordupldtime;
                                updateOrderList.Add(updateOrder);
                                Trace.TraceInformation("OrderId Already Generated For This Order :" + order.order_id);
                            }
                        }
                        azureConnection.Close();
                        Trace.TraceInformation("AzureConnection Closed");
                    }
                    catch (Exception ex)
                    {
                        azureConnection.Close();
                        Trace.TraceInformation("Order Insert error: " + ex.Message);
                    }
                }
                orderList.Clear();
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Order Complete error: " + ex.Message);
            }
            Trace.TraceInformation("Order Complete json: " + Ok(updateOrderList));
            Trace.TraceInformation("------>@< Order Insert Compleated >@<------");
            return Ok(updateOrderList);
        }
    }
}
