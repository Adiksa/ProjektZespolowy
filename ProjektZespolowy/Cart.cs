using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    [Activity(Label = "Cart", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Cart : Activity
    {
        private ListView listView;
        private ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.cart);
            base.OnCreate(savedInstanceState);
            ComponentsLocalizer();
            ActionHooker();
            refresh();
        }

        private void ActionHooker()
        {

        }

        private void ComponentsLocalizer()
        {
            listView = FindViewById<ListView>(Resource.Id.listViewCart);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBarCart);
        }
        private async void refresh()
        {
            progressBar.Visibility = ViewStates.Visible;
            await Task.Run(() =>
            {
                if (GlobalVars.cart != null)
                {
                    OrderListViewAdapter adapter = new OrderListViewAdapter(this, GlobalVars.cart);
                    RunOnUiThread(() => listView.Adapter = adapter);
                }
            });
            progressBar.Visibility = ViewStates.Invisible;

        }
    }
}