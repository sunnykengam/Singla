using SinglaApp.Model;
using SQLite;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using SinglaApp.Common;

namespace SinglaApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        public SQLiteAsyncConnection database;
        public Registration()
        {
            InitializeComponent();
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
            database = new SQLiteAsyncConnection(path);
        }
        private  void SignUp_Clicked(object sender, EventArgs e)
        {
            Customers reg = new Customers();
            reg.Name = ContantPerson.Text;
            reg.mobile = MobileNumber.Text;
            reg.EMail = CompanyEmail.Text;
            reg.address = Address.Text;
            reg.PIN = PinCode.Text;
            reg.City = City.Text;
            reg.DLNO = DrugLicence.Text;
            reg.GSTNO = Gst.Text;
            reg.CrLimit = CreaditLimtExceed.Text;
            UserMaster masterdetails = new UserMaster();
            masterdetails.Username = UserName.Text;
            masterdetails.Password = Password.Text;
            masterdetails.ConfirmPassword = ConfirmPassword.Text;          
            masterdetails.mobile = MobileNumber.Text;
            masterdetails.UserType = RoleType.Text;
         
            if (masterdetails.Username == null)
            {
                DisplayAlert("Alert", "Please Enter UserName", "ok");
            }
            else if (masterdetails.Password == null)
            {
                DisplayAlert("Alert", "Please Enter Password", "ok");
            }
            else if (masterdetails.ConfirmPassword == null)
            {
                DisplayAlert("Alert", "Please Enter ConfirmPassword", "ok");

            }
           //else if (Password.Text == "" || ConfirmPassword.Text == "")
           // {
           //     DisplayAlert("Alert", "Please enter values", "ok");
           //     Password.Focus();
           //     return;
           // }
            else if (Password.Text != ConfirmPassword.Text)
            {
                DisplayAlert("Alert", "Password not matching", "ok");
                ConfirmPassword.Focus();
                return;
            }
            else if (masterdetails.mobile == null)
            {
                DisplayAlert("Alert", "Please Enter MobileNumber", "ok");
            }
            else if (MobileNumber.Text.Length != 10)
            {

                DisplayAlert("Alert", "mobile must be TEN NUMBERS", "ok");
            }
            else if(masterdetails.UserType ==null)
            {
                DisplayAlert("Alert", "Please Enter RoleType", "ok");
            }
            else if (reg.Name == null)
            {
                DisplayAlert("Alert", "Please Enter ContantPerson", "ok");
            }
            else if (reg.mobile == null)
            {
                DisplayAlert("Alert", "Please Enter MobileNumber", "ok");
            }
            else if (MobileNumber.Text.Length != 10)
            {

                DisplayAlert("Alert", "mobile must be Ten Numbers", "ok");
            }
            else if (reg.EMail == null)
            {
                DisplayAlert("Alert", "Please Enter CompanyEmail", "ok");
            }
            else if (Regex.Match("lartsoft@gmail.com", @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
            {
                DisplayAlert("Alert", "Please Enter valid CompanyEmail", "ok");
            }
            
            else if (reg.address == null)
            {
                DisplayAlert("Alert", "Please Enter Address", "ok");
            }
            else if (reg.PIN == null)
            {
                DisplayAlert("Alert", "please enter PinCode", "ok");
            }
           else if (PinCode.Text.Length != 6)
            {

                DisplayAlert("Alert", "Pincode must be six digits","ok");
            }
           
            else if (reg.City == null)
            {
                DisplayAlert("Alert", "Please Enter reg.Name", "ok");
            }
            else if (reg.DLNO == null)
            {
                DisplayAlert("Alert", "Please Enter DrugLicence", "ok");
            }
            else if (reg.GSTNO == null)
            {
                DisplayAlert("Alert", "Please Enter Gst", "ok");
            }
            else if (reg.CrLimit == null)
            {
                DisplayAlert("Alert", "Please Enter CreaditLimtExceed", "ok");
            }
            else
            {
                    Navigation.PushAsync(new LoginPage());
                try
                {
                    var DATA = Task.Run(async () => await database.InsertAsync(reg)).Result;
                    var DATA1 = Task.Run(async () => await database.InsertAsync(masterdetails)).Result;
                }
                catch (Exception ex)
                {


                }
                var list = Task.Run(async () => await database.Table<UserMaster>().ToListAsync()).Result;
                var list1 = Task.Run(async () => await database.Table<Customers>().ToListAsync()).Result;
            }
        }
        private void cancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
            }
        }

        private void ShopChemistName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblShopchemistname.IsVisible = false;
            }
            else
            {
                lblShopchemistname.IsVisible = true;
            }
        }

        private void ContantPerson_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblName.IsVisible = false;
            }
            else
            {
                lblName.IsVisible = true;
            }
        }

        private void MobileNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblMobileNumber.IsVisible = false;
            }
            else
            {
                lblMobileNumber.IsVisible = true;
            }
        }

        private void CompanyEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblCompanyEmail.IsVisible = false;
            }
            else
            {
                lblCompanyEmail.IsVisible = true;
            }
        }

        private void Address_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblAddress.IsVisible = false;
            }
            else
            {
                lblAddress.IsVisible = true;
            }
        }

        private void PinCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblPinCode.IsVisible = false;
            }
            else
            {
                lblPinCode.IsVisible = true;
            }
        }

        private void State_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblState.IsVisible = false;
            }
            else
            {
                lblState.IsVisible = true;
            }
        }

        private void City_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblCity.IsVisible = false;
            }
            else
            {
                lblCity.IsVisible = true;
            }
        }

        private void DrugLicence_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblDrugLicence.IsVisible = false;
            }
            else
            {
                lblDrugLicence.IsVisible = true;
            }
        }

        private void Gst_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblGst.IsVisible = false;
            }
            else
            {
                lblGst.IsVisible = true;
            }
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblUserName.IsVisible = false;
            }
            else
            {
                lblUserName.IsVisible = true;
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblPassword.IsVisible = false;
            }
            else
            {
                lblPassword.IsVisible = true;
            }
        }

        private void ConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblConfirmPassword.IsVisible = false;
            }
            else
            {
                lblConfirmPassword.IsVisible = true;
            }
        }

        private void RoleType_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblRoleType.IsVisible = false;
            }
            else
            {
                lblRoleType.IsVisible = true;
            }
        }

        private void CreaditLimtExceed_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = ((Entry)sender).Text;
            if (txt == "")
            {
                lblCreaditLimtExceed.IsVisible = false;
            }
            else
            {
                lblCreaditLimtExceed.IsVisible = true;
            }
        }
    }
}