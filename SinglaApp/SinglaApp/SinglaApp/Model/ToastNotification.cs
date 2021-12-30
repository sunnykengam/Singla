using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinglaApp.Model
{
    public static class ToastNotification
    {
        public static void TostMessage(string message)
        {
            var context = Android.App.Application.Context;
            var tostMessage = message;
            var durtion = ToastLength.Long;
            Toast.MakeText(context, tostMessage, durtion).Show();
        }
    }
}
