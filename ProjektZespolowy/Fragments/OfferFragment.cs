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
    public class OfferFragment : DialogFragment
    {
        private View view;
        private TextView fOfferTitle;
        private ImageView fOfferPhoto;
        private TextView fOfferDesc;
        private TextView fOfferPrice;
        private Button fAddToCart;
        public Promotion promotion;
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
            fOfferTitle = view.FindViewById<TextView>(Resource.Id.offerTitle);
            fOfferPhoto = view.FindViewById<ImageView>(Resource.Id.offerPhoto);
            fOfferDesc = view.FindViewById<TextView>(Resource.Id.offerDesc);
            fOfferPrice = view.FindViewById<TextView>(Resource.Id.offerPrice);
            fAddToCart = view.FindViewById<Button>(Resource.Id.addToCart);
        }

        private void ActionHooker()
        {

        }
    }
}