using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Graphics.Bitmap;

namespace ProjektZespolowy.Fragments
{
    [Activity(Label = "Complaint", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ComplaintCreate : Android.Support.V4.App.Fragment
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

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == 0) && (data != null))
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                photoPreview.SetImageBitmap(bitmap);
            }
            if ((requestCode == 1) && (resultCode == 1) && (data != null))
            {
                Android.Net.Uri uri = data.Data;

                photoPreview.SetImageURI(uri);
            }
        }

        private void ActionHooker()
        {
            photoPreview.Click += delegate
            {
                this.Activity.Intent = new Intent();
                this.Activity.Intent.SetType("image/*");
                this.Activity.Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
            };
            btn1Complaint.Click += delegate
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, 1);
                StartActivityForResult(intent, 0);
            };
        }
    }
}