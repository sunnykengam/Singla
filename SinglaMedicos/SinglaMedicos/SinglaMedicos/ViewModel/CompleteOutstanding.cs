using MvvmHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SinglaMedicos.Common;
using SinglaMedicos.Model.Reports;
using Xamarin.Forms;
using Microsoft.AppCenter.Analytics;

namespace SinglaMedicos.ViewModel
{

    public class CompleteOutstanding
    {

        public ObservableRangeCollection<OutstandingViewmodel> compoutstandinglist { get; private set; }
            = new ObservableRangeCollection<OutstandingViewmodel>();
        public ICommand LoadDataCommand { get; private set; }
        public ICommand HeaderClickCommand { get; private set; }
        public CompleteOutstanding()
        {
            try
            {
                this.LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
                this.HeaderClickCommand = new Command<OutstandingViewmodel>((item) => ExecuteHeaderClickCommand(item));
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOutstanding--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        public async Task ExecuteLoadDataCommand()
        {
            try
             {
                ObservableRangeCollection<Outstanding> invlst = new ObservableRangeCollection<Outstanding>();
                if (string.IsNullOrEmpty(AppSettings.outstandingcustlistJson))
                {
                    App.Database.GetOutstandingPendingBill(AppSettings.partnercode, AppSettings.salesmanid.ToString());
                }
                else
                {
                    AppSettings.OutstandingStatus = true;
                }
                if (AppSettings.singleCustpendingbill == true)
                {
                    var invlst1 = AppSettings.outstandingcustlist.Where(i => i.custcode == AppSettings.Custcode).ToList();
                    invlst = new ObservableRangeCollection<Outstanding>(invlst1);
                }
                else
                {
                    invlst = AppSettings.outstandingcustlist;
                }


                foreach (var data in invlst)
                {
                    Outstanding custdata = new Outstanding();
                    if (Device.Idiom == TargetIdiom.Phone)
                    {

                        if (data.customername.Length > 16)
                            custdata.customername = data.custcode.Trim() + "-" + data.customername.Substring(0, 16) + "..";
                        else
                            custdata.customername = data.custcode.Trim() + "-" + data.customername;

                    }
                    else
                    {
                        custdata.customername = data.custcode.Trim() + "-" + data.customername;
                    }
                   
                    custdata.custbalanceamount = data.custbalanceamount;
                    custdata.inv_List.AddRange(data.inv_List);
                    var Custview = new OutstandingViewmodel(custdata);
                    compoutstandinglist.Add(Custview);
                }
                AppSettings.outstandingbill = compoutstandinglist;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CompleteOutstanding--ExecuteLoadDataCommand--Error : " + AppSettings.salesmancode + "--" + ex.Message);
            }
        }

        private void ExecuteHeaderClickCommand(OutstandingViewmodel item)
        {
            item.Expanded = !item.Expanded;
        }
    }
}
