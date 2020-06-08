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
using Plugin.NFC;
using static Android.Graphics.Bitmap;

namespace ProjektZespolowy
{
    ///Activity zrobione do testowania dodawania nowych tagów. 
    [Activity(Label = "Admin", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Admin : Activity
    {
        private Button nfcScanBtn;
        private Button warentyFromCamera;
        private Button warentyFromGallery;
        private Button specFromGalery;
        private Button specFromCamera;
        private Button promotionBtn;
        private Button furAdd;
        private TextView nfcText;
        private ImageView warentyImage;
        private ImageView specImage;
        private EditText warentyText;
        private EditText specText;
        private EditText name;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.admin);
            ComponentLocalizer();
            ActionHooker();
            nfcText.Text = GetString(Resource.String.scanON);
            CrossNFC.Init(this);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if((requestCode == 0) && (resultCode == Result.Ok) && (data != null))
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                warentyImage.SetImageBitmap(bitmap);
            }
            if ((requestCode == 1) && (resultCode == Result.Ok) && (data != null))
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                specImage.SetImageBitmap(bitmap);
            }
            if ((requestCode == 2) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                
                warentyImage.SetImageURI(uri);
            }
            if ((requestCode == 3) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;

                specImage.SetImageURI(uri);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            CrossNFC.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode>=0&&requestCode<4)
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
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        intent.PutExtra(MediaStore.ExtraOutput, 1);
                        StartActivityForResult(intent, 1);
                    }
                }
                if (requestCode == 2)
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
                        StartActivityForResult(Intent.CreateChooser(this.Intent, "Select picture"), 2);
                    }
                }
                if (requestCode == 3)
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
                        StartActivityForResult(Intent.CreateChooser(this.Intent, "Select picture"), 3);
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
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            nfcScanBtn.Click += NfcScanBtn_Click;
            warentyFromCamera.Click += delegate
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
            specFromCamera.Click += delegate
            {
                if (this.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
                {
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    intent.PutExtra(MediaStore.ExtraOutput, 1);
                    StartActivityForResult(intent, 1);
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.Camera };
                    RequestPermissions(requiredPermissions, 1);
                }
            };
            warentyFromGallery.Click += delegate
            {
                if (this.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted)
                {
                    this.Intent = new Intent();
                    this.Intent.SetType("image/*");
                    this.Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent.CreateChooser(this.Intent, "Select picture"), 2);
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.ReadExternalStorage };
                    RequestPermissions(requiredPermissions, 2);
                }
            };
            specFromGalery.Click += delegate
            {
                if (this.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted)
                {
                    this.Intent = new Intent();
                    this.Intent.SetType("image/*");
                    this.Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent.CreateChooser(this.Intent, "Select picture"), 3);
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.ReadExternalStorage };
                    RequestPermissions(requiredPermissions, 3);
                }
            };
            furAdd.Click += FurAdd_Click;
            promotionBtn.Click += delegate
            {
                StartActivity(typeof(PromotionAdd));
            };
        }
        private void FurAdd_Click(object sender, EventArgs e)
        {
            Furniture furniture = new Furniture()
            {
                id = nfcText.Text,
                warentyImage = ImageViewToBase64String(warentyImage),
                warentyText = warentyText.Text,
                specImage = ImageViewToBase64String(specImage),
                specText = specText.Text,
                name = name.Text
            };
            if(furniture.Correct())
            {
                FireBaseConnector connection = new FireBaseConnector();
                if (connection.checkFurniturePossibility(furniture)==0)
                {
                    
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alertDialog.SetTitle(GetString(Resource.String.furnitureExists));
                    alertDialog.SetIcon(Resource.Drawable.ic4b_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.overwrite));
                    alertDialog.SetPositiveButton(GetString(Resource.String.yes), delegate
                    {
                        if(connection.dataInsert(furniture)==-1)
                            Toast.MakeText(this, GetString(Resource.String.noInternetConnection), ToastLength.Long).Show();
                        Toast.MakeText(this, GetString(Resource.String.added), ToastLength.Long).Show();

                    });
                    alertDialog.SetNeutralButton(GetString(Resource.String.no), delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
                if(connection.checkFurniturePossibility(furniture) == 1)
                {
                    if (connection.dataInsert(furniture) == -1)
                        Toast.MakeText(this, GetString(Resource.String.noInternetConnection), ToastLength.Long).Show();
                    Toast.MakeText(this, GetString(Resource.String.added), ToastLength.Long).Show();
                }
                if (connection.checkFurniturePossibility(furniture) == 1)
                {
                    Toast.MakeText(this, GetString(Resource.String.noInternetConnection), ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, GetString(Resource.String.addCorrectData), ToastLength.Long).Show();
            }
        }

        private void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            Vibrator vibrator = (Vibrator)this.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(100);
            var memory = tagInfo.Records.AsMemory();
            if (memory.Span.ToArray().Length > 0)
            {
                nfcText.Text = memory.Span.ToArray()[0].Message;
            }
            else
            {
                Toast.MakeText(this, GetString(Resource.String.errorReadNFC), ToastLength.Long).Show();
            }
        }

        private void NfcScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                CrossNFC.Current.StartListening();
                nfcText.Text = GetString(Resource.String.scanDesc);
            }
            catch
            {
                nfcText.Text = GetString(Resource.String.errorNFC);
            }
        }

        private void ComponentLocalizer()
        {
            nfcScanBtn = FindViewById<Button>(Resource.Id.scan);
            warentyFromCamera = FindViewById<Button>(Resource.Id.camera);
            warentyFromGallery = FindViewById<Button>(Resource.Id.gallery);
            specFromCamera = FindViewById<Button>(Resource.Id.btn4);
            specFromGalery = FindViewById<Button>(Resource.Id.btn5);
            furAdd = FindViewById<Button>(Resource.Id.furAdd);
            promotionBtn = FindViewById<Button>(Resource.Id.promotion);
            nfcText = FindViewById<TextView>(Resource.Id.communicate);
            warentyImage = FindViewById<ImageView>(Resource.Id.photo1);
            specImage = FindViewById<ImageView>(Resource.Id.photo2);
            warentyText = FindViewById<EditText>(Resource.Id.editText1);
            specText = FindViewById<EditText>(Resource.Id.txtEdit2);
            name = FindViewById<EditText>(Resource.Id.txtEdit3);
        }

        private string ImageViewToBase64String(ImageView obj)
        {
            Android.Graphics.Drawables.BitmapDrawable bd1 = (Android.Graphics.Drawables.BitmapDrawable)obj.Drawable;
            Bitmap bitmap = bd1.Bitmap;
            MemoryStream ms = new MemoryStream();
            bitmap.Compress(CompressFormat.Png, 100, ms);
            byte[] bb = ms.ToArray();
            return Convert.ToBase64String(bb);
        }
    }
}