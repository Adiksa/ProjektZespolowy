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

namespace ProjektZespolowy.Fragments
{
    public class Dialog_SignUp : DialogFragment
    {
        private View view;
        private EditText signUpLogin;
        private EditText signUpPassword;
        private Button signUpAccept;
        private Button signUpCancel;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.sign_up, container, false);
            ComponentsLocalizer();
            ActionHooker();
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
            signUpLogin = view.FindViewById<EditText>(Resource.Id.signUpLogin);
            signUpPassword = view.FindViewById<EditText>(Resource.Id.signUpPassword);
            signUpAccept = view.FindViewById<Button>(Resource.Id.signUpAccept);
            signUpCancel = view.FindViewById<Button>(Resource.Id.signUpCancel);
        }

        private void ActionHooker()
        {
            signUpCancel.Click += delegate
            {
                Dialog.Cancel();
            };
            signUpAccept.Click += delegate
            {
                UserLogins login = new UserLogins()
                {
                    login = signUpLogin.Text,
                    userPassword = signUpPassword.Text,
                    admin = false
                };
                FireBaseConnector connector = new FireBaseConnector();
                if (login.login.Length > 0 && login.userPassword.Length > 0)
                {
                    if (connector.checkLoginPossibility(login) == 0)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.loginInUse));
                        alertDialog.SetIcon(Resource.Drawable.ic4b_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.loginOther));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if (connector.checkLoginPossibility(login) == 1)
                    {
                        connector.dataInsert(login);
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.accountCreated));
                        alertDialog.SetIcon(Resource.Drawable.ok2a_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.accountCreatedMessage)+" " + login.login);
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if (connector.checkLoginPossibility(login) == -1)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle(GetString(Resource.String.noInternetConnection));
                        alertDialog.SetIcon(Resource.Drawable.ic5c_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.checkConnection));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                }
                else
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                    alertDialog.SetTitle(GetString(Resource.String.registerError));
                    alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.registerErrorMessage));
                    alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
            };
        }
    }
}