using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy.Fragments
{
    public class Complaint_Progress : Android.Support.V4.App.Fragment
    {
        private View view;
        private ListView listProgress;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.complaint_progress, container, false);
            ComponentsLocalizer();
            ActionHooker();
            return view;
        }

        private void ComponentsLocalizer()
        {
            listProgress = view.FindViewById<ListView>(Resource.Id.listProgress);
        }

        private void ActionHooker()
        {

        }
    }
}