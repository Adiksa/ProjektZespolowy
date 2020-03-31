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
    public class Specification : Android.Support.V4.App.Fragment
    {
        private View view;
        private Button btnReturn;
        private ImageView photoPreview;
        private TextView description;
        public Furniture furniture;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.scan_successfull_view, container, false);
            ComponentsLocalizer();
            ActionHooker();
            description.Text = furniture.specText;
            photoPreview.SetImageBitmap(furniture.convertBase64ToBitmap(furniture.specImage));
            return view;
        }

        private void ComponentsLocalizer()
        {
            btnReturn = view.FindViewById<Button>(Resource.Id.btnReturn);
            photoPreview = view.FindViewById<ImageView>(Resource.Id.photoPreview);
            description = view.FindViewById<TextView>(Resource.Id.desc);
        }

        private void ActionHooker()
        {

        }

    }
}