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
    class OrderListViewAdapter : BaseAdapter<Order>
    {
        public List<Order> mlist;
        private Context mcontext;
        public OrderListViewAdapter(Context context, List<Order> list)
        {
            mlist = list;
            mcontext = context;
        }
        public override int Count
        {
            get { return mlist.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Order this[int position]
        {
            get { return mlist[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.order_row, null, false);
            }
            ImageView image = row.FindViewById<ImageView>(Resource.Id.orderPhoto);
            image.SetImageBitmap(mlist[position].product.convertBase64ToBitmap(mlist[position].product.image));
            TextView name = row.FindViewById<TextView>(Resource.Id.orderName);
            name.Text = mlist[position].product.title;
            TextView price = row.FindViewById<TextView>(Resource.Id.orderPrice);
            price.Text = mlist[position].amount.ToString() + "x" + mlist[position].product.price + "=" + mlist[position].price + "zl";
            return row;
        }
    }
}