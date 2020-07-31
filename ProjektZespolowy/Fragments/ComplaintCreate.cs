using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using static Android.Graphics.Bitmap;

namespace ProjektZespolowy.Fragments
{
    public class ComplaintCreate : DialogFragment
    {
        private View view;
        private EditText problemDesc;
        private ImageView photoPreview;
        private TextView btn1Complaint;
        private TextView btn2Complaint;
        public Furniture furniture;
        public event EventHandler ComplaintCreated;
        private Drawable photoDefault;
        private long lastClickTime;
        private List<Bitmap> images;
        private TextView lBtn;
        private TextView rBtn;
        private int photoi = 0;
        private TextView deleteImage;
        private TextView photoAmount;
        private ProgressBar progressBar;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            images = new List<Bitmap>();
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
            btn1Complaint = view.FindViewById<TextView>(Resource.Id.btn1Complaint);
            btn2Complaint = view.FindViewById<TextView>(Resource.Id.btn2Complaint);
            lBtn = view.FindViewById<TextView>(Resource.Id.leftBtn);
            rBtn = view.FindViewById<TextView>(Resource.Id.rightBtn);
            deleteImage = view.FindViewById<TextView>(Resource.Id.complaintImageDelete);
            photoAmount = view.FindViewById<TextView>(Resource.Id.txtClickToPick);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBarComplaintCreate);
            Refresh();
        }

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((resultCode == Result.Ok) && (data != null))
            {
                switch (requestCode)
                {
                    case 0:
                        images.Add((Bitmap)data.Extras.Get("data"));
                        break;
                    case 1:
                        List<Bitmap> bitmapy = new List<Bitmap>();
                        ClipData clipData = data.ClipData;
                        try
                        {
                            if (clipData.ItemCount > 0)
                            {
                                for (int i = 0; i < clipData.ItemCount; i++)
                                {
                                    Android.Net.Uri imageUri = clipData.GetItemAt(i).Uri;
                                    try
                                    {
                                        bitmapy.Add(MediaStore.Images.Media.GetBitmap(this.Activity.ContentResolver, imageUri));
                                    }
                                    catch
                                    {

                                    }

                                }
                                if (bitmapy.Count + images.Count > 5)
                                {
                                    Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotosSelected), ToastLength.Short).Show();
                                }
                                int j = 0;
                                while (images.Count < 5 && j < bitmapy.Count)
                                {
                                    images.Add(bitmapy[j]);
                                    j++;
                                }
                            }
                        }
                        catch
                        {
                            try
                            {
                                images.Add(MediaStore.Images.Media.GetBitmap(this.Activity.ContentResolver, data.Data));
                            }
                            catch
                            {

                            }
                        }
                        break;
                    case 2:
                        images[photoi] = (Bitmap)data.Extras.Get("data");
                        break;
                    case 3:
                        try
                        {
                            images[photoi] =
                                MediaStore.Images.Media.GetBitmap(this.Activity.ContentResolver, data.Data);
                        }
                        catch
                        {

                        }
                        break;
                }
                Refresh();
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case 0:
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this.Activity, Resource.String.noPermissions, ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        if (images.Count > 4)
                        {
                            Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotos), ToastLength.Short).Show();
                        }
                        else
                        {
                            Intent intent = new Intent(MediaStore.ActionImageCapture);
                            intent.PutExtra(MediaStore.ExtraVideoQuality, true);
                            intent.PutExtra(MediaStore.ExtraOutput, 1);
                            StartActivityForResult(intent, 0);
                        }
                    }
                    break;
                case 1:
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this.Activity, Resource.String.noPermissions, ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        if (images.Count > 4)
                        {
                            Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotos), ToastLength.Long).Show();
                        }
                        else
                        {
                            this.Activity.Intent = new Intent();
                            this.Activity.Intent.PutExtra(Intent.ExtraAllowMultiple, true);
                            this.Activity.Intent.SetType("image/*");
                            this.Activity.Intent.SetAction(Intent.ActionGetContent);
                            StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
                        }
                    }
                    break;
                case 2:
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Denied)
                    {
                        Toast.MakeText(this.Activity, Resource.String.noPermissions, ToastLength.Short).Show();
                    }
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        if (images.Count > 4)
                        {
                            Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotos), ToastLength.Short).Show();
                        }
                        else
                        {
                            Intent intent = new Intent(MediaStore.ActionImageCapture);
                            intent.PutExtra(MediaStore.ExtraVideoQuality, true);
                            intent.PutExtra(MediaStore.ExtraOutput, 1);
                            StartActivityForResult(intent, 2);
                        }
                    }
                    break;
                case 3:
                    if ((grantResults.Length == 1) && (grantResults[0]) == Android.Content.PM.Permission.Granted)
                    {
                        if (images.Count > 4)
                        {
                            Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotos), ToastLength.Long).Show();
                        }
                        else
                        {
                            this.Activity.Intent = new Intent();
                            this.Activity.Intent.SetType("image/*");
                            this.Activity.Intent.SetAction(Intent.ActionGetContent);
                            StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
                        }
                    }
                    break;
                default:
                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    break;
            }
        }

        private void ActionHooker()
        {
            photoPreview.Click += delegate
            {
                if (images.Count > 0 && images.Count < 5)
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                    alertDialog.SetTitle(GetString(Resource.String.photoReplaceTitle));
                    alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.photoReplaceMsg));
                    alertDialog.SetNeutralButton(GetString(Resource.String.photoReplaceCamera), delegate
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        intent.PutExtra(MediaStore.ExtraVideoQuality, true);
                        intent.PutExtra(MediaStore.ExtraOutput, 1);
                        StartActivityForResult(intent, 2);
                    });
                    alertDialog.SetNegativeButton(GetString(Resource.String.photoReplaceGalery), delegate
                    {
                        this.Activity.Intent = new Intent();
                        this.Activity.Intent.SetType("image/*");
                        this.Activity.Intent.SetAction(Intent.ActionGetContent);
                        StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 3);
                    });
                    alertDialog.SetPositiveButton(GetString(Resource.String.photoReplaceAdd), delegate
                    {
                        this.Activity.Intent = new Intent();
                        this.Activity.Intent.PutExtra(Intent.ExtraAllowMultiple, true);
                        this.Activity.Intent.SetType("image/*");
                        this.Activity.Intent.SetAction(Intent.ActionGetContent);
                        StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
                    });
                    alertDialog.Show();
                }
                else
                {
                    if (images.Count == 0)
                    {
                        if (this.Activity.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) == Android.Content.PM.Permission.Granted)
                        {
                            this.Activity.Intent = new Intent();
                            this.Activity.Intent.PutExtra(Intent.ExtraAllowMultiple, true);
                            this.Activity.Intent.SetType("image/*");
                            this.Activity.Intent.SetAction(Intent.ActionGetContent);
                            StartActivityForResult(Intent.CreateChooser(this.Activity.Intent, "Select picture"), 1);
                        }
                        else
                        {
                            var requiredPermissions = new String[] { Android.Manifest.Permission.ReadExternalStorage };
                            RequestPermissions(requiredPermissions, 1);
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotos), ToastLength.Short).Show();
                    }
                }
            };
            btn1Complaint.Click += delegate
            {
                if (this.Activity.ApplicationContext.CheckSelfPermission(Android.Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
                {
                    if (images.Count > 4)
                    {
                        Toast.MakeText(this.Activity, GetString(Resource.String.tooManyPhotos), ToastLength.Short).Show();
                    }
                    else
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        intent.PutExtra(MediaStore.ExtraVideoQuality, true);
                        intent.PutExtra(MediaStore.ExtraOutput, 1);
                        StartActivityForResult(intent, 0);
                    }
                }
                else
                {
                    var requiredPermissions = new String[] { Android.Manifest.Permission.Camera };
                    RequestPermissions(requiredPermissions, 0);
                }

            };
            btn2Complaint.Click += Btn2Complaint_Click;
            rBtn.Click += delegate
            {
                photoi++;
                if (photoi == images.Count) photoi = 0;
                Activity.RunOnUiThread(() => photoPreview.SetImageBitmap(images[photoi]));
                Refresh();
            };
            lBtn.Click += delegate
            {
                photoi--;
                if (photoi < 0) photoi = images.Count - 1;
                Activity.RunOnUiThread(() => photoPreview.SetImageBitmap(images[photoi]));
                Refresh();
            };
            deleteImage.Click += delegate
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity)
                    .SetTitle(GetString(Resource.String.deleteImageTitle))
                    .SetIcon(Resource.Drawable.ic4b_192x192)
                    .SetMessage(GetString(Resource.String.deleteImageMsg))
                    .SetPositiveButton(GetString(Resource.String.yes), delegate
                    {
                        images.RemoveAt(photoi);
                        if (images.Count != 0)
                        {
                            if (photoi >= images.Count)
                                photoi--;
                            Refresh();
                        }
                        else
                        {
                            photoPreview.SetImageDrawable(photoDefault);
                        }
                        Refresh();
                    })
                    .SetNegativeButton(GetString(Resource.String.no), delegate { });
                alertDialog.Show();
            };
        }

        private void Btn2Complaint_Click(object sender, EventArgs e)
        {
            if (SystemClock.ElapsedRealtime() - lastClickTime > 1000)
            {
                this.Activity.RunOnUiThread(() => btn2Complaint.Enabled = false);
                AddComplaint();
            }
            lastClickTime = SystemClock.ElapsedRealtime();
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

        private string ImageToBase64String(Bitmap bitmapa)
        {
            Bitmap bitmap = bitmapa;
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

        protected virtual void OnComplaintCreated()
        {
            ComplaintCreated(this, EventArgs.Empty);
        }

        private async Task AddComplaint()
        {
            this.Activity.RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);
            List<string> photos = new List<string>();
            foreach (Bitmap b in images)
            {
                photos.Add(ImageToBase64String(b));
            }
            Complaint complaint = new Complaint()
            {
                furnitureId = furniture.id,
                description = problemDesc.Text,
                photo = photos,
                senderName = GlobalVars.login,
                madeBy = furniture.madeBy
            };
            if (complaint.Correct() && photoDefault != photoPreview.Drawable)
            {
                FireBaseConnector connector = new FireBaseConnector();
                var res = connector.dataInsert(complaint);
                if (res == 0)
                {
                    if(photos.Count != 0)
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
                    else
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.photoError));
                        alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.photoErrorMsg));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
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
            this.Activity.RunOnUiThread(() => btn2Complaint.Enabled = true);
            this.Activity.RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);
        }
        private void Refresh()
        {
            switch (images.Count)
            {
                case 1:
                    lBtn.Visibility = ViewStates.Invisible;
                    rBtn.Visibility = ViewStates.Invisible;
                    deleteImage.Visibility = ViewStates.Visible;
                    photoPreview.SetImageBitmap(images[photoi]);
                    break;
                case 0:
                    lBtn.Visibility = ViewStates.Invisible;
                    rBtn.Visibility = ViewStates.Invisible;
                    deleteImage.Visibility = ViewStates.Invisible;
                    break;
                default:
                    lBtn.Visibility = ViewStates.Visible;
                    rBtn.Visibility = ViewStates.Visible;
                    deleteImage.Visibility = ViewStates.Visible;
                    photoPreview.SetImageBitmap(images[photoi]);
                    break;
            }
            RefreshNum();
        }
        private void RefreshNum()
        {
            switch (images.Count)
            {
                case 1:
                    photoAmount.Text = "(1/1)\n" + GetString(Resource.String.txtClickToPick);
                    break;
                case 0:
                    photoAmount.Text = GetString(Resource.String.txtClickToPick);
                    break;
                default:
                    photoAmount.Text = "(" + (photoi + 1) + "/" + images.Count + ")\n" + GetString(Resource.String.txtClickToPick);
                    break;
            }
        }
    }
}