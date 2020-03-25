using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        private CoordinatorLayout rootview;
        private NfcAdapter nfcAdapter;
        private TextView skanText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            ComponentLocalizer();
            Snackbar.Make(rootview, "Zalogowano pomyślnie.", Snackbar.LengthLong)
                        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
            skanText.Text = tag.ToString();
        }

        private void ComponentLocalizer()
        {
            rootview = FindViewById<CoordinatorLayout>(Resource.Id.coordinatorLayout1);
            skanText = FindViewById<TextView>(Resource.Id.skanText);
        }
    }
}

