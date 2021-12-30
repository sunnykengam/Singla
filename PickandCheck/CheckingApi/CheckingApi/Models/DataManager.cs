using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CheckingApi.Models
{
    public class DataManager
    {
     
        public UserMaster GetCheckingUserMaster(string Checkerid, SqlConnection myConnection)
        {
            UserMaster orderDetails = new UserMaster();
            try
            {
                myConnection.Open();
                string dbOrder = "select distinct code as checkercode,name as checkername from App_Checkers where code='" + Checkerid + "' ";
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(dbOrder, myConnection);
                sda.Fill(dt1);
                myConnection.Close();
                orderDetails = (from DataRow row in dt1.Rows
                                select new UserMaster
                                {
                                    checkerid = Convert.ToInt32(row["checkercode"]),
                                    checkername = row["checkername"].ToString()
                                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return orderDetails;
        }

        public List<CheckingDetail> GetCheckingDetailsList(string Checkerid, DateTime InvDate, SqlConnection myConnection)
        {
            List<CheckingDetail> checkingDetail = new List<CheckingDetail>();
            try
            {
                myConnection.Open();
                string dbOrder = "Select ChickerId,case when assignby is null then checkername else assignby end as checkername,custid,custcode,custname,NoofInvoices as Invnum,routeid,route,Block,PickingStatus as Picked,Checked,phone as pickerid,(select EmployeeName from AppPickerMaster where PickerCode = 'PIC' + pickername) as pickername,basketNo,phone From AppPickingDetails Where ChickerId = '" + Checkerid + "' and invdt = '" + InvDate + "' order by custcode ";
                SqlCommand salesCmd = new SqlCommand(dbOrder, myConnection);
                int i = 1;
                var checkReader = salesCmd.ExecuteReader();
                while (checkReader.Read())
                {
                    CheckingDetail Checking_Details = new CheckingDetail();
                    Checking_Details.sno = i.ToString();
                    i++;
                    Checking_Details.checkerid = Convert.ToInt32(checkReader["ChickerId"].ToString());
                    Checking_Details.custid = Convert.ToInt32(checkReader["custid"].ToString());
                    if (!string.IsNullOrEmpty(checkReader["custcode"].ToString()))
                        Checking_Details.custcode = checkReader["custcode"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["checkername"].ToString()))
                        Checking_Details.checkername = checkReader["checkername"].ToString().Trim();


                    if (!string.IsNullOrEmpty(checkReader["custname"].ToString()))
                        Checking_Details.custname = checkReader["custname"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["routeid"].ToString()))
                        Checking_Details.routeid = checkReader["routeid"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["pickername"].ToString()))
                        Checking_Details.pickername = checkReader["pickername"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["basketNo"].ToString()))
                        Checking_Details.basketNo = checkReader["basketNo"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["phone"].ToString()))
                        Checking_Details.phone = checkReader["phone"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["route"].ToString()))
                        Checking_Details.route = checkReader["route"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Block"].ToString()))
                        Checking_Details.Block = checkReader["Block"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Invnum"].ToString()))
                        Checking_Details.Invnum = checkReader["Invnum"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Picked"].ToString()))
                        Checking_Details.Picked = checkReader["Picked"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Checked"].ToString()))
                        Checking_Details.Checked = checkReader["Checked"].ToString().Trim();

                    if (Checking_Details.Picked == "y" && Checking_Details.Checked == "y")
                        Checking_Details.IsDisable = false;
                    else
                        Checking_Details.IsDisable = true;

                    checkingDetail.Add(Checking_Details);
                }
                checkReader.Close();
                myConnection.Close();
            }
            catch (Exception ex)
            {
            }
            return checkingDetail;
        }

        public int GetCheckingHoldCount(string Custcode, DateTime InvDt, string Block, string Invnum, SqlConnection myConnection)
        {
            int Count = 0;
            try
            {
                myConnection.Open();
                string sql = "Select count(*) from App_PickList where CustCode='" + Custcode + "' and Invdt='" + InvDt + "' and Block='" + Block + "' and Chicked='y' and Invnum='" + Invnum + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                Count = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            return Count;
        }

        public List<InvoiceDetail> GetInvoiceDetails(string Custcode, DateTime InvDt, string Block, string Invnum, SqlConnection myConnection)
        {
            List<InvoiceDetail> checkingDetail = new List<InvoiceDetail>();
            try
            {
                myConnection.Open();
                string dbOrder = "Select a.Id,a.CustId,a.CustCode,a.CustomerName,a.Invdt,a.Itemname,a.Pack,a.Location,a.Status,convert(int,a.Qty) as Qty,convert(int,a.NewQty) as NewQty,a.Picked,a.Invnum,a.Itemid,a.itemcode,a.Batchid,a.Batch,a.expiry,a.isbatch,a.isqty,a.isdelete,a.isUnpick,a.route,a.InvType,a.Phoneid,convert(int,a.FQty) as FQty,a.mrp,a.Chicked,a.BasketNo,a.checkedtime,a.SlipNum,f.ScmStr as scheme ,a.NewBatch,a.Newbatchid,convert(int,a.NewFqty) as NewFqty,a.rate,a.BatchQty,a.ManualBatch from App_PickList a, fifo f where a.Itemid = f.Itemc and f.psrlno = a.Batchid and f.Batch = a.Batch  and CustCode = '" + Custcode + "' and Invdt = '"+ InvDt + "' and Block = '"+ Block + "' and chlntype<>'SN' and Invnum = '"+ Invnum + "' order by a.Location asc, a.Picked desc";
                SqlCommand salesCmd = new SqlCommand(dbOrder, myConnection);
                int i = 1;
                var checkReader = salesCmd.ExecuteReader();
                while (checkReader.Read())
                {
                    InvoiceDetail Checking_Details = new InvoiceDetail();
                    Checking_Details.sno = i.ToString();
                    i++;

                    Checking_Details.Id = Convert.ToInt32(checkReader["Id"].ToString());
                    Checking_Details.CustId = Convert.ToInt32(checkReader["CustId"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["CustCode"].ToString()))
                        Checking_Details.CustCode = checkReader["CustCode"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["CustomerName"].ToString()))
                        Checking_Details.CustomerName = checkReader["CustomerName"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Invdt"].ToString()))
                        Checking_Details.Invdt = Convert.ToDateTime(checkReader["Invdt"].ToString().Trim());

                    if (!string.IsNullOrEmpty(checkReader["Itemname"].ToString()))
                        Checking_Details.Itemname = checkReader["Itemname"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Pack"].ToString()))
                        Checking_Details.Pack = checkReader["Pack"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Location"].ToString()))
                        Checking_Details.Location = checkReader["Location"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Status"].ToString()))
                        Checking_Details.Status = checkReader["Status"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Qty"].ToString()))
                        Checking_Details.Qty = Convert.ToInt32(checkReader["Qty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["NewQty"].ToString()))
                        Checking_Details.NewQty = Convert.ToInt32(checkReader["NewQty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["Invnum"].ToString()))
                        Checking_Details.Invnum = checkReader["Invnum"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Picked"].ToString()))
                        Checking_Details.Picked = checkReader["Picked"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Itemid"].ToString()))
                        Checking_Details.Itemid = Convert.ToInt32(checkReader["Itemid"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["itemcode"].ToString()))
                        Checking_Details.itemcode = checkReader["itemcode"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Batchid"].ToString()))
                        Checking_Details.Batchid = Convert.ToInt32(checkReader["Batchid"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["FQty"].ToString()))
                        Checking_Details.FQty = Convert.ToInt32(checkReader["FQty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["Batch"].ToString()))
                        Checking_Details.Batch = checkReader["Batch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["expiry"].ToString()))
                        Checking_Details.expiry = checkReader["expiry"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isbatch"].ToString()))
                        Checking_Details.isbatch = checkReader["isbatch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isqty"].ToString()))
                        Checking_Details.isqty = checkReader["isqty"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isdelete"].ToString()))
                        Checking_Details.isdelete = checkReader["isdelete"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isUnpick"].ToString()))
                        Checking_Details.isUnpick = checkReader["isUnpick"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["route"].ToString()))
                        Checking_Details.route = checkReader["route"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["InvType"].ToString()))
                        Checking_Details.InvType = checkReader["InvType"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["NewFQty"].ToString()))
                        Checking_Details.NewFQty = Convert.ToInt32(checkReader["NewFQty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["mrp"].ToString()))
                        Checking_Details.mrp = Convert.ToDecimal(checkReader["mrp"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["rate"].ToString()))
                        Checking_Details.Rate = Convert.ToDecimal(checkReader["rate"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["BasketNo"].ToString()))
                        Checking_Details.BasketNo = checkReader["BasketNo"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["checkedtime"].ToString()))
                        Checking_Details.CheckedTime = checkReader["checkedtime"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["scheme"].ToString()))
                        Checking_Details.Scheme = checkReader["scheme"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["NewBatch"].ToString()))
                        Checking_Details.NewBatch = checkReader["NewBatch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Newbatchid"].ToString()))
                        Checking_Details.NewBatchid = checkReader["Newbatchid"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["BatchQty"].ToString()))
                        Checking_Details.BatchQty = checkReader["BatchQty"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["ManualBatch"].ToString()))
                        Checking_Details.ManualBatch = checkReader["ManualBatch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Chicked"].ToString()))
                        Checking_Details.Chicked = false;
                    else
                        Checking_Details.Chicked = true;

                    Checking_Details.BatchList = GetBatchListByItemId(Checking_Details.Itemid, Checking_Details.Invdt.AddMonths(-12), Checking_Details.Invdt, Convert.ToDouble(Checking_Details.Rate),myConnection);

                    checkingDetail.Add(Checking_Details);
                }
                checkReader.Close();
                myConnection.Close();   
            }
            catch (Exception ex)
            {
            }
            return checkingDetail;
        }

        public List<InvoiceDetail> GetInvoiceDetail(string Custcode, DateTime InvDt, int Itemid, string Batch, string Invnum, SqlConnection myConnection)
        {
            List<InvoiceDetail> checkingDetail = new List<InvoiceDetail>();
            try
            {
                myConnection.Open();
                string dbOrder = "Select a.Id,a.CustId,a.CustCode,a.CustomerName,a.Invdt,a.Itemname,a.Pack,a.Location,a.Status,convert(int,a.Qty) as Qty,convert(int,a.NewQty) as NewQty,a.Picked,a.Invnum,a.Itemid,a.itemcode,a.Batchid,a.Batch,a.expiry,a.isbatch,a.isqty,a.isdelete,a.isUnpick,a.route,a.InvType,a.Phoneid,convert(int,a.FQty) as FQty,a.mrp,a.Chicked,a.BasketNo,a.checkedtime,a.SlipNum,f.ScmStr as scheme ,a.NewBatch,a.Newbatchid,convert(int,a.NewFqty) as NewFqty,a.rate,a.BatchQty,a.ManualBatch from App_PickList a, fifo f where a.Itemid = f.Itemc and f.psrlno = a.Batchid and f.Batch = a.Batch  and CustCode = '" + Custcode + "' and Invdt = '" + InvDt + "' and Itemid = '" + Itemid + "' and chlntype<>'SN' and Invnum = '" + Invnum + "' and a.batch='"+ Batch + "' order by a.Location asc, a.Picked desc";
                SqlCommand salesCmd = new SqlCommand(dbOrder, myConnection);
                int i = 1;
                var checkReader = salesCmd.ExecuteReader();
                while (checkReader.Read())
                {
                    InvoiceDetail Checking_Details = new InvoiceDetail();
                    Checking_Details.sno = i.ToString();
                    i++;

                    Checking_Details.Id = Convert.ToInt32(checkReader["Id"].ToString());
                    Checking_Details.CustId = Convert.ToInt32(checkReader["CustId"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["CustCode"].ToString()))
                        Checking_Details.CustCode = checkReader["CustCode"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["CustomerName"].ToString()))
                        Checking_Details.CustomerName = checkReader["CustomerName"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Invdt"].ToString()))
                        Checking_Details.Invdt = Convert.ToDateTime(checkReader["Invdt"].ToString().Trim());

                    if (!string.IsNullOrEmpty(checkReader["Itemname"].ToString()))
                        Checking_Details.Itemname = checkReader["Itemname"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Pack"].ToString()))
                        Checking_Details.Pack = checkReader["Pack"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Location"].ToString()))
                        Checking_Details.Location = checkReader["Location"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Status"].ToString()))
                        Checking_Details.Status = checkReader["Status"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Qty"].ToString()))
                        Checking_Details.Qty = Convert.ToInt32(checkReader["Qty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["NewQty"].ToString()))
                        Checking_Details.NewQty = Convert.ToInt32(checkReader["NewQty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["Invnum"].ToString()))
                        Checking_Details.Invnum = checkReader["Invnum"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Picked"].ToString()))
                        Checking_Details.Picked = checkReader["Picked"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Itemid"].ToString()))
                        Checking_Details.Itemid = Convert.ToInt32(checkReader["Itemid"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["itemcode"].ToString()))
                        Checking_Details.itemcode = checkReader["itemcode"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Batchid"].ToString()))
                        Checking_Details.Batchid = Convert.ToInt32(checkReader["Batchid"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["FQty"].ToString()))
                        Checking_Details.FQty = Convert.ToInt32(checkReader["FQty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["Batch"].ToString()))
                        Checking_Details.Batch = checkReader["Batch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["expiry"].ToString()))
                        Checking_Details.expiry = checkReader["expiry"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isbatch"].ToString()))
                        Checking_Details.isbatch = checkReader["isbatch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isqty"].ToString()))
                        Checking_Details.isqty = checkReader["isqty"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isdelete"].ToString()))
                        Checking_Details.isdelete = checkReader["isdelete"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["isUnpick"].ToString()))
                        Checking_Details.isUnpick = checkReader["isUnpick"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["route"].ToString()))
                        Checking_Details.route = checkReader["route"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["InvType"].ToString()))
                        Checking_Details.InvType = checkReader["InvType"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["NewFQty"].ToString()))
                        Checking_Details.NewFQty = Convert.ToInt32(checkReader["NewFQty"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["mrp"].ToString()))
                        Checking_Details.mrp = Convert.ToDecimal(checkReader["mrp"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["rate"].ToString()))
                        Checking_Details.Rate = Convert.ToDecimal(checkReader["rate"].ToString());

                    if (!string.IsNullOrEmpty(checkReader["BasketNo"].ToString()))
                        Checking_Details.BasketNo = checkReader["BasketNo"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["checkedtime"].ToString()))
                        Checking_Details.CheckedTime = checkReader["checkedtime"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["scheme"].ToString()))
                        Checking_Details.Scheme = checkReader["scheme"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["NewBatch"].ToString()))
                        Checking_Details.NewBatch = checkReader["NewBatch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Newbatchid"].ToString()))
                        Checking_Details.NewBatchid = checkReader["Newbatchid"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["BatchQty"].ToString()))
                        Checking_Details.BatchQty = checkReader["BatchQty"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["ManualBatch"].ToString()))
                        Checking_Details.ManualBatch = checkReader["ManualBatch"].ToString().Trim();

                    if (!string.IsNullOrEmpty(checkReader["Chicked"].ToString()))
                        Checking_Details.Chicked = false;
                    else
                        Checking_Details.Chicked = true;

                    checkingDetail.Add(Checking_Details);
                }
                checkReader.Close();
                myConnection.Close();
            }
            catch (Exception ex)
            {
            }
            return checkingDetail;
        }

        public List<PickerList> GetBatchListByItemId(int Itemid, DateTime FromDate, DateTime Invdt, double Rate, SqlConnection myConnection)
        {
            List<PickerList> checkingDetail = new List<PickerList>();
            try
            {
                string dbOrder = "select i.code as itemid,i.barcode as itemcode,i.name as itemname,i.pack as pack,f.batch as batch,f.expiry as expiry,convert(int,f.bqty) as qty,f.srate as Rate,f.mrp as MRP,f.psrlno as batchid,f.ScmStr as Scheme  from item i, fifo f where f.Itemc = i.code and f.hold <> 'H' and f.srate = '" + Rate + "' and i.code = '" + Itemid + "' and f.vdt between '" + FromDate + "' and '" + Invdt + "' order by f.vdt desc";
                SqlCommand salesCmd = new SqlCommand(dbOrder, myConnection);
                var checkReader = salesCmd.ExecuteReader();
                while (checkReader.Read())
                {
                    PickerList picdata = new PickerList();
                    picdata.Itemid= Convert.ToInt32(checkReader["itemid"].ToString());
                    picdata.itemcode = checkReader["itemcode"].ToString();
                    picdata.Itemname = checkReader["itemname"].ToString();
                    picdata.pack = checkReader["pack"].ToString();
                    picdata.batch = checkReader["batch"].ToString();
                    picdata.expiry = checkReader["expiry"].ToString();
                    picdata.qty = Convert.ToInt32(checkReader["qty"].ToString());
                    picdata.Rate = Convert.ToDouble(checkReader["Rate"].ToString());
                    picdata.MRP = Convert.ToDouble(checkReader["MRP"].ToString());
                    picdata.batchid = Convert.ToInt32(checkReader["batchid"].ToString());
                    picdata.Scheme = checkReader["Scheme"].ToString();
                    checkingDetail.Add(picdata);
                }
            }
            catch (Exception ex)
            {
            }
            return checkingDetail;
        }

        public InvoiceSummary GetCustomerInvoiceSummary(string custcode, string invdate, string Invnum, SqlConnection myConnection)
        {
            InvoiceSummary picdata = new InvoiceSummary();
            try
            {
                myConnection.Open();
                string dbOrder = "SELECT invdt as Invdate,Checkername as checkername,route as routename,custcode as custcode,customername as custname,COUNT(Itemid) as Totalines,Invnum,Invtype,sum(case when Picked is null then 0 else 1 end) as pickedlines,sum(case when chicked is null then 0 else 1 end) as checkedlines,(COUNT(Itemid)) - (sum(case when chicked is null then 0 else 1 end)) as pendinglines,sum(case when isbatch is null then 0 else 1 end) as batchchanges,sum(case when isqty is null then 0 else 1 end) as qtychangelines,sum(case when isdelete is null then 0 else 1 end) as nillines FROM App_PickList where Invdt = '"+ invdate + "' and custcode = '"+ custcode + "' and Invnum = '"+Invnum+"' group by Checkername,route,custcode,customername,invdt,Invnum,Invtype";
                SqlCommand salesCmd = new SqlCommand(dbOrder, myConnection);
                var checkReader = salesCmd.ExecuteReader();
                while (checkReader.Read())
                {
                    
                    picdata.Invdate = Convert.ToDateTime(checkReader["Invdate"].ToString());
                    picdata.Checkername = checkReader["Checkername"].ToString();
                    picdata.routename = checkReader["routename"].ToString();
                    picdata.custcode = checkReader["custcode"].ToString();
                    picdata.custname = checkReader["custname"].ToString();
                    picdata.Totalines = Convert.ToInt32(checkReader["Totalines"].ToString());
                    picdata.Invnum = checkReader["Invnum"].ToString();
                    picdata.Invtype = checkReader["Invtype"].ToString();
                    picdata.pickedlines = Convert.ToInt32(checkReader["pickedlines"].ToString());
                    picdata.batchchanges = Convert.ToInt32(checkReader["batchchanges"].ToString());
                    picdata.qtychangelines = Convert.ToInt32(checkReader["qtychangelines"].ToString());
                    picdata.nillines = Convert.ToInt32(checkReader["nillines"].ToString());
                }
                checkReader.Close();
                myConnection.Close();
            }
            catch (Exception ex)
            {
            }
            return picdata;
        }

        public string GetEDPStatus(string custcode, string invdate, string Invnum, SqlConnection myConnection)
        {
            string Esstatus = string.Empty;
            try
            {
                myConnection.Open();
                string sql = "select count(*) from AppInvoiceChanges where custcode='"+ custcode + "' and Invdate='"+ invdate + "' and Invnum='"+ Invnum + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                int count = (int)cmd.ExecuteScalar();
                if (count != 0)
                {
                    string sql1 = "select DISTINCT (case when EsStatus is null then 'N' else EsStatus end ) as EsStatus  from AppInvoiceChanges where custcode='" + custcode + "' and Invdate='" + invdate + "' and Invnum='" + Invnum + "'";
                    SqlCommand cmd1 = new SqlCommand(sql1, myConnection);
                    Esstatus = (string)cmd1.ExecuteScalar();
                }
                else
                {
                    Esstatus = "y";
                }
            }
            catch (Exception ex)
            {
            }
            return Esstatus;
        }

        //public string GetInvoiceNumbersinApp_Picklist(string custcode, string invdate, string Invnum, SqlConnection myConnection)
        //{
        //    string Invnumber = string.Empty;
        //    try
        //    {
        //        myConnection.Open();
        //        string sql = "select DISTINCT Invnum  from App_PickList where custcode='" + custcode + "' and Invdt='" + invdate + "' and Invnum='" + Invnum + "'";
        //        SqlCommand cmd = new SqlCommand(sql, myConnection);
        //        Invnumber = (string)cmd.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Invnumber;
        //}

        public string GetInvoiceNumbersinAppInvoicechanges(string custcode, string invdate, string Invnum, SqlConnection myConnection)
        {
            string Invnumber = string.Empty;
            try
            {
                myConnection.Open();
                string sql = "select DISTINCT Invnum from AppInvoiceChanges where custcode='" + custcode + "' and Invdate='" + invdate + "' and Invnum='" + Invnum + "'";
                SqlCommand cmd = new SqlCommand(sql, myConnection);
                Invnumber = (string)cmd.ExecuteScalar();
               
            }
            catch (Exception ex)
            {
            }
            return Invnumber;
        }
    }
}