using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model.Reports;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;

namespace SinglaApp.View.Reports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerPendingBill : ContentPage
	{
        ObservableRangeCollection<OutstandingInvoices> custinv_Lst = new ObservableRangeCollection<OutstandingInvoices>();
        decimal GrandTotal = 0;
        public CustomerPendingBill()
        {
            try
            {
                InitializeComponent();
                App.Database.GetCustomerPendingBill(AppSettings.partnercode, AppSettings.customerid);
                var invlst = AppSettings.customeroutstanding;
                foreach (var invoice_lst in invlst)
                {
                    OutstandingInvoices invdata = new OutstandingInvoices();
                    invdata.invoicedate = invoice_lst.invoicedate;
                    invdata.invoicenum = invoice_lst.invoicenum;
                    invdata.invtotalamount = invoice_lst.invtotalamount;
                    invdata.invbalanceamount = invoice_lst.invbalanceamount;
                    invdata.totaldiscount = invoice_lst.totaldiscount;
                    invdata.Days = invoice_lst.Days;
                    invdata.noofitem = invoice_lst.noofitem;
                    custinv_Lst.Add(invdata);
                }
                custmeroutstandinglst.ItemsSource = custinv_Lst;
                lblorders.Text = custinv_Lst.Count + "  Invoices";
                foreach (var data in custinv_Lst)
                {
                    GrandTotal = GrandTotal + data.invbalanceamount;
                }
                lbltotalamount.Text = "Rs: ₹ " + GrandTotal;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("CustomerPendingBill--Error : " + AppSettings.customercode + "--" + ex.Message);
            }
        }
	}
}