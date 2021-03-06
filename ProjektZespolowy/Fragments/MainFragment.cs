﻿using System;
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
        private Specification specification;
        private ComplaintList complaintList;
        private Shop shop;
        public Furniture furniture;
        private View view;
        private ImageButton specBtn;
        private ImageButton complaintBtn;
        private ImageButton shopBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.main_fragment, container, false);
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
            return view;
        }

        private void ActionHooker()
        {
            specBtn.Click += SpecBtn_Click;
            complaintBtn.Click += ComplaintBtn_Click;
            shopBtn.Click += ShopBtn_Click;
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
            specBtn = view.FindViewById<ImageButton>(Resource.Id.specBtn);
            complaintBtn = view.FindViewById<ImageButton>(Resource.Id.complaintBtn);
            shopBtn = view.FindViewById<ImageButton>(Resource.Id.shopBtn);
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