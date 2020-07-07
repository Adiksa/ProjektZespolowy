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
    [Activity(Label = "WhishList", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class WhishList : Activity
    {
        private ListView listView;
        private List<Promotion> whishList;
        private ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.whish_list);
            base.OnCreate(savedInstanceState);
            ComponentsLocalizer();
            ActionHooker();
            refresh();
        }

        private void ActionHooker()
        {
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var transaction = FragmentManager.BeginTransaction();
            OfferFragment offerFragment = new OfferFragment();
            offerFragment.promotion = whishList[e.Position];
            offerFragment.OfferChange += this.OnOfferChange;
            offerFragment.Show(transaction, "create offer dialog");
        }

        private void ComponentsLocalizer()
        {
            listView = FindViewById<ListView>(Resource.Id.listViewWhishList);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBarWhishList);
        }

        private async void refresh ()
        {
            progressBar.Visibility = ViewStates.Visible;
            await Task.Run(() =>
            {
                FireBaseConnector connector = new FireBaseConnector();
                whishList = connector.GetWhishList(GlobalVars.login);
                if (whishList != null)
                {
                    PromotionListViewAdapter adapter = new PromotionListViewAdapter(this, whishList);
                    RunOnUiThread(() => listView.Adapter = adapter);
                }
            });
            progressBar.Visibility = ViewStates.Invisible;
        }

        public async void OnOfferChange(object o, EventArgs e)
        {
            refresh();
        }
    }
}