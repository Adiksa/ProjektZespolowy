using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy.Fragments
{
    public class Complaint : Android.Support.V4.App.Fragment
    {
        private View view;
        private EditText problemDesc;
        private ImageView photoPreview;
        private Button btn1Complaint;
        private Button btn2Complaint;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.complaint, container, false);
            ComponentsLocalizer();
            ActionHooker();
            return view;
        }

        private void ComponentsLocalizer()
        {
            problemDesc = view.FindViewById<EditText>(Resource.Id.problemDesc);
            photoPreview = view.FindViewById<ImageView>(Resource.Id.photoPreview);
            btn1Complaint = view.FindViewById<Button>(Resource.Id.btn1Complaint);
            btn2Complaint = view.FindViewById<Button>(Resource.Id.btn2Complaint);
        }

        private void ActionHooker()
        {

        }
    }
}