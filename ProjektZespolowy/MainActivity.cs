using System;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Animation;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.NFC;
using ProjektZespolowy.Fragments;
using AnimationUtils = Android.Views.Animations.AnimationUtils;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace ProjektZespolowy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private SupportFragment currentFragment;
        private CoordinatorLayout rootview;
        private TextView skanText;
        private Furniture furniture;
        private ImageView animImageView;
        private AnimationDrawable animation;
        private ProgressBar progressBar;
        NfcAdapter nfcAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossNFC.Init(this);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            ComponentLocalizer();
            if(Intent.GetStringExtra("Login") == "1") Snackbar.Make(rootview, GetString(Resource.String.loginDone), Snackbar.LengthLong).Show();
            ActionHooker();
            animation = (AnimationDrawable)animImageView.Background;
            animation.Start();
            nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
            if(!nfcAdapter.IsEnabled)
            {
                animation.Stop();
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialog.SetTitle(GetString(Resource.String.offNFC));
                alertDialog.SetIcon(Resource.Drawable.ic1a_192x192);
                alertDialog.SetMessage(GetString(Resource.String.exitQuestion));
                alertDialog.SetCancelable(false);
                alertDialog.SetNeutralButton(GetString(Resource.String.settingsEnter), delegate
                {
                    Intent intent = new Intent();
                    intent.SetAction(Android.Provider.Settings.ActionNfcSettings);
                    StartActivity(intent);
                    alertDialog.Dispose();
                });
                alertDialog.SetPositiveButton(GetString(Resource.String.refreshNFC), delegate
                {
                    if (nfcAdapter.IsEnabled)
                    {
                        animation.Start();
                        alertDialog.Dispose();
                    }
                    else alertDialog.Show();
                    
                });
                alertDialog.SetNegativeButton(GetString(Resource.String.exit), delegate
                {
                    FinishAffinity();
                });
                alertDialog.Show();
            }
        }

        protected override void OnResume()
        {
            CrossNFC.Current.StartListening();
            if (nfcAdapter.IsEnabled) animation.Start();
            base.OnResume();
        }

        public override void OnBackPressed()
        {
            FinishAffinity();
        }

        private void ActionHooker()
        {
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
        }

        private async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            progressBar.Visibility = ViewStates.Visible;
            animation.Stop();
            skanText.Visibility = ViewStates.Invisible;
            animImageView.Visibility = ViewStates.Invisible;
            await Task.Run(() => ScanNFC(tagInfo));
            progressBar.Visibility = ViewStates.Invisible;
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            CrossNFC.OnNewIntent(intent);
        }

        private void ComponentLocalizer()
        {
            rootview = FindViewById<CoordinatorLayout>(Resource.Id.coordinatorLayout1);
            skanText = FindViewById<TextView>(Resource.Id.skanText);
            animImageView = FindViewById<ImageView>(Resource.Id.waitingForScan);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBarMain);
        }

        private void InitNewFragment(SupportFragment fragment)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            if (currentFragment != null && currentFragment != fragment)
            {
                transaction.Remove(currentFragment);
            }

            if (currentFragment == fragment)
            {
                transaction.Detach(fragment).Attach(fragment);
            }
            else
            {
                transaction.SetCustomAnimations(Resource.Animation.slide_up, Resource.Animation.slide_right);
                transaction.Add(Resource.Id.fragmentContainerMain, fragment, $"{fragment}");
            }
            transaction.Commit();
            currentFragment = fragment;
        }

        private async Task ScanNFC(ITagInfo tagInfo)
        {
            Vibrator vibrator = (Vibrator)this.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(100);
            var memory = tagInfo.Records.AsMemory();
            if (memory.Span.ToArray().Length > 0)
            {
                animation.Stop();
                string furnitureId = memory.Span.ToArray()[0].Message;
                FireBaseConnector fcon = new FireBaseConnector();
                furniture = fcon.getFurniture(furnitureId);
                if (furniture == null)
                {
                    this.RunOnUiThread(() =>
                    {
                        animation.Start();
                        skanText.Visibility = ViewStates.Visible;
                        animImageView.Visibility = ViewStates.Visible;
                    });
                    Snackbar.Make(rootview, GetString(Resource.String.noFurnitureID), Snackbar.LengthShort).Show();
                }
                else
                {
                    Intent intent = new Intent(this, typeof(FurnitureMain));
                    intent.PutExtra("Furniture",furnitureId);
                    Finish();
                    this.StartActivity(intent);
                }
            }
            else
            {
                this.RunOnUiThread(() =>
                {
                    animation.Start();
                    skanText.Visibility = ViewStates.Visible;
                    animImageView.Visibility = ViewStates.Visible;
                });
                Snackbar.Make(rootview, GetString(Resource.String.errorReadTagNFC), Snackbar.LengthShort).Show();
            }
        }
        
    }
}

