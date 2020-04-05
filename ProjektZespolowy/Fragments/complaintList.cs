using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Graphics.Bitmap;

namespace ProjektZespolowy.Fragments
{
    [Activity(Label = "ComplaintList", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ComplaintList : Android.Support.V4.App.Fragment
    {
        private View view;
        private ListView complaints;
        private Button createButton;
        public Furniture furniture;
        private List<Complaint> complaintList;
        private List<string> complaintIdList;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.complaint_list, container, false);
            ComponentsLocalizer();
            ActionHooker();
            FireBaseConnector connector = new FireBaseConnector();
            complaintIdList = connector.getFurnitureComplaintList(furniture.id);
            complaintList = connector.GetComplaints(complaintIdList);
            if (complaintList != null)
            {
                ComplaintListViewAdapter adapter = new ComplaintListViewAdapter(this.Activity, complaintList);
                complaints.Adapter = adapter;
            }
            return view;
        }
        public override void OnResume()
        {
            base.OnResume();
            FireBaseConnector connector = new FireBaseConnector();
            if(connector.getFurnitureComplaintList(furniture.id) != complaintIdList)
            {
                complaintIdList = connector.getFurnitureComplaintList(furniture.id);
                List<Complaint> complaintList = connector.GetComplaints(complaintIdList);
                if (complaintList != null)
                {
                    ComplaintListViewAdapter adapter = new ComplaintListViewAdapter(this.Activity, complaintList);
                    complaints.Adapter = adapter;
                }
            }
        }

        private void ComponentsLocalizer()
        {
            complaints = view.FindViewById<ListView>(Resource.Id.complaintList);
            createButton = view.FindViewById<Button>(Resource.Id.complaintCreate);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {

        }

        private void ActionHooker()
        {
            createButton.Click += delegate
            {
                var transaction = this.Activity.FragmentManager.BeginTransaction();
                ComplaintCreate create = new ComplaintCreate();
                create.furniture = furniture;
                create.Show(transaction, "create complaint dialog");
                this.OnResume();
            };
            complaints.ItemClick += Complaints_ItemClick;
        }

        private void Complaints_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var transaction = this.Activity.FragmentManager.BeginTransaction();
            Complaint_Progress progress = new Complaint_Progress();
            progress.complaintProgress = complaintList[e.Position].complaintProgress;
            progress.Show(transaction, "create complaint progress dialog");
        }
    }
}