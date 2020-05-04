using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private string[] gridViewString =
        {
            "ikona1", "ikona2", "ikona3", "ikona4", "ikona5", "ikona6"
        };

        private int[] imageId =
        {
            Resource.Drawable.icon1, Resource.Drawable.icon2, Resource.Drawable.icon3, Resource.Drawable.icon4,
            Resource.Drawable.icon5, Resource.Drawable.icon6
        };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.shopOffers, container, false);
            //ShopGridViewAdapter adapter = new ShopGridViewAdapter(this, gridViewString, imageId);
            gridView = view.FindViewById<GridView>(Resource.Id.grid_view_image_text);
            //gridView.Adapter = adapter;
            /*gridView.ItemClick += (s, e) =>
            {
                Toast.MakeText(this, "GridView Item: " + gridViewString[e.Position], ToastLength.Short).Show();
            };*/
            //powyższe linijki potrzebują dziedziczenia po AppCompatActivity
            ComponentsLocalizer();
            ActionHooker();
            return view;
        }
        private void ComponentsLocalizer()
        {
            picImageView = view.FindViewById<ImageView>(Resource.Id.shopImageView);
            nameTextView = view.FindViewById<TextView>(Resource.Id.shopTextView);
        }

        private void ActionHooker()
        {

        }
    }
}