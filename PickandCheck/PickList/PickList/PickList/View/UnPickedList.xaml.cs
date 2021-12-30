using PickList.Model;
using PickList.ViewModel;
using Plugin.Connectivity;
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
	public partial class UnPickedList : ContentPage
	{
        public List<object> chkboxes = new List<object>();
        ObservableCollection<UnPickingList> Listitems = new ObservableCollection<UnPickingList>();
        public UnPickedList ()
		{
            try
            {
                InitializeComponent();
                BackgroundImage = "BackgroundImage.png";
                Listitems = GetAPIData.GetUnpickinglist(AppSettings.PhoneCode);
                foreach (var data in Listitems)
                {
                    data.Qty = (int)data.Qty;
                }
                datagrid.ItemsSource = Listitems;
                ToastNotification.TostMessage("Total Lines" + Listitems.Count);
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
                int ItemId = Convert.ToInt32(Item.GetType().GetProperty("Itemid").GetValue(Item, null));
                int BatchId = Convert.ToInt32(Item.GetType().GetProperty("Batchid").GetValue(Item, null));
                string InvNO = Convert.ToString(Item.GetType().GetProperty("Invnum").GetValue(Item, null));
                var AddItem = Listitems.Where(i => i.Itemid == ItemId && i.Batchid == BatchId && i.Invnum == InvNO).FirstOrDefault();
                if (!chkboxes.Contains(Item))
                {
                    chkboxes.Add(Item);
                    AddItem.Ischecked = true;
                }
                else if (chkboxes.Contains(Item))
                {
                    chkboxes.Remove(Item);
                    AddItem.Ischecked = false;
                }

                popupLoadingView.IsVisible = false;
            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
            }
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    List<UnPickList> UnpickingData = new List<UnPickList>();
                    popupLoadingView.IsVisible = true;
                    await Task.Delay(5);
                    foreach (UnPickingList Rowinfo in Listitems)
                    {
                        UnPickList obj = new UnPickList();
                        obj.Invdt = Rowinfo.Invdt.ToString("yyyy-MM-dd");
                        obj.custCode = Rowinfo.custcode.Trim();
                        obj.itemId = Rowinfo.Itemid.ToString();
                        obj.Batchid = Rowinfo.Batchid.ToString();
                        obj.Invnum = Rowinfo.Invnum;
                        if (Rowinfo.Ischecked == true)
                            UnpickingData.Add(obj);
                    }
                    if(UnpickingData.Count != 0)
                        GetAPIData.UpdateUnPickingDetail(UnpickingData);

                    await Navigation.PopAsync();
                }
                else
                {
                    ToastNotification.TostMessage("Please Check Your InterNet Connection");
                }
            }
            catch (Exception ex)
            {
                popupLoadingView.IsVisible = false;
            }
        }
    }
}