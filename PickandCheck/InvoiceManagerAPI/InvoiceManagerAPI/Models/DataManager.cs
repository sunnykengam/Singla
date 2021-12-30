using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InvoiceManagerAPI.Models
{
    public static class DataManager
    {
        //public static List<InvoiceDetail> GetCheckingDetails(string CustCode, DateTime InvoiceDate, string Block)
        //{
        //    List<InvoiceDetail> orderDetails = new List<InvoiceDetail>();
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    System.Net.ServicePointManager.Expect100Continue = false;
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        myOrderConnection.Open();

        //        try
        //        {

        //            string dbOrder = "Select * from App_PickList where CustCode='" + CustCode + "' and Invdt='" + InvoiceDate.ToString("yyyy-MM-dd") + "' and [Block]='" + Block + "' order by Picked desc";
        //            SqlCommand orderCmd = new SqlCommand(dbOrder, myOrderConnection);
        //            var orderReader = orderCmd.ExecuteReader();
        //            while (orderReader.Read())
        //            {
        //                InvoiceDetail detail = new InvoiceDetail();
        //                detail.CustCode = orderReader["CustCode"].ToString();
        //                detail.CustomerName = orderReader["CustomerName"].ToString();
        //                detail.Invdt = Convert.ToDateTime(orderReader["Invdt"]);
        //                detail.Qty = Convert.ToInt32(orderReader["Qty"]);
        //                detail.CustId = Convert.ToInt32(orderReader["CustId"]);
        //                detail.Itemname = orderReader["Itemname"].ToString();
        //                detail.Pack = orderReader["Pack"].ToString();
        //                detail.Location = orderReader["Location"].ToString();
        //                detail.Status = orderReader["Status"].ToString();
        //                detail.NewQty = Convert.ToInt32(orderReader["NewQty"]);
        //                detail.Picked = orderReader["Picked"].ToString();
        //                detail.Invnum = orderReader["Invnum"].ToString();
        //                detail.Itemid = Convert.ToInt32(orderReader["Itemid"]);
        //                detail.Batchid = Convert.ToInt32(orderReader["Batchid"]);
        //                detail.Batch = orderReader["Batch"].ToString();
        //                detail.expiry = orderReader["expiry"].ToString();
        //                detail.mrp = Convert.ToInt32(orderReader["mrp"]);
        //               // detail.Chicked = orderReader["Chicked"].ToString();
        //                detail.itemcode = orderReader["itemcode"].ToString();
        //                var /Batchids = GetBatchIdsByItemId(detail.Itemid);
        //                int d = Batchids.Count();
        //                var batids = Batchids;
        //                detail.BatchList = new List<PickerList>();
        //                foreach (var item in Batchids.ToList())
        //                {
        //                    PickerList newlist = new PickerList();
        //                    var isAdded = detail.BatchList.Where(j => j.batch == item.batch).FirstOrDefault();
        //                    if (isAdded == null)
        //                    {
        //                        var mulbatch = batids.Where(k => k.batch == item.batch).ToList();
        //                        if (mulbatch.Count == 1)
        //                        {
        //                            newlist.PickerDisply = "Batch: " + item.batch + "   Qty: " + (int)item.qty + "   MRP: " + (int)item.MRP;
        //                            newlist.batch = item.batch;
        //                            detail.BatchList.Add(newlist);
        //                        }
        //                        else
        //                        {
        //                            foreach (BatchIdsByItemId item1 in mulbatch)
        //                            {
        //                                item.qty = item.qty + item1.qty;
        //                            }
        //                            newlist.PickerDisply = "Batch: " + item.batch + "   Qty: " + (int)item.qty + "   MRP: " + (int)item.MRP;
        //                            newlist.batch = item.batch;
        //                            detail.BatchList.Add(newlist);
        //                        }
        //                    }
        //                }
        //                if (detail.BatchList.Count() == 1)
        //                    detail.IsEnabled = false;

        //                orderDetails.Add(detail);
        //            }
        //            orderReader.Close();
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    return orderDetails;
        //}
        //public static List<BatchIdsByItemId> GetBatchIdsByItemId(int itemId)
        //{
        //    List<BatchIdsByItemId> BatchIdDetails = new List<BatchIdsByItemId>();
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {
        //            DateTime todate = DateTime.UtcNow.Date;
        //            DateTime FROMdATE = DateTime.UtcNow.Date.AddDays(-60);
        //            myOrderConnection.Open();
        //            string dbOrderitems = "select i.code as itemid,i.barcode as itemcode,i.name as itemname,i.pack as pack,f.batch as batch,f.expiry as expiry,f.bqty as qty,f.mrp as MRP,f.psrlno as batchid from item i, fifo f where f.Itemc = i.code  and f.hold <> 'H' and i.code ='" + itemId + "' and f.vdt between '" + FROMdATE.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "' order by f.vdt desc";
        //            SqlCommand orderitemsCmd = new SqlCommand(dbOrderitems, myOrderConnection);
        //            SqlDataReader BatchitemsReader = orderitemsCmd.ExecuteReader();
        //            while (BatchitemsReader.Read())
        //            {
        //                BatchIdsByItemId BatchId = new BatchIdsByItemId();
        //                BatchId.itemid = Convert.ToInt32(BatchitemsReader["itemid"].ToString());
        //                BatchId.itemcode = BatchitemsReader["itemcode"].ToString();
        //                BatchId.itemname = BatchitemsReader["itemname"].ToString();
        //                BatchId.pack = BatchitemsReader["pack"].ToString();
        //                BatchId.batch = BatchitemsReader["batch"].ToString();
        //                BatchId.expiry = BatchitemsReader["expiry"].ToString();
        //                BatchId.qty = Convert.ToDecimal(BatchitemsReader["qty"]);
        //                BatchId.MRP = Convert.ToDouble(BatchitemsReader["MRP"]);
        //                BatchId.batchid = Convert.ToInt32(BatchitemsReader["batchid"]);
        //                BatchIdDetails.Add(BatchId);
        //            }
        //            BatchitemsReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return BatchIdDetails;
        //}
        //public static InvoiceStatus GetInvoiceStatus(int custcode, string invdate)
        //{
        //    InvoiceStatus invstatus = new InvoiceStatus();

        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {


        //            myOrderConnection.Open();
        //            string dbOrderitems = "select count(*) as TotalBlocks,sum(case when Status is null then 0 else 1 end) as pickedlines,sum(case when Checked is null then 0 else 1 end) as checkedlines from AppPickingDetails where custcode ='" + custcode + "' and invdt ='" + invdate + "'";
        //            SqlCommand orderitemsCmd = new SqlCommand(dbOrderitems, myOrderConnection);
        //            SqlDataReader invReader = orderitemsCmd.ExecuteReader();
        //            if (invReader.Read())
        //            {

        //                invstatus.TotalBlocks = Convert.ToInt32(invReader["TotalBlocks"].ToString());
        //                invstatus.pickedlines = Convert.ToInt32(invReader["pickedlines"].ToString());
        //                invstatus.checkedlines = Convert.ToInt32(invReader["checkedlines"].ToString());

        //            }
        //            invReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return invstatus;
        //}
        //public static List<CustomerInvoiceSummary> GetCustomerInvoiceSummary(int custcode, string invdate)
        //{
        //    List<CustomerInvoiceSummary> BatchIdDetails = new List<CustomerInvoiceSummary>();
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {
                    
        //            myOrderConnection.Open();
        //            string dbOrderitems = "SELECT invdt as Invdate,Checkername as checkername,route as routename,custcode as custcode,customername as custname,COUNT(Itemid) as Totalines,sum(case when Picked is null then 0 else 1 end) as pickedlines,sum(case when chicked is null then 0 else 1 end) as checkedlines,(COUNT(Itemid)) - (sum(case when chicked is null then 0 else 1 end)) as pendinglines,sum(case when isbatch is null then 0 else 1 end) as batchchanges,sum(case when isqty is null then 0 else 1 end) as qtychangelines,sum(case when isdelete is null then 0 else 1 end) as nillines FROM App_PickList where Invdt ='" + invdate + "' and custcode ='" + custcode + "' group by Checkername,route,custcode,customername,invdt";
        //            SqlCommand orderitemsCmd = new SqlCommand(dbOrderitems, myOrderConnection);
        //            SqlDataReader BatchitemsReader = orderitemsCmd.ExecuteReader();
        //            while (BatchitemsReader.Read())
        //            {
        //                CustomerInvoiceSummary InvoiceSummary = new CustomerInvoiceSummary();
        //                InvoiceSummary.Invdate = Convert.ToDateTime(BatchitemsReader["Invdate"].ToString());
        //                InvoiceSummary.checkername = BatchitemsReader["checkername"].ToString();
        //                InvoiceSummary.routename = BatchitemsReader["routename"].ToString();
        //                InvoiceSummary.custcode = BatchitemsReader["custcode"].ToString();
        //                InvoiceSummary.custname = BatchitemsReader["custname"].ToString();
        //                InvoiceSummary.Totalines = Convert.ToInt32(BatchitemsReader["Totalines"].ToString());
        //                InvoiceSummary.pickedlines = Convert.ToInt32(BatchitemsReader["pickedlines"].ToString());
        //                InvoiceSummary.checkedlines = Convert.ToInt32(BatchitemsReader["MRP"].ToString());
        //                InvoiceSummary.pendinglines = Convert.ToInt32(BatchitemsReader["batchid"].ToString());
        //                InvoiceSummary.batchchanges = Convert.ToInt32(BatchitemsReader["batchchanges"].ToString());
        //                InvoiceSummary.qtychangelines = Convert.ToInt32(BatchitemsReader["qtychangelines"].ToString());
        //                InvoiceSummary.nillines = Convert.ToInt32(BatchitemsReader["nillines"].ToString());
        //                BatchIdDetails.Add(InvoiceSummary);
        //            }
        //            BatchitemsReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return BatchIdDetails;
        //}
        //public static List<PickedDetail> GetPickedDetails(int CheckerID, DateTime InvDate)
        //{
        //    List<PickedDetail> BatchIdDetails = new List<PickedDetail>();
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {
        //            DateTime todate = DateTime.UtcNow.Date;
        //            DateTime FROMdATE = DateTime.UtcNow.Date.AddDays(-60);
        //            myOrderConnection.Open();
        //            string dbOrderitems = "select m.code as checkerid,m.name as checkername,a.code as custid,a.altercode as custcode,a.name as custname,p.routeid,p.route,p.Block,p.Status as Picked,p.Checked as Checked,p.pickername,p.basketNo,p.phone from Acm a, master m , AppPickingDetails p where a.sman = m.code and a.code = p.custid and a.Slcd = 'CL' and a.code > 0 and a.status <> '*' and m.code = '" + CheckerID + "' and p.Status = 'y' and a.code in (select acno from Salepurchase1 where trntype = 'SB' and status<>'C' and p.invdt = '" + InvDate.ToString("yyyy-MM-dd") + "') order by p.route asc, a.name asc, p.Block asc";
        //            SqlCommand orderitemsCmd = new SqlCommand(dbOrderitems, myOrderConnection);
        //            SqlDataReader BatchitemsReader = orderitemsCmd.ExecuteReader();
        //            while (BatchitemsReader.Read())
        //            {
        //                PickedDetail PickedDetails = new PickedDetail();
        //                PickedDetails.checkerid = Convert.ToInt32(BatchitemsReader["checkerid"].ToString());
        //                PickedDetails.checkername = BatchitemsReader["checkername"].ToString();
        //                PickedDetails.custid = Convert.ToInt32(BatchitemsReader["custid"].ToString());
        //                PickedDetails.custcode = BatchitemsReader["custcode"].ToString();
        //                PickedDetails.custname = BatchitemsReader["custname"].ToString();
        //                PickedDetails.routeid = BatchitemsReader["routeid"].ToString();
        //                PickedDetails.route = BatchitemsReader["route"].ToString();
        //                PickedDetails.Block = BatchitemsReader["Block"].ToString().Trim();
        //                PickedDetails.Picked = BatchitemsReader["Picked"].ToString().Trim();
        //                PickedDetails.Checked = BatchitemsReader["Checked"].ToString().Trim();
        //                PickedDetails.pickername = BatchitemsReader["pickername"].ToString();
        //                PickedDetails.basketNo = BatchitemsReader["basketNo"].ToString();
        //                PickedDetails.phone = BatchitemsReader["phone"].ToString();
        //                BatchIdDetails.Add(PickedDetails);
        //            }
        //            BatchitemsReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return BatchIdDetails;
        //}
        //public static List<UserMaster> GetUserDetail(string UserName)
        //{
        //    List<UserMaster> BatchIdDetails = new List<UserMaster>();
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {
                   
        //            myOrderConnection.Open();
        //            string dbOrderitems = "select distinct m.code as checkerid,m.name as checkername from Acm a , master m where a.sman=m.code and a.Slcd='CL' and a.code>0 and a.status<>'*' and a.code in (select acno from Salepurchase1 where trntype = 'SB' and status<>'C' ) and m.code = '" + UserName + "'";
        //            SqlCommand orderitemsCmd = new SqlCommand(dbOrderitems, myOrderConnection);
        //            SqlDataReader BatchitemsReader = orderitemsCmd.ExecuteReader();
        //            while (BatchitemsReader.Read())
        //            {
        //                UserMaster User = new UserMaster();
        //                User.checkerid = Convert.ToInt32(BatchitemsReader["checkerid"].ToString());
        //                User.checkername = BatchitemsReader["checkername"].ToString();
        //                BatchIdDetails.Add(User);
        //            }
        //            BatchitemsReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return BatchIdDetails;
        //}
        //public static string InsertInvoiceSummary(InvoiceSummary objDetail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "Insert into [dbo].[App_InvoiceSummary](Invdate,checkername,routename,custcode,custname,Totalines,pickedlines,checkedlines,pendinglines,Noofbatchchanges,Noofqtychangelines,Noofnillines,Noofboxes,Noofpackets) values(@Invdate, @Checkername, @routename, @custcode, @customername, @Totalines, @pickedlines, @checkedlines, @pendinglines, @Noofbatchchanges, @Noofqtychangelines, @Noofnillines, @Noofboxes, @Noofpackets)";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@Invdate", SqlDbType.DateTime).Value = objDetail.Invdate;
        //            cmd.Parameters.Add("@Checkername", SqlDbType.VarChar).Value = objDetail.checkername;
        //            cmd.Parameters.Add("@routename", SqlDbType.Float).Value = objDetail.routename;
        //            cmd.Parameters.Add("@custcode", SqlDbType.Float).Value = objDetail.custcode;
        //            cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = objDetail.custname;
        //            cmd.Parameters.Add("@Totalines", SqlDbType.VarChar).Value = objDetail.Totalines;
        //            cmd.Parameters.Add("@pickedlines", SqlDbType.Int).Value = objDetail.pickedlines;
        //            cmd.Parameters.Add("@checkedlines", SqlDbType.Int).Value = objDetail.checkedlines;
        //            cmd.Parameters.Add("@pendinglines", SqlDbType.Int).Value = objDetail.pendinglines;
        //            cmd.Parameters.Add("@Noofbatchchanges", SqlDbType.Int).Value = objDetail.batchchanges;
        //            cmd.Parameters.Add("@Noofqtychangelines", SqlDbType.Int).Value = objDetail.qtychangelines;
        //            cmd.Parameters.Add("@Noofnillines", SqlDbType.Int).Value = objDetail.nillines;
        //            cmd.Parameters.Add("@Noofboxes", SqlDbType.Int).Value = objDetail.Noofboxes;
        //            cmd.Parameters.Add("@Noofpackets", SqlDbType.Int).Value = objDetail.Noofpackets;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string InsertCheckerChanges(CheckingDetail objDetail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string invdate = Convert.ToDateTime(objDetail.Invdt).ToString("yyyy-MM-dd");
        //            string Chakerdate = Convert.ToDateTime(objDetail.ChickedDate).ToString("yyyy-MM-dd");
        //            string sql = "DECLARE @custcode1 INT; if not exists((select custcode from AppInvoiceChanges where Invdate = @Invdate and Invnum = @Invnum and custcode = @custcode and itemid = @itemid))Insert into AppInvoiceChanges(Invdate, Invnum, custid, custcode, customername, itemid, itemcode, itemname, Remarks, oldqty, newqty, oldbatch, newbatch, Checkername, changedatetime, Description, IsDelete)values(@Invdate, @Invnum, @custid, @custcode, @customername, @itemid, @itemcode, @itemname, @Remarks, @oldqty, @newqty, @oldbatch, @newbatch, @Checkername, @changedatetime, @Description, @IsDelete)";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@Invdate", SqlDbType.DateTime).Value =Convert.ToDateTime(invdate);
        //            cmd.Parameters.Add("@Invnum", SqlDbType.Int).Value = objDetail.Invnum;
        //            cmd.Parameters.Add("@custid", SqlDbType.Int).Value = objDetail.CustId;
        //            cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = objDetail.CustCode;
        //            cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = objDetail.CustomerName;
        //            cmd.Parameters.Add("@itemid", SqlDbType.VarChar).Value = objDetail.Itemid;
        //            cmd.Parameters.Add("@itemcode", SqlDbType.Int).Value = objDetail.Itemcode;
        //            cmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = objDetail.Itemname;
        //            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = objDetail.Remarks;
        //            cmd.Parameters.Add("@oldqty", SqlDbType.VarChar).Value = objDetail.Qty;
        //            cmd.Parameters.Add("@newqty", SqlDbType.VarChar).Value = objDetail.NewQty;
        //            cmd.Parameters.Add("@oldbatch", SqlDbType.VarChar).Value = objDetail.Batch;
        //            cmd.Parameters.Add("@newbatch", SqlDbType.VarChar).Value = objDetail.NewBatch;
        //            cmd.Parameters.Add("@Checkername", SqlDbType.VarChar).Value = objDetail.CheckerName;
        //            cmd.Parameters.Add("@changedatetime", SqlDbType.VarChar).Value = Chakerdate;
        //            cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = objDetail.Description;
        //            cmd.Parameters.Add("@IsDelete", SqlDbType.VarChar).Value = objDetail.Isdelete;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string InsertUnpickedChanges(UnpickedDetail objDetail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {
        //            string invdate = Convert.ToDateTime(objDetail.Invdt).ToString("yyyy-MM-dd");

        //            myOrderConnection.Open();
        //            string sql = "Insert into App_UnPickedList(CustCode,CustomerName,Invdt,Qty,CustId,Itemname,Pack,Location,Status,NewQty,Invnum,Itemid,Batchid,Batch,expiry,mrp,itemcode,routeid,route,Block,checkername,CheckerId,Phoneid,PickerName) values(@CustCode, @CustomerName, @Invdt, @Qty, @CustId, @Itemname, @Pack, @Location, @Status, @NewQty, @Invnum, @Itemid, @Batchid, @Batch, @expiry, @mrp, @itemcode, @routeid, @route, @Block, @checkername, @CheckerId, @Phoneid, @PickerName)";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@CustCode", SqlDbType.VarChar).Value = objDetail.CustCode;
        //            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = objDetail.CustomerName;
        //            cmd.Parameters.Add("@Invdt", SqlDbType.DateTime).Value =Convert.ToDateTime(invdate);
        //            cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = objDetail.Qty;
        //            cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = objDetail.CustId;
        //            cmd.Parameters.Add("@Itemname", SqlDbType.VarChar).Value = objDetail.Itemname;
        //            cmd.Parameters.Add("@Pack", SqlDbType.VarChar).Value = objDetail.Pack;
        //            cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = objDetail.Location;
        //            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = objDetail.Status;
        //            cmd.Parameters.Add("@NewQty", SqlDbType.Decimal).Value = objDetail.NewQty;
        //            cmd.Parameters.Add("@Invnum", SqlDbType.VarChar).Value = objDetail.Invnum;
        //            cmd.Parameters.Add("@Itemid", SqlDbType.Int).Value = objDetail.Itemid;
        //            cmd.Parameters.Add("@Batchid", SqlDbType.Int).Value = objDetail.Batchid;
        //            cmd.Parameters.Add("@Batch", SqlDbType.VarChar).Value = objDetail.Batch;
        //            cmd.Parameters.Add("@expiry", SqlDbType.VarChar).Value = objDetail.expiry;
        //            cmd.Parameters.Add("@mrp", SqlDbType.Decimal).Value = objDetail.mrp;
        //            cmd.Parameters.Add("@itemcode", SqlDbType.VarChar).Value = objDetail.itemcode;
        //            cmd.Parameters.Add("@routeid", SqlDbType.Int).Value = objDetail.routeid;
        //            cmd.Parameters.Add("@route", SqlDbType.VarChar).Value = objDetail.route;
        //            cmd.Parameters.Add("@Block", SqlDbType.VarChar).Value = objDetail.Block;
        //            cmd.Parameters.Add("@checkername", SqlDbType.VarChar).Value = objDetail.checkername;
        //            cmd.Parameters.Add("@CheckerId", SqlDbType.VarChar).Value = objDetail.CheckerId;
        //            cmd.Parameters.Add("@Phoneid", SqlDbType.VarChar).Value = objDetail.Phoneid;
        //            cmd.Parameters.Add("@PickerName", SqlDbType.VarChar).Value = objDetail.PickerName;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string UpdatePickingDetail(CheckingDetail objDetail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "Update App_PickList set Chicked='y',NewQty=@NewQuenty,checkername=@ChickerName,ChickerId=@ChickerId  where Invdt=@Invdt and CustCode=@custCode and Itemid=@itemId and Batchid=@Batchid";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@NewQuenty", SqlDbType.Decimal).Value = objDetail.NewQty;
        //            cmd.Parameters.Add("@ChickerName", SqlDbType.VarChar).Value = objDetail.CheckerName;
        //            cmd.Parameters.Add("@ChickerId", SqlDbType.VarChar).Value = objDetail.CheckerId;
        //            cmd.Parameters.Add("@Invdt", SqlDbType.VarChar).Value = objDetail.Invdt;
        //            cmd.Parameters.Add("@custCode", SqlDbType.VarChar).Value = objDetail.CustCode;
        //            cmd.Parameters.Add("@itemId", SqlDbType.VarChar).Value = objDetail.Itemid;
        //            cmd.Parameters.Add("@Batchid", SqlDbType.VarChar).Value = objDetail.Batchid;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string UpdateChickingDetail(UpdatePickedDetail detail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "Update AppPickingDetails set Checked='y' where custcode=@custCode and invdt=@pickDate and [Block]=@block";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@custCode", SqlDbType.VarChar).Value = detail.custCode;
        //            cmd.Parameters.Add("@pickDate", SqlDbType.VarChar).Value = detail.InvDt;
        //            cmd.Parameters.Add("@block", SqlDbType.VarChar).Value = detail.block;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string UpdateQty(UpdateQty detail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "Update App_PickList set NewQty=@NewQuenty ,isqty='y' where Invdt=@Invdt and CustCode=@custCode and Itemid=@itemId and Batchid=@Batchid";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@NewQuenty", SqlDbType.Decimal).Value = detail.NewQuenty;
        //            cmd.Parameters.Add("@Invdt", SqlDbType.VarChar).Value = detail.Invdt;
        //            cmd.Parameters.Add("@custCode", SqlDbType.VarChar).Value = detail.custCode;
        //            cmd.Parameters.Add("@itemId", SqlDbType.VarChar).Value = detail.itemId;
        //            cmd.Parameters.Add("@Batchid", SqlDbType.VarChar).Value = detail.Batchid;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string UpdateBatchStatus(UpdateQty detail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "Update App_PickList set isbatch='y' where Invdt=@Invdt and CustCode=@custCode and Itemid=@itemId and Batchid=@Batchid";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@Invdt", SqlDbType.VarChar).Value = detail.Invdt;
        //            cmd.Parameters.Add("@custCode", SqlDbType.VarChar).Value = detail.custCode;
        //            cmd.Parameters.Add("@itemId", SqlDbType.VarChar).Value = detail.itemId;
        //            cmd.Parameters.Add("@Batchid", SqlDbType.VarChar).Value = detail.Batchid;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string UpdateDeleteStatus(UpdateQty detail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "Update App_PickList set isdelete='y' where Invdt=@Invdt and CustCode=@custCode and Itemid=@itemId and Batchid=@Batchid";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@Invdt", SqlDbType.VarChar).Value = detail.Invdt;
        //            cmd.Parameters.Add("@custCode", SqlDbType.VarChar).Value = detail.custCode;
        //            cmd.Parameters.Add("@itemId", SqlDbType.VarChar).Value = detail.itemId;
        //            cmd.Parameters.Add("@Batchid", SqlDbType.VarChar).Value = detail.Batchid;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}
        //public static string UpdateNewBatchStatus(UpdateBatchStatus detail)
        //{
        //    string response = string.Empty;
        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {

        //            myOrderConnection.Open();
        //            string sql = "update salepurchase2 set Batch1=@NewBatch where Trntype='SB' and Batch = @OldBatch and Vno = @Invnum and Vdt = @InvDate and Acno = @custid and Itemc = @itemid and Psrlno = @BatchID";
        //            SqlCommand cmd = new SqlCommand(sql, myOrderConnection);
        //            cmd.Parameters.Add("@NewBatch", SqlDbType.VarChar).Value = detail.NewBatch;
        //            cmd.Parameters.Add("@OldBatch", SqlDbType.VarChar).Value = detail.OldBatch;
        //            cmd.Parameters.Add("@Invnum", SqlDbType.Int).Value = detail.Invnum;
        //            cmd.Parameters.Add("@InvDate", SqlDbType.VarChar).Value = detail.Invdt;
        //            cmd.Parameters.Add("@custid", SqlDbType.Int).Value = detail.Custid;
        //            cmd.Parameters.Add("@itemid", SqlDbType.Int).Value = detail.itemId;
        //            cmd.Parameters.Add("@BatchID", SqlDbType.Int).Value = detail.Batchid;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.ExecuteNonQuery();
        //            response = "success";
        //            myOrderConnection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            myOrderConnection.Close();
        //            response = "Failure";
        //        }
        //    }
        //    return response;

        //}

        //public static List<InvoiceDetail> GetPickingItemDetail(string invdate)
        //{
        //    List<InvoiceDetail> details = new List<InvoiceDetail>();

        //    var con = ConfigurationManager.ConnectionStrings["MasterDB"].ToString();
        //    using (SqlConnection myOrderConnection = new SqlConnection(con))
        //    {
        //        try
        //        {


        //            myOrderConnection.Open();
                    
        //            string dbOrderitems = "SELECT *  FROM App_PickList where invdt ='" + invdate + "'";
        //            SqlCommand orderitemsCmd = new SqlCommand(dbOrderitems, myOrderConnection);
        //            SqlDataReader invReader = orderitemsCmd.ExecuteReader();
        //            if (invReader.Read())
        //            {
        //                InvoiceDetail detail = new InvoiceDetail();


        //                detail.CustCode = invReader["CustCode"].ToString();
        //                detail.Invdt = Convert.ToDateTime(invReader["Invdt"]);
        //                detail.Itemid = Convert.ToInt32(invReader["Itemid"]);
        //                detail.Batchid = Convert.ToInt32(invReader["Batchid"]);
        //                details.Add(detail);
        //            }
        //            invReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    return details;
        //}

    }
}
