using SQLite;
using System;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyProfile : ContentPage
	{
        public SQLiteAsyncConnection database;
        public MyProfile ()
        {
            try
            {
                InitializeComponent();
                BackgroundColor = Color.White;
                var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
                database = new SQLiteAsyncConnection(path);

                if (AppSettings.loginname == "Salesman Login")
                {
                    if (!string.IsNullOrEmpty(AppSettings.Custcode))
                    {
                        stkcustdetail.IsVisible = true;
                        var custdetails = Task.Run(async () => await database.Table<CustomerMaster>().Where(i => i.customercode == AppSettings.Custcode).FirstOrDefaultAsync()).Result;
                        Lbcustname.Text = custdetails.customername;
                        LbPhonenumber.Text = custdetails.mobile;
                        Lbaddress.Text = custdetails.customeraddress;
                        LbEmail.Text = custdetails.customeremail;
                        Lbcustcode.Text = custdetails.customercode;
                        lbldlexpirydate.Text = custdetails.DLExpiry.ToString();
                        lbldlnumber.Text = custdetails.dlnumber;
                        lblgstnumber.Text = custdetails.gstnumber;
                        mainstack.Margin = new Thickness(20, 10, 20, 40);
                    }
                    else
                    {

                        BackgroundImage = "Mainbackground.png";
                        stkcustdetail.IsVisible = false;
                        lblsalesname.Text = "Salesman Name";
                        lblsalescode.Text = "Salesman Code";
                        lbname.Text = AppSettings.salesmanname;
                        Lbcustname.Text = AppSettings.salesmanname;
                        Lbcustcode.Text = Convert.ToString(AppSettings.Salesmandetail.salesmanid);
                        // Lbaddress.Text = AppSettings.Salesmandetail[0].a;
                        // LbEmail.Text = AppSettings.Salesmandetail[0].salesmanname;
                        LbPhonenumber.Text = AppSettings.Salesmandetail.mobile;
                    }
                }
                else
                {
                    stkcustdetail.IsVisible = true;
                    lbname.Text = AppSettings.customername;
                    Lbcustname.Text = AppSettings.customername;
                    Lbcustcode.Text = Convert.ToString(AppSettings.custdetail.customerid);
                    Lbaddress.Text = AppSettings.custdetail.customeraddress;
                    LbEmail.Text = AppSettings.custdetail.customeremail;
                    LbPhonenumber.Text = AppSettings.custdetail.mobile;
                    lbldlexpirydate.Text = AppSettings.custdetail.DLExpiry.ToString();
                    lbldlnumber.Text = AppSettings.custdetail.dlnumber;
                    lblgstnumber.Text = AppSettings.custdetail.gstnumber;
                }
            }
            catch (Exception ex)
            {
            }
        }
	}
}