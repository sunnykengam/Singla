using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PickList.Droid.Renders;
using PickList.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FontAwesome), typeof(CustomLabelFontAwesome))]

namespace PickList.Droid.Renders
{

        public class CustomLabelFontAwesome : LabelRenderer
        {

            private static Context _context;
            public CustomLabelFontAwesome(Context cn) : base(cn)
            {
                _context = cn;
            }

            protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
            {
                // Context CurrentContext => CrossCurrentActivity.Current.Activity;

                //Current.ApplicationContext

                //var am = (AudioManager)ApplicationContext .GetSystemService(Context.AudioService);

                var am2 = (AudioManager)_context.GetSystemService(Context.AudioService);


                base.OnElementChanged(e);
                if (e.OldElement == null)
                {
                    Control.Typeface = Typeface.CreateFromAsset(Context.Assets, "FontAwesome.otf");
                }
            }
        }
    }
