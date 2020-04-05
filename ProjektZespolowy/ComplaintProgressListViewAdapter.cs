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
    class ComplaintProgressListViewAdapter : BaseAdapter<string>
    {
        public List<String> mlist;
        private Context mcontext;

        public ComplaintProgressListViewAdapter(Context context, List<String> list)
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

        public override string this[int position]
        {
            get { return mlist[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.complaint_progres_listview_row, null, false);
            }

            TextView id = row.FindViewById<TextView>(Resource.Id.textComplaintProgress);
            id.Text = mlist[position];
            return row;
        }
    }
}