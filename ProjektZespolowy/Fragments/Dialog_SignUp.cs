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

            // Create your fragment here
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
                userLogins login = new userLogins()
                {
                    login = signUpLogin.Text,
                    userPassword = signUpPassword.Text
                };
                FireBaseConnector connector = new FireBaseConnector();
                if (login.login.Length > 0 && login.userPassword.Length > 0)
                {
                    if (connector.checkLoginPossibility(login) == 0)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle("Zajety login.");
                        alertDialog.SetMessage("Zmienisz login?");
                        alertDialog.SetNeutralButton("No dobra.", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if (connector.checkLoginPossibility(login) == 1)
                    {
                        connector.dataInsert(login);
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle("Konto utworzone.");
                        alertDialog.SetMessage("Utworzone konto o loginie " + login.login);
                        alertDialog.SetNeutralButton("Gitara.", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if (connector.checkLoginPossibility(login) == -1)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        alertDialog.SetTitle("Brak neta.");
                        alertDialog.SetMessage("Podepniesz sie do neta?");
                        alertDialog.SetNeutralButton("No dobra.", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                }
                else
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                    alertDialog.SetTitle("Bład rejestrowania.");
                    alertDialog.SetMessage("Dodasz wszystko jak trzeba?");
                    alertDialog.SetNeutralButton("No dobra.", delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
            };
        }
    }
}