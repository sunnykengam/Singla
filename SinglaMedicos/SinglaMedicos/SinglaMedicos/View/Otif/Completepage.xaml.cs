using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SinglaMedicos.View.Otif
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Completepage : ContentPage
	{
		public Completepage ()
		{
			InitializeComponent ();
		}
        private async void LogOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                progessbar.IsRunning = true;
                await Task.Delay(8);
                await Navigation.PushAsync(new MasterDetail(), true);
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            popupLoadingView.IsVisible = false;
            progessbar.IsRunning = false;
            Navigation.PushAsync(new MasterDetail());
            return true;
        }
    }
}