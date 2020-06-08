using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    public class ShopGridViewAdapter : BaseAdapter
    {
        private Context context;
        private List<Promotion> promotions;

        public ShopGridViewAdapter(Context context, List<Promotion> promotionslist)
        {
            this.context = context;
            if(promotionslist != null)
            {
                promotions = promotionslist;
            }
        }

        public override int Count
        {
            get { return promotions.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            if (convertView == null)
            {
                view = new View(context);
                view = inflater.Inflate(Resource.Layout.shopGrid, null);
                TextView txtView = view.FindViewById<TextView>(Resource.Id.shopTextView);
                ImageView imgView = view.FindViewById<ImageView>(Resource.Id.shopImageView);
                if(promotions[position].title != null) txtView.Text = promotions[position].title;
                if(promotions[position].convertBase64ToBitmap(promotions[position].image) != null) imgView.SetImageBitmap(promotions[position].convertBase64ToBitmap(promotions[position].image));
            }
            else
            {
                view = (View)convertView;

            }
            return view;
        }
    }
}