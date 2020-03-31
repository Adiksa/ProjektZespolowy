using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Plugin.NFC;
using ProjektZespolowy.Fragments;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace ProjektZespolowy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private SupportFragment currentFragment;
        private MainFragment mainFragment;
        private CoordinatorLayout rootview;
        private TextView skanText;
        private Furniture furniture;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            ComponentLocalizer();
            Snackbar.Make(rootview, "Zalogowano pomyślnie.", Snackbar.LengthLong).Show();
            CrossNFC.Init(this);
            ActionHooker();
        }

        protected override void OnResume()
        {
            base.OnResume();
            CrossNFC.Current.StartListening();
        }

        public override void OnBackPressed()
        {
            if(mainFragment!=null && !mainFragment.IsMainScreen())
            {
                InitNewFragment(mainFragment);
            }
            else
            {
                base.OnBackPressed();
            }
        }
        private void ActionHooker()
        {
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
        }

        private void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            Vibrator vibrator = (Vibrator)this.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(100);
            var memory = tagInfo.Records.AsMemory();
            if (memory.Span.ToArray().Length>0)
            {
                string furnitureId = memory.Span.ToArray()[0].Message;
                skanText.Text = memory.Span.ToArray()[0].Message;
                FireBaseConnector fcon = new FireBaseConnector();
                furniture = fcon.getFurniture(furnitureId);
                if(furniture==null)
                {
                    Snackbar.Make(rootview, "Mebel o podanym id nie istnieje w naszej bazie.", Snackbar.LengthShort).Show();
                }
                else
                {
                    mainFragment = new MainFragment();
                    mainFragment.furniture = furniture;
                    InitNewFragment(mainFragment);
                    skanText.Visibility = ViewStates.Invisible;
                }
            }
            else
            {
                Snackbar.Make(rootview, "Bład wczytywania tagu nfc.", Snackbar.LengthShort).Show();
            }
            
            
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
    }
}

