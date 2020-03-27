using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy.Fragments
{
    [Activity(Label = "Scan_Success_View", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Scan_Success_View : Android.Support.V4.App.Fragment
    {
        private View view;
        private TextView itemName;
        private ImageView photoPreview;
        private TextView description;
        public Furniture furniture;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.scan_successfull_view,container,false);
            ComponentsLocalizer();
            ActionHooker();
            description.Text = furniture.warentyText;
            itemName.Text = furniture.name;
            photoPreview.SetImageBitmap(furniture.convertBase64ToBitmap(furniture.warentyImage));
            return view;
        }

        private void ComponentsLocalizer()
        {
            itemName = view.FindViewById<TextView>(Resource.Id.itemName);
            photoPreview = view.FindViewById<ImageView>(Resource.Id.photoPreview);
            description = view.FindViewById<TextView>(Resource.Id.desc);
        }

        private void ActionHooker()
        {

        }

    }
}