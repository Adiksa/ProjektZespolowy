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
    [Activity(Label = "Admin", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Admin : Activity
    {
        private Button nfcScanBtn;
        private Button warentyFromCamera;
        private Button warentyFromGallery;
        private Button specFromGalery;
        private Button specFromCamera;
        private Button furAdd;
        private TextView nfcText;
        private ImageView warentyImage;
        private ImageView specImage;
        private EditText warentyText;
        private EditText specText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.admin);
            ComponentLocalizer();
            ActionHooker();
            nfcText.Text = "Włącz skanowanie";
            CrossNFC.Init(this);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == 0)
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                warentyImage.SetImageBitmap(bitmap);
            }
            if (requestCode == 1)
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                specImage.SetImageBitmap(bitmap);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            CrossNFC.OnNewIntent(intent);
        }

        private void ActionHooker()
        {
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
            nfcScanBtn.Click += NfcScanBtn_Click;
            warentyFromCamera.Click += delegate
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                StartActivityForResult(intent, 0);
            };
            specFromCamera.Click += delegate
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                StartActivityForResult(intent, 1);
            };
            furAdd.Click += FurAdd_Click;
        }

        private void FurAdd_Click(object sender, EventArgs e)
        {
            Furniture furniture = new Furniture()
            {
                id = nfcText.Text,
                warentyImage = ImageViewToBase64String(warentyImage),
                warentyText = warentyText.Text,
                specImage = ImageViewToBase64String(specImage),
                specText = specText.Text
            };
            if(furniture.Correct())
            {
                FireBaseConnector connection = new FireBaseConnector();
                if (connection.checkFurniturePossibility(furniture)==0)
                {
                    
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alertDialog.SetTitle("Istnieje juz taki mebel");
                    alertDialog.SetMessage("Chcesz go nadpisać?");
                    alertDialog.SetPositiveButton("tak", delegate
                    {
                        if(connection.dataInsert(furniture)==-1)
                            Toast.MakeText(this, "Brak internetu.", ToastLength.Long).Show();
                        Toast.MakeText(this, "Dodano.", ToastLength.Long).Show();

                    });
                    alertDialog.SetNeutralButton("nie", delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
                if(connection.checkFurniturePossibility(furniture) == 1)
                {
                    if (connection.dataInsert(furniture) == -1)
                        Toast.MakeText(this, "Brak internetu.", ToastLength.Long).Show();
                    Toast.MakeText(this, "Dodano.", ToastLength.Long).Show();
                }
                if (connection.checkFurniturePossibility(furniture) == 1)
                {
                    Toast.MakeText(this, "Brak internetu.", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Dodaj wszystkie dane poprawnie.", ToastLength.Long).Show();
            }
        }

        private void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            Vibrator vibrator = (Vibrator)this.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(100);
            var memory = tagInfo.Records.AsMemory();
            if (memory.Span.ToArray().Length > 0)
            {
                nfcText.Text = memory.Span.ToArray()[0].MimeType;
            }
            else
            {
                Toast.MakeText(this, "Bład czytania NFC", ToastLength.Long).Show();
            }
        }

        private void NfcScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                CrossNFC.Current.StartListening();
                nfcText.Text = "Skanowanie";
            }
            catch
            {
                nfcText.Text = "Błąd NFC";
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
            nfcText = FindViewById<TextView>(Resource.Id.communicate);
            warentyImage = FindViewById<ImageView>(Resource.Id.photo1);
            specImage = FindViewById<ImageView>(Resource.Id.photo2);
            warentyText = FindViewById<EditText>(Resource.Id.editText1);
            specText = FindViewById<EditText>(Resource.Id.txtEdit2);
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