using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace ProjektZespolowy.Fragments
{
    public class MainFragment : Android.Support.V4.App.Fragment
    {
        private SupportFragment currentFragment;
        private Scan_Success_View scan_Success_View;
        private View view;
        public Furniture furniture;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.main_fragment, container, false);
            scan_Success_View = new Scan_Success_View();
            scan_Success_View.furniture = furniture;
            InitNewFragment(scan_Success_View);
            ComponentsLocalizer();
            return view;
        }

        private void ComponentsLocalizer()
        {

        }

        private void InitNewFragment(SupportFragment fragment)
        {
            var transaction = this.FragmentManager.BeginTransaction();
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
                transaction.Add(Resource.Id.fragmentConatiner, fragment, $"{fragment}");
            }

            transaction.Commit();
            currentFragment = fragment;
        }
    }
}