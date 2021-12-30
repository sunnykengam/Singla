using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SinglaApp.Common;
using SinglaApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaApp.View.Otif
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InvoiceDetails : ContentPage
    {
       
        public SQLiteAsyncConnection database;
        byte[] bitmapData;
        public string imagepath = string.Empty;
        // ObservableCollection<AppPartner> partnerlist = new ObservableCollection<AppPartner>();
        public List<object> chkboxes = new List<object>();
        public List<DeliveryItems> listitems = new List<DeliveryItems>();
        double pagewith = 0;
        public InvoiceDetails()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            var path = DependencyService.Get<IFileHelper>().GetLocalFilePath("Singla.Sqlite");
            database = new SQLiteAsyncConnection(path);
            List<DeliveryStatus> list = new List<DeliveryStatus>();
            int i = 1;
            AppSettings.Imagepath = string.Empty;
            //int showcode = Convert.ToInt32(AppSettings.Customercode);
            //var invoicedetails = Task.Run(async () => await App.ServiceClient.GetTable<SinglaApp.Azure.AppcustInvoice>().Where(j => j.custcode == AppSettings.Customercode && j.DlvryStatus == null).OrderByDescending(k => k.invdate).ToListAsync()).Result.ToList();
            //if (invoicedetails.Count != 0)
            //{
            //    AppSettings.Invoicedetails = invoicedetails;
            //    AppSettings.partnercode = Convert.ToInt32(invoicedetails[0].partnercode);
            //    try
            //    {
            //        ptrname.Text = Task.Run(async () => await database.Table<Partner>().Where(k => k.Code == AppSettings.partnercode).ToListAsync()).Result.ToList().FirstOrDefault().ShortName;
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    sthidelayout.IsVisible = true;
            //    try
            //    {
            //        foreach (SinglaApp.Azure.AppcustInvoice item in invoicedetails)
            //        {
            //            DeliveryItems Data = new DeliveryItems();
            //            Data.sno = i.ToString();
            //            Data.custcode = Convert.ToString(item.custcode);
            //            Data.custname = item.custname;
            //            Data.Address = item.address;
            //            Data.Address1 = item.address1;
            //            Data.Address2 = item.address2;
            //            Data.Address3 = item.address3;
            //            Data.Address4 = item.address4;
            //            Data.NoofItems = item.noofitems.ToString();
            //            Data.TotalAmount = item.invamt.ToString();
            //            Data.InVoicedate = item.invdate.Date.ToString().Replace(" ", string.Empty).Replace("00", string.Empty).Replace(":", string.Empty);
            //            Data.InvocieNo = item.invnum.ToString();
            //            Data.IsChicked = false;
            //            i++;
            //            listitems.Add(Data);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    lst.ItemsSource = listitems;
            //    try
            //    {
            //        takePhoto.Clicked += async (sender, args) =>
            //        {
            //            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //            {
            //                await DisplayAlert("No Camera", ":( No camera available.", "OK");
            //                return;
            //            }
            //            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //            {
            //                Directory = "OTIF",
            //                SaveToAlbum = false,
            //                CompressionQuality = 30,
            //                Name = string.Format(AppSettings.Customercode + "_{0}", DateTime.Now.ToString("yy/MM/dd/hh:mm:ss")),
            //                CustomPhotoSize = 20,
            //                PhotoSize = PhotoSize.MaxWidthHeight,
            //                MaxWidthHeight = 400,

            //                DefaultCamera = CameraDevice.Rear
            //            });
            //            if (file == null)
            //                return;

            //            imagepath = file.Path;
            //            Stream filestrm = null;
            //        };
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
            //else
            //{
            //    Msgstacklayout.IsVisible = true;
            //}
        }


        protected override void OnSizeAllocated(double width, double height)
        {
            try
            {
                base.OnSizeAllocated(width, height);
                Device.OnPlatform(Android: () =>
                {
                    if (height >= 1000)
                        Mainstacklayout.HeightRequest = 650;
                    else if (height >= 900)

                        Mainstacklayout.HeightRequest = 550;
                    else if (height >= 800)

                        Mainstacklayout.HeightRequest = 600;
                    else if (height >= 700)

                        Mainstacklayout.HeightRequest = 440;
                    else if (height >= 600)

                        Mainstacklayout.HeightRequest = 350;
                    else if (height >= 500)

                        Mainstacklayout.HeightRequest = 300;

                    else if (height >= 400)

                        Mainstacklayout.HeightRequest = 200;
                    else if (height >= 300)

                        Mainstacklayout.HeightRequest = 125;

                    else if (height >= 250)

                        Mainstacklayout.HeightRequest = 125;
                },
                iOS: () => { Mainstacklayout.HeightRequest = 400; }
                );
            }
            catch (Exception ex)
            {
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        private void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                var Item = ((Xamarin.Forms.CheckBox)sender).BindingContext;
                if (!chkboxes.Contains(Item))
                    chkboxes.Add(Item);
                else if (chkboxes.Contains(Item))
                {
                    chkboxes.Remove(Item);
                }
            }
            catch (Exception ex)
            {
            }
        }

       
        private void CheckBox_CheckedChanged_1(object sender, XLabs.EventArgs<bool> e)
        {
            try
            {
                var Item = ((Xamarin.Forms.CheckBox)sender);
                List<DeliveryItems> listitemDetail = new List<DeliveryItems>();
                if (Item.IsChecked == true)
                {
                    foreach (DeliveryItems obj in listitems)
                    {
                        obj.IsChicked = true;
                        chkboxes.Add(obj);
                        listitemDetail.Add(obj);
                    }
                }
                else
                {
                    foreach (DeliveryItems obj in listitems)
                    {
                        obj.IsChicked = false;
                        chkboxes.Remove(obj);
                        listitemDetail.Add(obj);
                    }
                }
                lst.ItemsSource = listitemDetail;
            }
            catch (Exception ex)
            {
            }
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayAlert("Exit?", "Are you sure? Do you want to Logout", "Yes", "No");
                if (action)
                {
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                    await Navigation.PushAsync(new MasterDetail());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            popupLoadingView.IsVisible = true;
            progessbar.IsRunning = true;
            await Task.Delay(8);
            try
            {
                if (chkboxes.Count != 0)
                {
                    if (!string.IsNullOrEmpty(imagepath))
                    {
                        using (FileStream stream = File.Open(imagepath, FileMode.Open))
                        {
                            bitmapData = ReadFully(stream);
                        }
                        if (bitmapData != null)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                if (chkboxes.Count != 0 && !String.IsNullOrEmpty(Numberofboxes.Text) && bitmapData != null)
                                {
                                    List<String> stringList = new List<String>();
                                    AppcustInvoice Insertdata = new AppcustInvoice();
                                    string invoicedetail = string.Empty;
                                    string invoicedate = string.Empty;
                                    string custname = string.Empty;
                                    string invodate = string.Empty;
                                    insertinvoiceinfo(Insertdata, ref invoicedetail, ref invoicedate, ref custname, ref invodate);
                                    Insertdelvryinfo(Insertdata, invoicedate, custname, invoicedetail,bitmapData);
                                   
                                   
                                    if (chkboxes.Count != 0 && !String.IsNullOrEmpty(Numberofboxes.Text) && bitmapData != null)
                                    {
                                        try
                                        {
                                            await Navigation.PushAsync(new MasterDetail(), true);
                                            Numberofboxes.Text = string.Empty;
                                            NumberofPacks.Text = string.Empty;
                                        }
                                        finally
                                        {
                                            popupLoadingView.IsVisible = false;
                                            progessbar.IsRunning = false;
                                        }
                                    }
                                }
                                else if (chkboxes.Count == 0)
                                {
                                    await DisplayAlert("Alert:", "Please Select The CheckBoxes", "OK");
                                    popupLoadingView.IsVisible = false;
                                    progessbar.IsRunning = false;
                                }
                                else if (String.IsNullOrEmpty(Numberofboxes.Text))
                                {
                                    await DisplayAlert("Alert:", "Please Enter Number of Boxes", "OK");
                                    popupLoadingView.IsVisible = false;
                                    progessbar.IsRunning = false;
                                }
                                else if (String.IsNullOrEmpty(NumberofPacks.Text))
                                {
                                    await DisplayAlert("Alert:", "Please Please Number of Packes", "OK");
                                    popupLoadingView.IsVisible = false;
                                    progessbar.IsRunning = false;
                                }
                                else if (bitmapData == null)
                                {
                                    await DisplayAlert("Alert:", "Please Take Photo", "OK");
                                    popupLoadingView.IsVisible = false;
                                    progessbar.IsRunning = false;
                                }
                            }
                            else
                            {
                                await DisplayAlert("Alert:", "Please Check Your InterNet Connection", "Ok");
                                popupLoadingView.IsVisible = false;
                                progessbar.IsRunning = false;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Alert:", "Please Take Photo", "OK");
                            popupLoadingView.IsVisible = false;
                            progessbar.IsRunning = false;
                        }

                    }
                    else
                    {
                        await DisplayAlert("Alert:", "Please Take Photo", "OK");
                        popupLoadingView.IsVisible = false;
                        progessbar.IsRunning = false;
                    }
                }
                else
                {
                    await DisplayAlert("Alert:", "Please Select Invoice", "Ok");
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert:", "Please Contact the Admin " + ex.Message, "Ok");
                popupLoadingView.IsVisible = false;
                progessbar.IsRunning = false;
            }
        }
        private void insertinvoiceinfo(AppcustInvoice Insertdata, ref string invoicedetail, ref string invoicedate, ref string custname, ref string invodate)
        {
            try
            {
                foreach (object data in chkboxes)
                {
                    string InvocieNo = Convert.ToString(data.GetType().GetProperty("InvocieNo").GetValue(data, null));
                    Insertdata.custcode = Convert.ToString(data.GetType().GetProperty("custcode").GetValue(data, null));
                   
                    //var CustinvoiceDetail = Task.Run(async () => await App.ServiceClient.GetTable<SinglaApp.Azure.AppcustInvoice>().Where(i => i.invnum == InvocieNo).ToListAsync()).Result;
                    //if (CustinvoiceDetail.Count != 0 && CustinvoiceDetail[0].DlvryStatus == null)
                    //{
                    //    invoicedetail = invoicedetail + CustinvoiceDetail[0].invnum + ",";
                    //    invoicedate = invoicedate + CustinvoiceDetail[0].invdate + ",";

                    //    CustinvoiceDetail[0].DlvryStatus = "y";
                    //    CustinvoiceDetail[0].DlvryDate = DateTime.Now.ToString("MM/dd/yyyy");
                    //    CustinvoiceDetail[0].DlvryTime = DateTime.Now.ToString("HH:mm:ss");
                    //    try
                    //    {
                    //        //App.ServiceClient.GetTable<SinglaApp.Azure.AppcustInvoice>().UpdateAsync(CustinvoiceDetail[0]);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool DeleteFileByPath(string filePath)
        {
            try
            {
                File.Delete(filePath);

                if (File.Exists(filePath))
                    return false;//not deleted
                else
                    return true;//deleted
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async void Insertdelvryinfo(AppcustInvoice Insertdata, string invoicedate, string custname, string invoicedetail,byte[] bitmapData)
        {
            try
            {
                //SinglaApp.Azure.AppCustDlvry InsertFields = new SinglaApp.Azure.AppCustDlvry();
                //InsertFields.Custcode =Convert.ToInt32(AppSettings.custdetail.customercode);
                //InsertFields.Custname = AppSettings.custdetail.customername;
                //InsertFields.NoofBoxes = Numberofboxes.Text;
                //InsertFields.Noofpackes = NumberofPacks.Text;
                //InsertFields.DlvryDate = DateTime.Now.ToString("MM/dd/yyyy");
                //InsertFields.DlvryTime = DateTime.Now.ToString("HH:mm:ss");
                //InsertFields.InvoiceDate = invoicedate.TrimEnd(',');
                //InsertFields.InvoiceImage = AzureStorage.UploadFileAsync(ContainerType.photos, new MemoryStream(bitmapData));
                //InsertFields.Photo = imagepath.Substring(imagepath.LastIndexOf('/'));
                //InsertFields.CustAltcode = Insertdata.custcode.ToString();
                //InsertFields.InvoiceDeltails = invoicedetail.TrimEnd(',');
                //InsertFields.CreatedDate = Convert.ToDateTime(DateTime.Now);
                //InsertFields.patnercode = AppSettings.partnercode;
                //string delvrycode = Guid.NewGuid().ToString();
                //InsertFields.DlvryCode = delvrycode;
                //DeleteFileByPath(imagepath);
                //await App.ServiceClient.GetTable<SinglaApp.Azure.AppCustDlvry>().InsertAsync(InsertFields);
            }
            catch (Exception ex)
            {
            }
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                //var action = await DisplayAlert("Exit?", "Are you sure to MasterDetail", "Yes", "No");
                //if (action)
                //{
                    popupLoadingView.IsVisible = false;
                    progessbar.IsRunning = false;
                    await Navigation.PushAsync(new MasterDetail());
               // }
            }
            catch (Exception ex)
            {
            }
        }
    }
}