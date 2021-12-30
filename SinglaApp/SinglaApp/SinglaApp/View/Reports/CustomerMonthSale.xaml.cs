using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinglaApp.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp.View.Reports
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerMonthSale : ContentPage
	{
        decimal GrandTotal=0;
        public CustomerMonthSale ()
		{
            try
            {
                InitializeComponent();
                App.Database.GetCustomerMonthWiseSale();
                lblcustcodename.Text = AppSettings.customercode + "-" + AppSettings.customername;
                if (AppSettings.Customermonthsale != null)
                {
                    if (AppSettings.Customermonthsale.Count != 0)
                    {
                        custsalesList.ItemsSource = AppSettings.Customermonthsale;
                        foreach (var data in AppSettings.Customermonthsale)
                        {
                            GrandTotal = GrandTotal + data.sales;
                        }
                        lblsaletotalamount.Text = GrandTotal.ToString("0.##");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}