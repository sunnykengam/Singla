using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using SinglaApp.Droid.Renders;
using SinglaApp.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(DatePickerCtrl), typeof(DatePickerCtrlRenderer))]
namespace SinglaApp.Droid.Renders
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class DatePickerCtrlRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            GradientDrawable gd = new GradientDrawable();
            this.Control.SetTextColor(Android.Graphics.Color.ParseColor("#0000FF"));
            this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            this.Control.SetPadding(20, 0, 0, 0);
            this.Control.SetBackgroundDrawable(gd);

            gd.SetCornerRadius(10);
            gd.SetColor(Android.Graphics.Color.Transparent);
            gd.SetStroke(3, Android.Graphics.Color.ParseColor("#0000FF"));             
         

             DatePickerCtrl element = Element as DatePickerCtrl;
            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                Control.Text = element.Placeholder;
            }
            this.Control.TextChanged += (sender, arg) =>
            {
                var selectedDate = arg.Text.ToString();
                if (selectedDate == element.Placeholder)
                {
                    Control.Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
            };
        }

    }
#pragma warning restore CS0618 // Type or member is obsolete
}
