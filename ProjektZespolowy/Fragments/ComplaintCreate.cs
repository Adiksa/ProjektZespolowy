using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using static Android.Graphics.Bitmap;

namespace ProjektZespolowy.Fragments
{
    public class ComplaintCreate : DialogFragment
    {
        private View view;
        private EditText problemDesc;
        private ImageView photoPreview;
        private Button btn1Complaint;
        private Button btn2Complaint;
        public Furniture furniture;
        public event EventHandler ComplaintCreated;
        private Drawable photoDefault;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.complaint, container, false);
            ComponentsLocalizer();
            photoDefault = photoPreview.Drawable;
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

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == 0) && (resultCode == Result.Ok) && (data != null))
            {
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                photoPreview.SetImageBitmap(bitmap);
            }
            if ((requestCode == 1) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;

                photoPreview.SetImageURI(uri);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if(requestCode == 0 || requestCode == 1)
            {
                if(requestCode==0)
                {
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this.Activity, Resource.String.noPermissions, ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        intent.PutExtra(MediaStore.ExtraOutput, 1);
                        StartActivityForResult(intent, 0);
                    }
                }
                if(requestCode==1)
                {
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this.Activity, Resource.String.noPermissions, ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        this.Activity.Intent = new Intent();
                        this.Activity.Intent.SetType("image/*");
                        this.Activity.Intent.SetAction(Intent.ActionGetContent);
                        StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
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
            photoPreview.Click += delegate
            {
                if (this.Activity.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted)
                {
                    this.Activity.Intent = new Intent();
                    this.Activity.Intent.SetType("image/*");
                    this.Activity.Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.ReadExternalStorage };
                    RequestPermissions(requiredPermissions, 1);
                }
            };
            btn1Complaint.Click += delegate
            {
                if (this.Activity.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
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
            btn2Complaint.Click += delegate
            {
                btn2Complaint.Enabled = false;
                Complaint complaint = new Complaint()
                {
                    furnitureId = furniture.id,
                    description = problemDesc.Text,
                    photo = ImageViewToBase64String(photoPreview)
                };
                if (complaint.Correct() && photoDefault!=photoPreview.Drawable)
                {
                    FireBaseConnector connector = new FireBaseConnector();
                    var res = connector.dataInsert(complaint);
                    if (res == 0)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.dataError));
                        alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.addCorrect));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if (res == -1)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.noInternetConnection));
                        alertDialog.SetIcon(Resource.Drawable.ic5c_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.checkConnection));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if (res == 1)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.complaintCorrect));
                        alertDialog.SetIcon(Resource.Drawable.ok2a_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.complaintMessage));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                            this.OnComplaintCreated();
                            this.Dismiss();
                        });
                        alertDialog.Show();

                    }
                }
                else
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                    alertDialog.SetTitle(GetString(Resource.String.dataError));
                    alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.addCorrect));
                    alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
                btn2Complaint.Enabled = true;
            };
        }
        private string ImageViewToBase64String(ImageView obj)
        {
            Android.Graphics.Drawables.BitmapDrawable bd1 = (Android.Graphics.Drawables.BitmapDrawable)obj.Drawable;
            Bitmap bitmap = bd1.Bitmap;
            if(bitmap.Height > 1000 || bitmap.Width > 1000)
            {
                if(bitmap.Height > bitmap.Width)
                {
                    int newWidth = Convert.ToInt32((double)bitmap.Width / (double)bitmap.Height * 1000.0);
                    if (newWidth>0)
                    {
                        bitmap = Bitmap.CreateScaledBitmap(bitmap, newWidth, 1000, true);
                    }
                }
                else
                {
                    int newHeight = Convert.ToInt32((double)bitmap.Height / (double)bitmap.Width * 1000.0);
                    if (newHeight>0)
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

        protected virtual void OnComplaintCreated()
        {
            ComplaintCreated(this, EventArgs.Empty);
        }
    }
}