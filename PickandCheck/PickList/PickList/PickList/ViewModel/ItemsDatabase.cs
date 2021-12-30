using PickList.Model;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickList.ViewModel
{
   public class ItemsDatabase
    {
        private static object obj;
        readonly SQLiteAsyncConnection database;

        public ItemsDatabase()
        {
            try
            {
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
                database = new SQLiteAsyncConnection(path);
            }
            catch (Exception ex)
            {
            }

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

        public void InsertandUpdatePickListInServer()
        {
            try
            {
                AppSettings.BasketNo = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var listitems = Task.Run(async () => await database.Table<PickingDetail>().ToListAsync()).Result;
                    if (listitems != null)
                    {
                        if (listitems.Count != 0)
                        {
                            ObservableCollection<UpdatePickList> subList = new ObservableCollection<UpdatePickList>();
                            foreach (PickingDetail pd in listitems)
                            {
                                UpdatePickList upitem = new UpdatePickList();
                                string invdate = Convert.ToDateTime(pd.Invdt).ToString("yyyy-MM-dd");
                                if (!string.IsNullOrEmpty(AppSettings.BasketNo))
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(pd.BasketNo))
                                        {
                                            var basket_no = AppSettings.BasketNo;
                                            string[] arrList = basket_no.Split(',');
                                            var query = (from num in arrList
                                                         where num == pd.BasketNo
                                                         select num).FirstOrDefault();
                                            if (query == null)
                                                AppSettings.BasketNo = AppSettings.BasketNo + "," + pd.BasketNo;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else
                                {
                                    AppSettings.BasketNo = pd.BasketNo;
                                }
                                upitem.Inv_Date = Convert.ToDateTime(invdate);
                                upitem.Cust_Code = pd.CustCode;
                                upitem.itemId = pd.Itemid.ToString();
                                upitem.Picked = pd.Picked;
                                if (string.IsNullOrEmpty(pd.Picked))
                                    upitem.Picked = null;

                                upitem.BasketNo = pd.BasketNo;
                                upitem.PickedTime = pd.PickedTime;
                                upitem.Invnum = pd.Invnum;
                                subList.Add(upitem);
                            }
                            if (listitems.Count != 0)
                            {
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    if (subList.Count != 0)
                                        GetAPIData.UpdatePicklist(subList);

                                    if (AppSettings.BasketNo != null)
                                        GetAPIData.UpdateBasketNo(listitems[0].Invdt, AppSettings.Block, AppSettings.customerDetail.custcode, AppSettings.BasketNo, AppSettings.Invnum);
                                }
                                else
                                {
                                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                                }
                            }
                        }
                    }
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
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
