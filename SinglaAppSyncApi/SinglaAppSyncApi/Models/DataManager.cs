using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SinglaAppSyncApi.Models
{
    public class DataManager
    {

        public async Task<List<SalesmanMaster>> GetsalesmanLogincheck(string Username, string Password)
        {
           List<SalesmanMaster> salesList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                try
                {
                    dbConnection.Open();
                    string dbItem = "select * from SalesmanMaster where username='" + Username + "'  and userpassword='" + Password + "'";
                    SqlDataAdapter adapter = new SqlDataAdapter(dbItem, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    salesList = ds.Tables[0].AsEnumerable().
                               Select(dataRow => new SalesmanMaster
                               {
                                   id = dataRow["id"].ToString(),
                                   partnercode = Convert.ToInt32(dataRow["partnercode"]),
                                   username = dataRow["username"].ToString(),
                                   userpassword = dataRow["userpassword"].ToString(),
                                   salesmanid = Convert.ToInt32(dataRow["salesmanid"]),
                                   salesmancode = dataRow["salesmancode"].ToString(),
                                   salesmanname = dataRow["salesmanname"].ToString(),
                                   mobile = dataRow["mobile"].ToString(),
                                   telephone = GetScheme(dataRow["telephone"].ToString()),
                                   salesmantarget = (float)Convert.ToDouble(dataRow["salesmantarget"].ToString()),
                                   salesmancommision = dataRow["salesmancommision"].ToString()
                               }).ToList();

                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetsalesLogin--GetsalesmanLogincheck()" + ex.Message);
                }
            }
            return salesList;
        }

        public async Task<List<CustomerMaster>> GetCustomerLogincheck(string Username, string Password)
        {
            List<CustomerMaster> customerList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                try
                {
                    dbConnection.Open();
                    string dbCust = "select (case when creditLockDays is null then 0 else creditLockDays end) as credit_LockDays,* from CustomerMaster where UserName='" + Username + "' and UserPassword='" + Password + "'";
                    SqlDataAdapter adapter = new SqlDataAdapter(dbCust, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    customerList = ds.Tables[0].AsEnumerable().
                               Select(dataRow => new CustomerMaster
                               {
                                   id = dataRow["id"].ToString(),
                                   city = dataRow["city"].ToString(),
                                   creditlimit = Convert.ToInt32(dataRow["creditlimit"]),
                                   CreditLockDays = Convert.ToInt32(dataRow["credit_LockDays"]),
                                   customeraddress = dataRow["customeraddress"].ToString(),
                                   customercode = dataRow["customercode"].ToString(),
                                   customeremail = dataRow["customeremail"].ToString(),
                                   customerid = Convert.ToInt32(dataRow["customerid"]),
                                   customername = dataRow["customername"].ToString(),
                                   customerstatus = dataRow["customerstatus"].ToString(),
                                   dlnumber = dataRow["dlnumber"].ToString(),
                                   gstnumber = dataRow["gstnumber"].ToString(),
                                   mobile = dataRow["mobile"].ToString(),
                                   outstandingbal = Convert.ToInt32(dataRow["outstandingbal"]),
                                   partnercode = Convert.ToInt32(dataRow["partnercode"]),
                                   pincode = Convert.ToInt32(dataRow["pincode"]),
                                   salesmanarea = Convert.ToInt32(dataRow["salesmanarea"]),
                                   salesmancode = Convert.ToInt32(dataRow["salesmancode"]),
                                   salesmanroute = Convert.ToInt32(dataRow["salesmanroute"]),
                                   telephone = dataRow["telephone"].ToString(),
                                   DLExpiry = DLExpiry(dataRow["DLExpiry"].ToString()),
                                   locked = Convert.ToInt32(dataRow["lock"]),
                                   lockreason = dataRow["lockreason"].ToString(),
                                   UserName = dataRow["UserName"].ToString(),
                                   UserPassword = dataRow["UserPassword"].ToString()
                               }).ToList();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetCustLogin--GetCustomerLogincheck()" + ex.Message);
                }
            }
            return customerList;
        }

        private static bool IsMatch(string itemName)
        {
            string Itemnamefirstlatter = itemName.ToString().FirstOrDefault().ToString();
            bool match = Regex.IsMatch(Itemnamefirstlatter, @"[^0-9a-zA-Z\._]");
            if (itemName.StartsWith("."))
            {
                match = true;
            }
            return match;
        }

        public string GetStockColor(float stock)
        {
            string stockcolor = string.Empty;
            if (stock > 11)
            {
                stockcolor = "Green";
            }
            else
            {
                if(stock ==0)
                {
                    stockcolor = "Red";
                }
                else
                {
                    stockcolor = "Blue";
                }
                
            }
            return stockcolor;
        }

        public string GetScheme(string scheme)
        {
            if (string.IsNullOrEmpty(scheme))
            {
                scheme = "N";
            }
            return scheme;
        }

        public async Task<List<ItemMaster>> GetProductdetails(int partnercode)
        {
            List<ItemMaster> itemList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                Trace.TraceInformation("GetProducts--GetProductdetails--Products Retrieving Started");
                try
                {
                    dbConnection.Open();
                    string dbItem = "select partnercode,itemid,itemcode,itemname,manufacturer,manufacturergroup," +
                        "packsize,ptr,mrp,itemstatus,boxsize,stock,Halfscheme,Scheme,creationdate,substitute,itemnamesearch from ItemMaster where itemstatus='Active'  and partnercode=" + partnercode + "";
                    SqlDataAdapter adapter = new SqlDataAdapter(dbItem, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    itemList = ds.Tables[0].AsEnumerable().
                        Where(dataRow => dataRow["manufacturer"].ToString() != "~ZZ-NOT IN USE(EAST WEST PHARMA (CYNOR)"
                        && !IsMatch(dataRow["itemname"].ToString())).
                               Select(dataRow => new ItemMaster
                               {
                                   Halfscheme = dataRow["Halfscheme"].ToString(),
                                   partnercode = Convert.ToInt32(dataRow["partnercode"]),
                                   itemcode = dataRow["itemcode"].ToString(),
                                   itemid = Convert.ToInt32(dataRow["itemid"]),
                                   itemname = dataRow["itemname"].ToString(),
                                   manufacturer = dataRow["manufacturer"].ToString(),
                                   manufacturergroup = dataRow["manufacturergroup"].ToString(),
                                   packsize = dataRow["packsize"].ToString(),
                                   Scheme = GetScheme(dataRow["Scheme"].ToString()),
                                   mrp = (float)Convert.ToDouble(dataRow["mrp"].ToString()),
                                   ptr = (float)Convert.ToDouble(dataRow["ptr"].ToString()),
                                   stock = (float)Convert.ToDouble(dataRow["stock"].ToString()),
                                   frmstockcolor = GetStockColor((float)Convert.ToDouble(dataRow["stock"].ToString())),
                                   boxsize = (float)Convert.ToDouble(dataRow["boxsize"].ToString()),
                                   creationdate = Convert.ToDateTime(Convert.ToDateTime(dataRow["creationdate"]).ToString("yyyy-MM-dd")),
                                   substitute = dataRow["substitute"].ToString(),
                                   itemnamesearch = dataRow["itemnamesearch"].ToString(),
                               }).ToList();

                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetProducts--GetProductdetails()" + ex.Message);
                }
            }
            return itemList;
        }

        public async Task<List<CustomerMaster>> GetAllCustomerdetails(string salesmanid, string partnercode)
        {
            List<CustomerMaster> customerList = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection dbConnection = new SqlConnection(con))
            {
                Trace.TraceInformation("GetProducts--GetProductdetails--Products Retrieving Started");
                try
                {
                    dbConnection.Open();
                    string dbCust = "select (case when creditLockDays is null then 0 else creditLockDays end) as credit_LockDays,* from CustomerMaster where customerstatus='Active'  and partnercode=" + partnercode + "";
                    SqlDataAdapter adapter = new SqlDataAdapter(dbCust, dbConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    customerList = ds.Tables[0].AsEnumerable().
                               Select(dataRow => new CustomerMaster
                               {
                                   id = dataRow["id"].ToString(),
                                   city = dataRow["city"].ToString(),
                                   creditlimit = Convert.ToInt32(dataRow["creditlimit"]),
                                   CreditLockDays = Convert.ToInt32(dataRow["credit_LockDays"]),
                                   customeraddress = dataRow["customeraddress"].ToString(),
                                   customercode = dataRow["customercode"].ToString(),
                                   customeremail = dataRow["customeremail"].ToString(),
                                   customerid = Convert.ToInt32(dataRow["customerid"]),
                                   customername = dataRow["customername"].ToString(),
                                   customerstatus = dataRow["customerstatus"].ToString(),
                                   dlnumber = dataRow["dlnumber"].ToString(),
                                   gstnumber = dataRow["gstnumber"].ToString(),
                                   mobile = dataRow["mobile"].ToString(),
                                   outstandingbal = Convert.ToInt32(dataRow["outstandingbal"]),
                                   partnercode = Convert.ToInt32(dataRow["partnercode"]),
                                   pincode = Convert.ToInt32(dataRow["pincode"]),
                                   salesmanarea = Convert.ToInt32(dataRow["salesmanarea"]),
                                   salesmancode = Convert.ToInt32(dataRow["salesmancode"]),
                                   salesmanroute = Convert.ToInt32(dataRow["salesmanroute"]),
                                   telephone = dataRow["telephone"].ToString(),
                                   DLExpiry = DLExpiry(dataRow["DLExpiry"].ToString()),
                                   locked = Convert.ToInt32(dataRow["lock"]),
                                   lockreason = dataRow["lockreason"].ToString(),
                                   UserName = dataRow["UserName"].ToString(),
                                   UserPassword = dataRow["UserPassword"].ToString()
                               }).ToList();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("GetAllCustomers--GetAllCustomerdetails()" + ex.Message);
                }
            }

            return customerList;
        }

        public DateTime DLExpiry(string dlexpiry)
        {
            DateTime dlexpirydt = Convert.ToDateTime(dlexpiry);
            return dlexpirydt;
        }
        public List<Orders> GetOrderhistorybycustomet(string Custmerid, string Fromdate, string Todate, string Partnercode)
        {
            List<Orders> orderDetails = new List<Orders>();
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection mysalesmanConnection = new SqlConnection(con))
            {
                try
                {
                    mysalesmanConnection.Open();
                    string dbSales = "select id,order_id,order_status,partnercode,order_date,NoOfItems,customer_id,customername,SequenceOrder_id,ordupldtime,ordtaketime,OrderGuid,Amt,UpdateStatus from Orders where customer_id='" + Custmerid + "'and order_date between '" + Fromdate + "' and '" + Todate + "'and partnercode='" + Partnercode + "'order by order_date desc ";
                    SqlCommand salesCmd = new SqlCommand(dbSales, mysalesmanConnection);
                    var salesReader = salesCmd.ExecuteReader();
                    while (salesReader.Read())
                    {
                        Orders Order_Details = new Orders();
                        Order_Details.order_id = salesReader["order_id"].ToString();
                        Order_Details.partnercode = salesReader["partnercode"].ToString();
                        Order_Details.order_status = salesReader["order_status"].ToString();
                        Order_Details.order_date = Convert.ToDateTime(salesReader["order_date"]).ToString("yyyy-MM-dd");
                        Order_Details.NoOfItems = Convert.ToInt32(salesReader["NoOfItems"]);
                        Order_Details.customer_id = salesReader["customer_id"].ToString();
                        Order_Details.customername = salesReader["customername"].ToString();
                        Order_Details.SequenceOrder_id = Convert.ToInt32(salesReader["SequenceOrder_id"]);
                        //if (string.IsNullOrEmpty(Order_Details.ordupldtime))
                        //{
                        //    Order_Details.ordupldtime = "";
                        //}
                        //else
                        //{
                        Order_Details.ordupldtime = salesReader["ordupldtime"].ToString();
                        //}
                        Order_Details.ordtaketime = salesReader["ordtaketime"].ToString();
                        Order_Details.OrderGuid = salesReader["OrderGuid"].ToString();
                        Order_Details.Amt = Convert.ToDouble(salesReader["Amt"]);
                        Order_Details.UpdateStatus = salesReader["UpdateStatus"].ToString();
                        Order_Details.orderItemList = GetCompliteordersitems(salesReader["order_id"].ToString());
                        orderDetails.Add(Order_Details);
                    }
                    salesReader.Close();
                    mysalesmanConnection.Close();
                }
                catch (Exception ex)
                {
                }
            }
            return orderDetails;
        }

        public List<Orders> GetOrderhistorybySman(string Smanid, string Fromdate, string Todate, string Partnercode)
        {
            List<Orders> orderDetails = new List<Orders>();
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection mysalesmanConnection = new SqlConnection(con))
            {
                try
                {
                    mysalesmanConnection.Open();
                    string dbSales = "select id,order_id,order_status,partnercode,order_date,NoOfItems,customer_id,customername,SequenceOrder_id,ordupldtime,ordtaketime,OrderGuid,Amt,UpdateStatus from Orders where Sman='" + Smanid + "'and order_date between '" + Fromdate + "' and '" + Todate + "'and partnercode='" + Partnercode + "'order by order_date desc";
                    SqlCommand salesCmd = new SqlCommand(dbSales, mysalesmanConnection);
                    var salesReader = salesCmd.ExecuteReader();
                    while (salesReader.Read())
                    {
                        Orders Order_Details = new Orders();
                        Order_Details.order_id = salesReader["order_id"].ToString();
                        Order_Details.partnercode = salesReader["partnercode"].ToString();
                        Order_Details.order_status = salesReader["order_status"].ToString();
                        Order_Details.order_date = Convert.ToDateTime(salesReader["order_date"]).ToString("yyyy-MM-dd");
                        Order_Details.NoOfItems = Convert.ToInt32(salesReader["NoOfItems"]);
                        Order_Details.customer_id = salesReader["customer_id"].ToString();
                        Order_Details.customername = salesReader["customername"].ToString();
                        Order_Details.SequenceOrder_id = Convert.ToInt32(salesReader["SequenceOrder_id"]);
                        //if (string.IsNullOrEmpty(Order_Details.ordupldtime))
                        //{
                        //    Order_Details.ordupldtime = "";
                        //}
                        //else
                        //{
                        Order_Details.ordupldtime = salesReader["ordupldtime"].ToString();
                        //}
                        Order_Details.ordtaketime = salesReader["ordtaketime"].ToString();
                        Order_Details.OrderGuid = salesReader["OrderGuid"].ToString();
                        Order_Details.Amt = Convert.ToDouble(salesReader["Amt"]);
                        Order_Details.UpdateStatus = salesReader["UpdateStatus"].ToString();
                        Order_Details.orderItemList = GetCompliteordersitems(salesReader["order_id"].ToString());
                        orderDetails.Add(Order_Details);
                    }
                    salesReader.Close();
                    mysalesmanConnection.Close();
                }
                catch (Exception ex)
                {
                }
            }
            return orderDetails;
        }
        public List<orderitems> GetCompliteordersitems(string orderid)
        {
            List<orderitems> orderDetails = new List<orderitems>();
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection mysalesmanConnection = new SqlConnection(con))
            {
                try
                {
                    mysalesmanConnection.Open();
                    string dbSales = "select pack,itemname,orderqty,scm1,scm2,Rate,MRP from orderitems where order_id ='" + orderid + "'";
                    SqlCommand salesCmd = new SqlCommand(dbSales, mysalesmanConnection);
                    var salesReader = salesCmd.ExecuteReader();
                    while (salesReader.Read())
                    {
                        orderitems Order_Details = new orderitems();
                        Order_Details.pack = salesReader["pack"].ToString();
                        Order_Details.itemname = salesReader["itemname"].ToString();
                        Order_Details.orderqty = salesReader["orderqty"].ToString();
                        Order_Details.scm1 = salesReader["scm1"].ToString();
                        Order_Details.scm2 = salesReader["scm2"].ToString();
                        Order_Details.Rate = salesReader["Rate"].ToString();
                        Order_Details.MRP = salesReader["MRP"].ToString();
                        orderDetails.Add(Order_Details);
                    }
                    salesReader.Close();
                    mysalesmanConnection.Close();
                }
                catch (Exception ex)
                {
                }
            }
            return orderDetails;
        }
        public async Task<List<Orderstatus>> Getupdatedstatu(string orderid)
        {

            List<Orderstatus> orderslist = null;
            var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
            using (SqlConnection myCustomerConnection = new SqlConnection(con))
            {
                Trace.TraceInformation("Getupdatedstatus--Getupdatedstatusdetails--status Retrieving Started");
                try
                {
                    myCustomerConnection.Open();
                    string dbCust = "select UpdateStatus,Invoice_date,Invoice_No,order_status,order_id From Orders where order_id in (" + orderid + ")";
                    SqlDataAdapter adapter = new SqlDataAdapter(dbCust, myCustomerConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    orderslist = ds.Tables[0].AsEnumerable().
                   Select(dataRow => new Orderstatus
                   {
                       UpdateStatus = dataRow["UpdateStatus"].ToString(),
                       Invoice_No = dataRow["Invoice_No"].ToString(),
                       order_status = dataRow["order_status"].ToString(),
                       order_id = dataRow["order_id"].ToString(),
                       Invoice_date = dataRow["Invoice_date"].ToString()

                   }).ToList();
                    myCustomerConnection.Close();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("Getupdatedstatus--Getupdatedstatusdetails()" + ex.Message);
                }
            }
            return orderslist;
        }

    }
}