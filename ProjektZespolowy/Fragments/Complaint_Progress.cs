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
    public class Complaint_Progress : DialogFragment
    {
        private View view;
        private ListView listProgress;
        public List<String> complaintProgress;
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
            if(complaintProgress!=null)
            {
                ComplaintProgressListViewAdapter adapter = new ComplaintProgressListViewAdapter(this.Activity, complaintProgress);
                listProgress.Adapter = adapter;
            }
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
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