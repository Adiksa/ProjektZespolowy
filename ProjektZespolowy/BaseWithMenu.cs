using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    [Activity]
    public abstract class BaseWithMenu : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public DrawerLayout drawerLayout;
        public NavigationView navigationView;
        public ViewStub stub;
        public Button logoutBtn;
        public View headerview;
        public TextView loginAs;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.base_with_menu);
            ComponentsLocalizer();
            ActionHooker();
            headerview = navigationView.GetHeaderView(0);
            loginAs = headerview.FindViewById<TextView>(Resource.Id.loginAs);
            loginAs.Text += " " + GlobalVars.login;
            navigationView.SetNavigationItemSelectedListener(this);
        }

        protected virtual void ActionHooker()
        {
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

        protected virtual void ComponentsLocalizer()
        {
            stub = FindViewById<ViewStub>(Resource.Id.viewStub);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            logoutBtn = FindViewById<Button>(Resource.Id.button_sing_out);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.furMenu_whishList:
                    StartActivity(typeof(WhishList));
                    break;
                case Resource.Id.furMenu_cart:
                    StartActivity(typeof(Cart));
                    break;
            }
            return true;
        }
    }
}