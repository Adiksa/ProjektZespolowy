using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy.Fragments
{
    public class Shop : Android.Support.V4.App.Fragment
    {
        private View view;
        private ImageView picImageView;
        private TextView nameTextView;
        private GridView gridView;
        private List<Promotion> promotions;
        private ProgressBar progressBar;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.shopOffers, container, false);
            ComponentsLocalizer();
            ActionHooker();
            return view;
        }

        public override async void OnResume()
        {
            PromotionLoad();
            base.OnResume();
        }
        private void ComponentsLocalizer()
        {
            picImageView = view.FindViewById<ImageView>(Resource.Id.shopImageView);
            nameTextView = view.FindViewById<TextView>(Resource.Id.shopTextView);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.ShopProgressBar);
        }

        private void ActionHooker()
        {
            
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            gridView.ItemClick -= GridView_ItemClick;
            var transaction = this.Activity.FragmentManager.BeginTransaction();
            OfferFragment offerFragment = new OfferFragment();
            offerFragment.promotion = promotions[e.Position];
            offerFragment.OfferChange += this.OnOfferChange;
            offerFragment.UnlockGridView += this.UnlockGridView;
            offerFragment.Show(transaction, "create offer dialog");
        }

        private async Task PromotionLoad()
        {
            progressBar.Visibility = ViewStates.Visible;
            await Task.Run(() =>
            {
                try
                {
                    FireBaseConnector connector = new FireBaseConnector();
                    promotions = connector.GetPromotions();
                    if (promotions != null)
                    {
                        ShopGridViewAdapter adapter = new ShopGridViewAdapter(this.Activity.BaseContext, promotions);
                        gridView = view.FindViewById<GridView>(Resource.Id.grid_view_image_text);
                        this.Activity.RunOnUiThread(() => { gridView.Adapter = adapter; });
                        gridView.ItemClick += GridView_ItemClick;
                    }
                }
                catch
                {

                }
            });
            progressBar.Visibility = ViewStates.Invisible;
        }

        public async void OnOfferChange(object o, EventArgs e)
        {
            FireBaseConnector connector = new FireBaseConnector();
            promotions = connector.GetPromotions();
        }
        public void UnlockGridView(object o, EventArgs e)
        {
            if(gridView != null)
            {
                gridView.ItemClick += GridView_ItemClick;
            }
        }
    }
}