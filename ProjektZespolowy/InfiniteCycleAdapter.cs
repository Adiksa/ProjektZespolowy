using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    class InfiniteCycleAdapter : PagerAdapter
    {
        List<string> list;
        Context context;
        LayoutInflater layoutInflater;

        public InfiniteCycleAdapter(List<string> list, Context context)
        {
            this.list = list;
            this.context = context;
            layoutInflater = LayoutInflater.From(context);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View view = layoutInflater.Inflate(Resource.Layout.card_item, container, false);
            ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageViewCardItem);
            imageView.SetImageBitmap(Complaint.convertBase64ToBitmap(list[position]));
            container.AddView(view);
            return view;
        }

        public override int Count => list.Count;

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view.Equals(@object);
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            container.RemoveView((View)@object);
        }
    }
}