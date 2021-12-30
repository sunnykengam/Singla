using PickList.Model;
using PickList.ViewModel;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using CheckBox = XLabs.Forms.Controls.CheckBox;

namespace PickList.View
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotPickedPage : ContentPage
    {
        public SQLiteAsyncConnection database;
        public List<object> chkboxes = new List<object>();
        public List<PickingDetail> notPickedDetails = new List<PickingDetail>();
        PickingDetail pickeditem = new PickingDetail();

        public NotPickedPage()
        {
            try
            {
                InitializeComponent();
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("PicklistDB.Sqlite");
                database = new SQLiteAsyncConnection(path);
                string invnum = AppSettings.customerDetail.Invnum.ToString();
                notPickedDetails = Task.Run(async () => await database.Table<PickingDetail>().Where(k => k.Picked==null && k.CustCode==AppSettings.customerDetail.custcode && k.Invdt==AppSettings.customerDetail.invdt && k.Invnum== invnum).ToListAsync()).Result;
                foreach (var item in notPickedDetails)
                {
                    item.rowcolor = "Transparent";
                    if (item.ITEMCATEGORY == 1)
                        item.rowcolor = "#DC143C";//#ColdRoom

                    if (item.ITEMCATEGORY == 2)
                        item.rowcolor = "#00FFFF";//#Drops

                    if (item.ITEMCATEGORY == 3)
                        item.rowcolor = "#F4A460";//#Powders 

                    if (item.ITEMCATEGORY == 4)
                        item.rowcolor = "#00FA9A";//#Syrups

                    if (item.ITEMCATEGORY == 5)
                        item.rowcolor = "#FFD700";//#Injections

                    if (item.ITEMCATEGORY == 6)
                        item.rowcolor = "#EE82EE";//#Office 
                }
                datagrid.ItemsSource = notPickedDetails;
            }
            catch (Exception ex)
            {
            }
        }

        private void CheckYes_Tapped(object sender, EventArgs e)
        {
            try
            {
                unpick.IsVisible = false;
                int ItemId = Convert.ToInt32(pickeditem.GetType().GetProperty("Itemid").GetValue(pickeditem, null));
                var AddItem = AppSettings.PickingDateDetails.Where(i => i.Itemid == ItemId).FirstOrDefault();
                chkboxes.Remove(pickeditem);
                AddItem.Ischecked = false;
                AddItem.BasketNo = null;
                AddItem.Picked = null;
                AddItem.PickedTime = DateTime.Now.ToString("HH:mm:ss");
                string qry = "update PickingDetail set Ischecked=false,Picked=null,BasketNo=null,PickedTime='" + AddItem.PickedTime + "' where ItemId ='" + AddItem.Itemid + "'";
                var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
            }
            catch (Exception ex)
            {
            }

        }

        private async void CheckNo_Tapped(object sender, EventArgs e)
        {
            try
            {
                unpick.IsVisible = false;
                popupLoadingView.IsVisible = true;
                await Task.Delay(2);
                int ItemId = Convert.ToInt32(pickeditem.GetType().GetProperty("Itemid").GetValue(pickeditem, null));
                var AddItem = AppSettings.PickingDateDetails.Where(i => i.Itemid == ItemId).FirstOrDefault();
                AddItem.Ischecked = true;
                datagrid.ItemsSource = null;
                datagrid.ItemsSource = AppSettings.PickingDateDetails.Where(k => k.Picked == null).OrderBy(i=>i.Ischecked==true);
            }
            catch (Exception ex)
            {

            }
            popupLoadingView.IsVisible = false;
        }



        private async void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                var Item = ((CheckBox)sender).BindingContext;
                pickeditem = (PickingDetail)Item;
                bool Checked = ((CheckBox)sender).Checked;
                int ItemId = Convert.ToInt32(Item.GetType().GetProperty("Itemid").GetValue(Item, null));
                var AddItem = AppSettings.PickingDateDetails.Where(i => i.Itemid == ItemId).FirstOrDefault();
                if (Checked == true)
                {
                    chkboxes.Add(Item);
                    AddItem.Ischecked = true;
                    AddItem.PickedTime = DateTime.Now.ToString("HH:mm:ss");
                    AddItem.BasketNo = AppSettings.CurrentBasketNo;
                    AddItem.Picked = "Y";
                    string qry = "update PickingDetail set Ischecked='" + AddItem.Ischecked + "',Picked='" + AddItem.Picked + "',BasketNo='" + AddItem.BasketNo + "',PickedTime='" + AddItem.PickedTime + "' where ItemId ='" + AddItem.Itemid + "'";
                    var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
                }
                else
                {
                    unpick.IsVisible = true;
                    chkboxes.Remove(Item);
                    AddItem.Ischecked = false;
                    AddItem.BasketNo = null;
                    AddItem.Picked = null;
                    AddItem.PickedTime = DateTime.Now.ToString("HH:mm:ss");
                    string qry = "update PickingDetail set Ischecked=false,Picked=null,BasketNo=null,PickedTime='" + AddItem.PickedTime + "' where ItemId ='" + AddItem.Itemid + "'";
                    var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
                }
            }
            catch (Exception ex)
            {
            }
            popupLoadingView.IsVisible = false;
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    AppSettings.BasketNo = null;
                    popupLoadingView.IsVisible = true;
                    await Task.Delay(2);
                    App.Database.InsertandUpdatePickListInServer();
                    GetAPIData.UpdatePickingDetail(AppSettings.PickingDateDetails[0].Invdt, AppSettings.Block, AppSettings.customerDetail.custcode.Trim(), AppSettings.Invnum);
                    string qry = "delete from PickingDetail";
                    var DATA = Task.Run(async () => await database.QueryAsync<PickingDetail>(qry)).Result;
                    await Navigation.PushAsync(new CheckerDetail());
                }
                else
                {
                    popupLoadingView.IsVisible = false;
                    await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                }

            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
            }
        }
        protected override bool OnBackButtonPressed()
        {
            App.Database.InsertandUpdatePickListInServer();
            Navigation.PopAsync();
            popupLoadingView.IsVisible = false;
            return true;
        }
    }
}