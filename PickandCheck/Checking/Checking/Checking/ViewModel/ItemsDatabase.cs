using Checking.Models;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Checking.ViewModel
{
    public class ItemsDatabase
    {
        readonly SQLiteAsyncConnection database;

        public ItemsDatabase()
        {
            try
            {
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Invoiceitems.sqlite");
                database = new SQLiteAsyncConnection(path);
            }
            catch (Exception ex)
            {
            }
        }
        public void GetUrl()
        {
            try
            {
                //using (SqlConnection connection = new SqlConnection(@"data source=192.168.2.8;initial catalog=EsData;user id=sa;password=keimed@123;"))
                //{
                //    connection.Open();
                //    SqlCommand command = new SqlCommand("select Ipaddress from App_URL where Appname='Checking App'", connection);
                //    AppSettings.IPAddress = command.ExecuteScalar().ToString();
                //}
            }
            catch (Exception ex)
            {
            }
        }

        public  void UpdateandInsertqtyinAppinvoicechangesApp_picklist(UpdateQty objdty, CheckingDetail objDetails, string NewQty, int OldQty, InvoiceItem AddItem, InvoiceItem data, double FQty, double NewFQty)
        {
            try
            {
                //Insert AppInvoiceChanges Table //
                objDetails.Invdt = data.Invdt;
                objDetails.Invnum = Convert.ToInt32(data.Invnum);
                objDetails.CustId = Convert.ToInt32(data.CustId);
                objDetails.CustCode = data.CustCode;
                objDetails.CustomerName = data.CustomerName;
                objDetails.Itemid = data.Itemid;
                if (!string.IsNullOrEmpty(data.itemcode))
                    objDetails.Itemcode = data.itemcode;
                else
                    objDetails.Itemcode = "0";

                objDetails.Itemname = data.Itemname;
                objDetails.Remarks = "Item Updated old Qty: " + data.Qty + "- New Qty: " + NewQty;
                objDetails.Qty = data.Qty.ToString();
                objDetails.NewQty = NewQty;
                objDetails.FQty = FQty;
                objDetails.NewFQty = NewFQty;
                objDetails.batch = data.Batch;
                objDetails.NewBatch = "";
                objDetails.CheckerName = AppSettings.UserDetail.checkername;
                objDetails.ChickedDate = Convert.ToDateTime(DateTime.Today.Date.ToString("yyyy-MM-dd"));
                objDetails.Description = "QTY Change";
                objDetails.IsStatus = 2;
                objDetails.Isdelete = false;
                objDetails.InvType = data.InvType;
                objDetails.Routename = data.route;
                objDetails.NewPsrlno = data.Batchid;
                objDetails.Batchid = data.Batchid;
                objDetails.Id = data.Id;
                objDetails.mrp = data.mrp;
                objDetails.Rate = data.Rate;

                GetAPIData.InsertInvoiceChanges(objDetails);

                //Update App_Picklist Table //
                objdty.Invdt = data.Invdt;
                objdty.custCode = data.CustCode;
                objdty.itemId = data.Itemid.ToString();
                objdty.Batchid = data.Batchid.ToString();
                objdty.Invnum = data.Invnum;
                if (!string.IsNullOrEmpty(NewQty))
                    objdty.NewQuenty = Convert.ToDecimal(NewQty);
                objdty.CheckedTime = DateTime.Now.ToString("HH:mm:ss");
                objdty.Id = data.Id;
                objdty.NewFQty = NewFQty;
                GetAPIData.UpdateQtyChange(objdty);
            }
            catch (Exception ex)
            {
            }
        }

        public void UpdateandInsertDatainAppinvoicechangesApp_picklist(UpdateQty objdty, CheckingDetail objDetails, InvoiceItem data)
        {
            try
            {
                //Insert AppInvoiceChanges Table //
                objDetails.Invdt = data.Invdt;
                objDetails.Invnum = Convert.ToInt32(data.Invnum);
                objDetails.CustId = Convert.ToInt32(data.CustId);
                objDetails.CustCode = data.CustCode;
                objDetails.CustomerName = data.CustomerName;
                objDetails.Itemid = data.Itemid;
                if (!string.IsNullOrEmpty(data.itemcode))
                    objDetails.Itemcode = data.itemcode;
                else
                    objDetails.Itemcode = "0";

                objDetails.Itemname = data.Itemname;
                objDetails.Remarks = "Item Delete old Qty: " + data.Qty + "- New Qty: 0";
                objDetails.Qty = data.Qty.ToString();
                objDetails.NewQty = "";
                objDetails.batch = data.Batch;
                objDetails.NewBatch = "";
                objDetails.CheckerName = AppSettings.UserDetail.checkername;
                objDetails.ChickedDate = Convert.ToDateTime(DateTime.Today.Date.ToString("yyyy-MM-dd"));
                objDetails.Description = "ITEM DELETE";
                objDetails.Isdelete = true;
                objDetails.IsStatus = 1;
                objDetails.NewPsrlno = data.Batchid;
                objDetails.InvType = data.InvType;
                objDetails.Routename = data.route;
                objDetails.Batchid = data.Batchid;
                objDetails.Id = data.Id;
                objDetails.mrp = data.mrp;
                objDetails.Rate = data.Rate;
                GetAPIData.InsertInvoiceChanges(objDetails);



                //Update App_Picklist Table //
                objdty.Invdt = data.Invdt;
                objdty.custCode = data.CustCode;
                objdty.itemId = data.Itemid.ToString();
                objdty.Batchid = data.Batchid.ToString();
                objdty.Invnum = data.Invnum;
                objdty.NewQuenty = 0;
                objdty.CheckedTime = DateTime.Now.ToString("HH:mm:ss");
                objdty.Id = data.Id;
                GetAPIData.UpdateDeleteStatus(objdty);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task InsertandUpdateItemsinServer()
        {
            //if (CrossConnectivity.Current.IsConnected)
            //{
            //    try
            //    {
            //        if (AppSettings.CheckerPickedList != null)
            //        {
            //            if (AppSettings.CheckerPickedList.Count != 0)
            //            {
            //                List<CheckingDetail> obj = new List<CheckingDetail>();
            //                var list = AppSettings.CheckerPickedList;
            //                foreach (InvoiceItem data in list)
            //                {
            //                    UpdateQty objdty = new UpdateQty();

            //                    CheckingDetail objDetails = new CheckingDetail();
            //                    objDetails.Invdt = data.Invdt;
            //                    objDetails.CustCode = data.CustCode;

            //                    objDetails.Invnum = Convert.ToInt32(data.Invnum);
            //                    objDetails.CustId = Convert.ToInt32(data.CustId);
            //                    objDetails.Location = data.Location;
            //                    objDetails.CheckedTime = data.CheckedTime;
            //                    objDetails.Status = data.Status;
            //                    objDetails.expiry = data.expiry;
            //                    objDetails.Itemname = data.Itemname;
            //                    objDetails.mrp = data.mrp;
            //                    objDetails.Qty = data.Qty.ToString();
            //                    objDetails.batch = data.Batch;
            //                    objDetails.Isqty = "y";
            //                    objDetails.ChickedDate = Convert.ToDateTime(DateTime.Today.Date.ToString("yyyy-MM-dd"));
            //                    objDetails.CustomerName = data.CustomerName;
            //                    objDetails.Itemcode = data.itemcode;
            //                    objDetails.Itemid = data.Itemid;
            //                    if (data.Chicked)
            //                        objDetails.Checked = "y";
            //                    else
            //                        objDetails.Checked = null;

            //                    objDetails.Batchid = data.Batchid;
            //                    objDetails.Id = data.Id;
            //                    objDetails.CheckerName = data.CheckerName;
            //                    objDetails.CheckerId = data.CheckerId;
            //                    if (!string.IsNullOrEmpty(data.NewQty))
            //                    {
            //                        if (data.Qty >= Convert.ToInt32(data.NewQty))
            //                        {
            //                            objDetails.NewQty = data.NewQty.ToString();
            //                            objdty.Invdt = data.Invdt;
            //                            objdty.Invnum = data.Invnum;
            //                            objdty.custCode = data.CustCode;
            //                            objdty.itemId = data.Itemid.ToString();
            //                            objdty.NewQuenty = Convert.ToDecimal(data.NewQty);
            //                            objdty.Batchid = data.Batchid.ToString();
            //                            objDetails.Remarks = "Item Updated old Qty: " + data.Qty + "- New Qty: " + objDetails.NewQty;
            //                            objDetails.Description = "QTY Change";
            //                            objDetails.IsStatus = 2;
            //                            objDetails.Routename = data.route;
            //                            objDetails.InvType = data.InvType;
            //                            objDetails.NewPsrlno = data.Batchid;
            //                            objDetails.OldPsrlno = data.Batchid;
            //                            GetAPIData.InsertInvoiceChanges(objDetails);
            //                            GetAPIData.UpdateQtyChange(objdty);
            //                        }
            //                    }
            //                    else
            //                        objDetails.NewQty = "0";

            //                    obj.Add(objDetails);
            //                }
            //                GetAPIData.UpdatePickList(obj);
            //               var OrderJson = JsonConvert.SerializeObject(obj);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
            //AppSettings.InsertStatus = false;
        }

        public async Task<string> getAppvarsions(string Name)
        {
            try
            {
                var value = Task.Run(async () => await database.Table<Appvarsions>().Where(i => i.Name == Name).FirstOrDefaultAsync()).Result.Value;
                return value;
            }
            catch (Exception ex)
            {
                return "";
            }
        } 

        public async void UpdateAppvarsions(string Name, string value)
        {
            try
            {
                Appvarsions obj = new Appvarsions();
                obj.Name = Name;
                obj.Value = value;
                obj.sno = 1;
                string qry = "update Appvarsions set value='" + value + "' where sno =1";
                var DATA = Task.Run(async () => await database.QueryAsync<Appvarsions>(qry)).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public async void ConnectionChecking()
        {
            try
            {
                var profiles = Connectivity.ConnectionProfiles;
                if (!profiles.Contains(ConnectionProfile.WiFi))
                {
                    ToastNotification.TostMessage("Please Check InterNet Connection..");
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
public interface IFileHelper
{
    string GetLocalFilePath(string filename);
    void CopyDatabase();
    string Version { get; }
    void closeApplication();
}


