using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProjektZespolowy.Fragments;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.View;
using System.Runtime.Remoting.Messaging;
using Android.Util;

namespace ProjektZespolowy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class FurnitureMain : AppCompatActivity
    {
        private SupportFragment currentFragment;
        private Scan_Success_View scan_Success_View;
        private Specification specification;
        private ComplaintList complaintList;
        private Shop shop;
        public Furniture furniture;
        private ImageButton specBtn;
        private ImageButton complaintBtn;
        private ImageButton shopBtn;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View headerview;
        private TextView loginAs;
        private Button logoutBtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_fragment);
            FireBaseConnector fcon = new FireBaseConnector();
            string test = Intent.GetStringExtra("Furniture");
            furniture = fcon.getFurniture(test);
            scan_Success_View = new Scan_Success_View();
            scan_Success_View.furniture = furniture;
            specification = new Specification();
            specification.furniture = furniture;
            complaintList = new ComplaintList();
            complaintList.furniture = furniture;
            shop = new Shop();
            InitNewFragment(scan_Success_View);
            ComponentsLocalizer();
            ActionHooker();
            headerview = navigationView.GetHeaderView(0);
            loginAs = headerview.FindViewById<TextView>(Resource.Id.loginAs);
            loginAs.Text += " " + GlobalVars.login;
        }

        public override void OnBackPressed()
        {
            if (currentFragment != scan_Success_View)
            {
                InitNewFragment(scan_Success_View);
            }
            else
            {
                Finish();
                StartActivity(typeof(MainActivity));
            }
        }

        private void ActionHooker()
        {
            specBtn.Click += SpecBtn_Click;
            complaintBtn.Click += ComplaintBtn_Click;
            shopBtn.Click += ShopBtn_Click;
            logoutBtn.Click += delegate
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialog.SetTitle(GetString(Resource.String.logoutAlert));
                alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                alertDialog.SetMessage(GetString(Resource.String.logoutAlertMsg));
                alertDialog.SetPositiveButton(GetString(Resource.String.yes), delegate
                {
                    this.Finish();
                    StartActivity(typeof(Login));
                    alertDialog.Dispose();
                });
                alertDialog.SetNegativeButton(GetString(Resource.String.no), delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
                
            };
        }

        private void ComplaintBtn_Click(object sender, EventArgs e)
        {
            InitNewFragment(complaintList);
        }

        private void SpecBtn_Click(object sender, EventArgs e)
        {
            InitNewFragment(specification);
        }
        private void ShopBtn_Click(object sender, EventArgs e)
        {
            InitNewFragment(shop);
        }

        private void ComponentsLocalizer()
        {
            specBtn = FindViewById<ImageButton>(Resource.Id.specBtn);
            complaintBtn = FindViewById<ImageButton>(Resource.Id.complaintBtn);
            shopBtn = FindViewById<ImageButton>(Resource.Id.shopBtn);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            logoutBtn = FindViewById<Button>(Resource.Id.button_sing_out);
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
                transaction.Add(Resource.Id.fragmentContainer, fragment, $"{fragment}");
            }
            transaction.Commit();
            currentFragment = fragment;
        }

        public bool IsMainScreen()
        {
            if (currentFragment == scan_Success_View)
                return true;
            return false;
        }
    }
}