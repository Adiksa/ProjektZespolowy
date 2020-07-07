using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy.Fragments
{
    public class OfferFragment : DialogFragment
    {
        private View view;
        private TextView fOfferTitle;
        private ImageView fOfferPhoto;
        private TextView fOfferDesc;
        private TextView fOfferPrice;
        private Button fAddToCart;
        private ImageView fAddToFav;
        private ProgressBar fProgressBar;
        private bool fav;
        private bool refresh;
        public Promotion promotion;
        public event EventHandler OfferChange;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.offer, container, false);
            ComponentsLocalizer();
            ActionHooker();
            fOfferDesc.Text = promotion.desc;
            fOfferPrice.Text = GetString(Resource.String.priceTxt) + " "+ promotion.price;
            fOfferTitle.Text = promotion.title;
            fOfferPhoto.SetImageBitmap(promotion.convertBase64ToBitmap(promotion.image));
            if(promotion.WhishList!=null)
            {
                if (promotion.WhishList.Contains(GlobalVars.login))
                {
                    fav = true;
                    fAddToFav.SetImageDrawable(this.Activity.GetDrawable(Resource.Drawable.heart_red));
                }
                else fav = false;
            }
            else fav = false;
            refresh = false;
            return view;
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

        public override void OnDetach()
        {
            base.OnDetach();
            if(refresh)
            {
                OnOfferChange();
            }
        }

        private void ComponentsLocalizer()
        {
            fOfferTitle = view.FindViewById<TextView>(Resource.Id.offerTitle);
            fOfferPhoto = view.FindViewById<ImageView>(Resource.Id.offerPhoto);
            fOfferDesc = view.FindViewById<TextView>(Resource.Id.offerDesc);
            fOfferPrice = view.FindViewById<TextView>(Resource.Id.offerPrice);
            fAddToCart = view.FindViewById<Button>(Resource.Id.addToCart);
            fAddToFav = view.FindViewById<ImageView>(Resource.Id.addToFav);
            fProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBarOffer);
        }

        private void ActionHooker()
        {
            fAddToFav.Click += FAddToFav_Click;
        }
        protected virtual void OnOfferChange()
        {
            OfferChange(this, EventArgs.Empty);
        }

        private async void FAddToFav_Click(object sender, EventArgs e)
        {
            fProgressBar.Visibility = ViewStates.Visible;
            await Task.Run(() =>
            {
                FireBaseConnector connector = new FireBaseConnector();
                if (fav)
                {
                    if (connector.AddToWhishList(promotion.id, GlobalVars.login, true))
                    {
                        fav = false;
                        fAddToFav.SetImageDrawable(this.Activity.GetDrawable(Resource.Drawable.heart));
                        refresh = true;
                    }

                }
                else
                {
                    if (connector.AddToWhishList(promotion.id, GlobalVars.login, false))
                    {
                        fav = true;
                        fAddToFav.SetImageDrawable(this.Activity.GetDrawable(Resource.Drawable.heart_red));
                        refresh = true;
                    }
                }
            });
            fProgressBar.Visibility = ViewStates.Invisible;
        }
    }
}