using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Plugin.NFC;
using ProjektZespolowy.Fragments;

namespace ProjektZespolowy
{
    [Activity(Label = "Meble NFC", Icon="@drawable/icon5", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        private EditText userLogin;
        private EditText userPassword;
        private Button loginBtn;
        private Button signupBtn;
        private ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossNFC.Init(this);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            ComponentLocalizer();
            ActionHooker();
            NfcAdapter nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
            if (nfcAdapter == null)
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialog.SetTitle(GetString(Resource.String.noNFC));
                alertDialog.SetIcon(Resource.Drawable.ic1c_192x192);
                alertDialog.SetMessage(GetString(Resource.String.noSupport));
                alertDialog.SetCancelable(false);
                alertDialog.SetNeutralButton(GetString(Resource.String.exit), delegate
                {
                    Finish();
                });
                alertDialog.Show();
            }
        }

        private void ComponentLocalizer()
        {
            userLogin = FindViewById<EditText>(Resource.Id.login);
            userPassword = FindViewById<EditText>(Resource.Id.password);
            loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            signupBtn = FindViewById<Button>(Resource.Id.signUpButton);
            progressBar = FindViewById<ProgressBar>(Resource.Id.LoginsProgressBar);
        }

        private void ActionHooker()
        {
            
            loginBtn.Click += delegate
            {
                LoginCheck();
            };
            signupBtn.Click += delegate
            {
                var transaction = FragmentManager.BeginTransaction();
                Dialog_SignUp signUpDialog = new Dialog_SignUp();
                signUpDialog.Show(transaction, "dialog fragment");
            };
        }

        private async Task LoginCheck()
        {
            await Task.Run(() => this.RunOnUiThread(() =>
            {
                progressBar.Visibility = ViewStates.Visible;
            }));
            UserLogins login = new UserLogins()
            {
                login = userLogin.Text,
                userPassword = userPassword.Text
            };
            FireBaseConnector connection = new FireBaseConnector();
            if (login.login.Length > 0 && login.userPassword.Length > 0)
            {
                var res = connection.checkLogin(login);
                await Task.Run(() => this.RunOnUiThread(() =>
                {
                    progressBar.Visibility = ViewStates.Invisible;
                }));
                if (res == 2)
                {
                    Finish();
                    StartActivity(typeof(Admin));
                }
                if (res == 1)
                {
                    GlobalVars.login = login.login;
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("Login", "1");
                    Finish();
                    this.StartActivity(intent);

                }
                if (!connection.connection)
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alertDialog.SetTitle(GetString(Resource.String.noInternetConnection));
                    alertDialog.SetIcon(Resource.Drawable.ic5c_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.checkConnection));
                    alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
                if (res == 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alertDialog.SetTitle(GetString(Resource.String.loginError));
                    alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.correctLogin));
                    alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
            }
            else
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialog.SetTitle(GetString(Resource.String.loginError));
                alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                alertDialog.SetMessage(GetString(Resource.String.emptyLogin));
                alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }
            await Task.Run(() => this.RunOnUiThread(() =>
            {
                progressBar.Visibility = ViewStates.Invisible;
            }));
        }
    }
}