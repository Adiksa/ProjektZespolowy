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
    class OrderListViewAdapter : BaseAdapter<Promotion>
    {
        public List<Promotion> mlist;
        private Context mcontext;
        public OrderListViewAdapter(Context context, List<Promotion> list)
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

        public override Promotion this[int position]
        {
            get { return mlist[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.whish_list_row, null, false);
            }

            TextView name = row.FindViewById<TextView>(Resource.Id.WhishListRowPromotionName);
            name.Text = mlist[position].title;
            TextView price = row.FindViewById<TextView>(Resource.Id.WhishListRowPromotionPrice);
            price.Text = mlist[position].price + "zł";

            return row;
        }
    }
}