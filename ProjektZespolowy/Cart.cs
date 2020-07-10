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
using ProjektZespolowy.Fragments;

namespace ProjektZespolowy
{
    [Activity(Label = "Cart", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Cart : BaseWithMenu
    {
        private ListView listView;
        private ProgressBar progressBar;
        private TextView orderTotal;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.base_with_menu);
            ComponentsLocalizer();
            ActionHooker();
            refresh();
        }

        protected override void ActionHooker()
        {
            base.ActionHooker();
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var transaction = FragmentManager.BeginTransaction();
            OfferFragment offerFragment = new OfferFragment();
            offerFragment.promotion = GlobalVars.cart[e.Position].product;
            offerFragment.order = GlobalVars.cart[e.Position];
            offerFragment.OfferChange += this.OnOfferChange;
            offerFragment.Show(transaction, "create offer dialog");
        }

        protected override void ComponentsLocalizer()
        {
            base.ComponentsLocalizer();
            stub.LayoutResource = Resource.Layout.cart;
            stub.Inflate();
            listView = FindViewById<ListView>(Resource.Id.listViewCart);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBarCart);
            orderTotal = FindViewById<TextView>(Resource.Id.cartTotal);
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
                else
                {
                    listView.Adapter = null;
                }
                RunOnUiThread(() => orderTotal.Text = GetString(Resource.String.orderPriceSum) + Order.Total(GlobalVars.cart) + "zł");
            });
            progressBar.Visibility = ViewStates.Invisible;

        }

        public async void OnOfferChange(object o, EventArgs e)
        {
            refresh();
        }
    }
}