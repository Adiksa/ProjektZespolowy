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
        private string[] gridViewString;
        private int[] gridViewImage;

        public ShopGridViewAdapter(Context context, string[] gridViewstr, int[] gridViewImage)
        {
            this.context = context;
            gridViewString = gridViewstr;
            this.gridViewImage = gridViewImage;
        }

        public override int Count
        {
            get { return gridViewString.Length; }
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
                txtView.Text = gridViewString[position];
                imgView.SetImageResource(gridViewImage[position]);
            }
            else
            {
                view = (View)convertView;

            }
            return view;
        }
    }
}