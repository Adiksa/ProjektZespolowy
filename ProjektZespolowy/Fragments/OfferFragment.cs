using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
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
        private Button fRemoveFromCart;
        private ImageView fAddToFav;
        private ProgressBar fProgressBar;
        private EditText fAmmount;
        private TextView fOrderPrice;
        private TextView fAmmountText;
        private ImageView closeBtn;
        private bool fav;
        private bool refresh;
        public Promotion promotion;
        public Order order;
        public event EventHandler OfferChange;
        public event EventHandler UnlockGridView;
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
            if (order != null)
            {
                fOrderPrice.Text = order.price.ToString();
                fAmmount.Text = order.amount.ToString();
                fRemoveFromCart.Visibility = ViewStates.Visible;
                fAmmount.Visibility = ViewStates.Visible;
                fAmmountText.Visibility = ViewStates.Visible;
                fOrderPrice.Visibility = ViewStates.Visible;
                fAddToCart.Visibility = ViewStates.Invisible;
                fOrderPrice.Text = "x" + order.product.price + "zł = " + order.price + "zł";
            }
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
            UnlockGridView(this, EventArgs.Empty);
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
            fRemoveFromCart = view.FindViewById<Button>(Resource.Id.removeFromCart);
            fOrderPrice = view.FindViewById<TextView>(Resource.Id.offerAmountSum);
            fAmmount = view.FindViewById<EditText>(Resource.Id.offerAmount);
            fAmmountText = view.FindViewById<TextView>(Resource.Id.offerAmountText);
            closeBtn = view.FindViewById<ImageView>(Resource.Id.offerCloseButton);
        }

        private void ActionHooker()
        {
            fAddToFav.Click += FAddToFav_Click;
            fAddToCart.Click += delegate
            {
                GlobalVars.cart = Order.AddToList(GlobalVars.cart, promotion);
                Toast.MakeText(this.Activity, GetString(Resource.String.addedToCart), ToastLength.Short).Show();
            };
            fAmmount.AfterTextChanged += delegate
            {
                try
                {
                    var temp = int.Parse(fAmmount.Text);
                    if(temp<0)
                    {
                        fAmmount.Text = order.amount.ToString();
                        Toast.MakeText(this.Activity, GetString(Resource.String.errorAmmount), ToastLength.Short);
                    }
                    else
                    {
                        if(temp == 0)
                        {
                            fAmmount.Text = order.amount.ToString();
                            Toast.MakeText(this.Activity, GetString(Resource.String.errorRemove), ToastLength.Short);
                        }
                        else
                        {
                            refresh = true;
                            order.amount = temp;
                            order.priceCount();
                            fOrderPrice.Text = "x" + order.product.price + "zł = " + order.price + "zł";
                            foreach (Order o in GlobalVars.cart)
                            {
                                if(o == order)
                                {
                                    o.amount = temp;
                                    o.priceCount();
                                }
                            }
                        }
                    }
                }
                catch
                {
                    fAmmount.Text = order.amount.ToString();
                    Toast.MakeText(this.Activity, GetString(Resource.String.errorAmmount), ToastLength.Short);
                }
            };
            fRemoveFromCart.Click += delegate
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                alertDialog.SetTitle(GetString(Resource.String.removeFromCart));
                alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                alertDialog.SetMessage(GetString(Resource.String.alertRemove));
                alertDialog.SetPositiveButton(GetString(Resource.String.yes), delegate
                {
                    refresh = true;
                    GlobalVars.cart.Remove(order);
                    order = null;
                    this.Dismiss();
                    alertDialog.Dispose();
                });
                alertDialog.SetNegativeButton(GetString(Resource.String.no), delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            };
            closeBtn.Click += delegate
            {
                this.Dismiss();
            };
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