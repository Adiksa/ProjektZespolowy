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
    class ComplaintListViewAdapter : BaseAdapter<Complaint>
    {
        public List<Complaint> mlist;
        private Context mcontext;

        public ComplaintListViewAdapter(Context context, List<Complaint> list)
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

        public override Complaint this[int position]
        {
            get { return mlist[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.complaint_listview_row, null, false);
            }

            TextView id = row.FindViewById<TextView>(Resource.Id.textComplaintId);
            id.Text = "Reklamacja nr: "+mlist[position].id;
            ImageView photo = row.FindViewById<ImageView>(Resource.Id.imageComplaint);
            photo.SetImageBitmap(mlist[position].convertBase64ToBitmap(mlist[position].photo));

            return row;
        }
    }
}