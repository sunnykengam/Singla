using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace SinglaApp.Renders
{

    public class FontAwesome : Label
    {
        //Must match the exact "Name" of the font which you can get by double clicking the TTF in Windows
        private const string Typeface = "FontAwesome";

        public FontAwesome()
        {
            FontFamily = Typeface;
        }

        public FontAwesome(string fontAwesomeLabel = null)
        {
            FontFamily = Typeface;    //iOS is happy with this, Android needs a renderer to add ".ttf"
            Text = fontAwesomeLabel;
        }
    }

    public static class Icon
    {
        public static readonly string FASpinner = "\uf110";
        public static readonly string FARefresh = "\uf021";
        public static readonly string FASignOut = "\uf08b";
        public static readonly string FAOpencart = "\uf217";
        public static readonly string FACog = "\uf013";
        public static readonly string FAUser = "\uf007";
        public static readonly string FAUserS = "\uf0c0";
        public static readonly string FAMapMarker = "\uf041";
        public static readonly string FAEnvelope = "\uf0e0";
        public static readonly string FAPhoneSquare = "\uf098";
        public static readonly string FATruck = "\uf0d1";
        public static readonly string FARepeat = "\uf021";
        public static readonly string FAClipboard = "\uf46d";
        public static readonly string FAPending = "\uf0c9";
        public static readonly string FAInventory = "\uf022";
        public static readonly string FAinvoicedollar = "\uf0d6";
        //public static readonly string FAReport = "\uf188";
        public static readonly string FAThList = "\uf0ca";
        public static readonly string FAAngellist = "\uf0cb";
        public static readonly string FAFeedback = "\uf0e6";
        public static readonly string FAArrowLeft = "\uf060";
        public static readonly string FASearch = "\uf002";
        public static readonly string FAFileText = "\uf15c";
        public static readonly string FACodepen = "\uf1cb";
        public static readonly string FASitemap = "\uf0e8";
        public static readonly string FAQrcode = "\uf029";
        public static readonly string FASortNumericAsc = "\uf162";
        public static readonly string FACalendar = "\uf073";



        public static readonly string FAHistory = "\uf1da";

        public static readonly string FAFilter = "\uf0b0";
    }
}
