using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Graphics.Bitmap;

namespace ProjektZespolowy
{
    [Activity(Label = "Activity1")]
    public class PromotionAdd : Activity
    {
        private ImageView imagePromo;
        private EditText titlePromo;
        private EditText pricePromo;
        private EditText desc;
        private Button imageFromCamera;
        private Button imageFromGallery;
        private Button promotionBtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.promo_add);
            ComponentLocalizer();
            ActionHooker();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == 0) && (resultCode == Result.Ok) && (data != null))
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                imagePromo.SetImageBitmap(bitmap);
            }
            if ((requestCode == 1) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                imagePromo.SetImageURI(uri);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == 0 || requestCode == 1)
            {
                if (requestCode == 0)
                {
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this, GetString(Resource.String.noPermissions), ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        intent.PutExtra(MediaStore.ExtraOutput, 1);
                        StartActivityForResult(intent, 0);
                    }
                }
                if (requestCode == 1)
                {
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this, GetString(Resource.String.noPermissions), ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        this.Intent = new Intent();
                        this.Intent.SetType("image/*");
                        this.Intent.SetAction(Intent.ActionGetContent);
                        StartActivityForResult(Intent.CreateChooser(this.Intent, "Select picture"), 1);
                    }
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        private void ActionHooker()
        {
            imageFromGallery.Click += delegate
            {
                if (this.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted)
                {
                    this.Intent = new Intent();
                    this.Intent.SetType("image/*");
                    this.Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent.CreateChooser(this.Intent, "Select picture"), 1);
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.ReadExternalStorage };
                    RequestPermissions(requiredPermissions, 1);
                }
            };
            imageFromCamera.Click += delegate
            {
                if (this.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
                {
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    intent.PutExtra(MediaStore.ExtraOutput, 1);
                    StartActivityForResult(intent, 0);
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.Camera };
                    RequestPermissions(requiredPermissions, 0);
                }

            };
            promotionBtn.Click += PromotionBtn_Click;
        }

        private void PromotionBtn_Click(object sender, EventArgs e)
        {
            Promotion promotion = new Promotion()
            {
                desc = desc.Text,
                image = ImageViewToBase64String(imagePromo),
                title = titlePromo.Text,
                price = pricePromo.Text
            };
            if (promotion.Correct() && imagePromo.Drawable != GetDrawable(Resource.Drawable.ic6b_192x192))
            {
                FireBaseConnector connection = new FireBaseConnector();
                var res = connection.dataInsert(promotion);
                if(res == 1 && connection.connection)
                {
                    Toast.MakeText(this, GetString(Resource.String.added), ToastLength.Long).Show();
                    Finish();
                }
                else
                {
                    if(connection.connection==false)
                    {
                        Toast.MakeText(this, GetString(Resource.String.noInternetConnection), ToastLength.Long).Show();
                    }
                }
            }
            else
            {
                Toast.MakeText(this, GetString(Resource.String.addCorrectData), ToastLength.Long).Show();
            }
        }

        private void ComponentLocalizer()
        {
            imagePromo = FindViewById<ImageView>(Resource.Id.promotionPic);
            titlePromo = FindViewById<EditText>(Resource.Id.promoTitle);
            desc = FindViewById<EditText>(Resource.Id.promoDesc);
            pricePromo = FindViewById<EditText>(Resource.Id.pricePromo);
            imageFromCamera = FindViewById<Button>(Resource.Id.photoMake);
            imageFromGallery = FindViewById<Button>(Resource.Id.picSelect);
            promotionBtn = FindViewById<Button>(Resource.Id.btnAddPromo);
        }

        private string ImageViewToBase64String(ImageView obj)
        {
            Android.Graphics.Drawables.BitmapDrawable bd1 = (Android.Graphics.Drawables.BitmapDrawable)obj.Drawable;
            Bitmap bitmap = bd1.Bitmap;
            if (bitmap.Height > 1000 || bitmap.Width > 1000)
            {
                if (bitmap.Height > bitmap.Width)
                {
                    int newWidth = Convert.ToInt32((double)bitmap.Width / (double)bitmap.Height * 1000.0);
                    if (newWidth > 0)
                    {
                        bitmap = Bitmap.CreateScaledBitmap(bitmap, newWidth, 1000, true);
                    }
                }
                else
                {
                    int newHeight = Convert.ToInt32((double)bitmap.Height / (double)bitmap.Width * 1000.0);
                    if (newHeight > 0)
                    {
                        bitmap = Bitmap.CreateScaledBitmap(bitmap, 1000, newHeight, true);
                    }
                }
            }
            MemoryStream ms = new MemoryStream();
            bitmap.Compress(CompressFormat.Png, 100, ms);
            byte[] bb = ms.ToArray();
            return Convert.ToBase64String(bb);
        }
    }
}