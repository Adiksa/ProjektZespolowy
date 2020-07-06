using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    [Activity(Label = "WhishList", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class WhishList : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}