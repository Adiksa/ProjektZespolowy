using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossNFC.Init(this);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            ComponentLocalizer();
            ActionHooker();
            if(!CrossNFC.Current.IsAvailable)
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("Brak NFC");
                alertDialog.SetMessage("Te urządzenie nie wspiera komunikacji NFC.");
                alertDialog.SetCancelable(false);
                alertDialog.SetNeutralButton("Wyjdź z programu.", delegate
                {
                    Finish();
                });
            }
        }

        private void ComponentLocalizer()
        {
            userLogin = FindViewById<EditText>(Resource.Id.login);
            userPassword = FindViewById<EditText>(Resource.Id.password);
            loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            signupBtn = FindViewById<Button>(Resource.Id.signUpButton);
        }

        private void ActionHooker()
        {
            loginBtn.Click += delegate
            {
                UserLogins login = new UserLogins()
                {
                    login = userLogin.Text,
                    userPassword = userPassword.Text
                };
                FireBaseConnector connection = new FireBaseConnector();
                if (login.login.Length > 0 && login.userPassword.Length > 0)
                {
                    var res = connection.checkLogin(login);
                    if (res == 2)
                    {
                        Finish();
                        StartActivity(typeof(Admin));
                    }
                    if (res == 1)
                    {
                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.PutExtra("Login", "1");
                        Finish();
                        this.StartActivity(intent);
                           
                    }
                    if(res == -1)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                        alertDialog.SetTitle("Brak neta.");
                        alertDialog.SetMessage("Podepniesz sie do neta?");
                        alertDialog.SetNeutralButton("No dobra.", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                    if(res == 0)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                        alertDialog.SetTitle("Bład logowania.");
                        alertDialog.SetMessage("Dodasz wszystko jak trzeba?");
                        alertDialog.SetNeutralButton("No dobra.", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    }
                }
                else
                {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                        alertDialog.SetTitle("Bład logowania.");
                        alertDialog.SetMessage("Dodasz wszystko jak trzeba?");
                        alertDialog.SetNeutralButton("No dobra.", delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                }
            };
            signupBtn.Click += delegate
            {
                var transaction = FragmentManager.BeginTransaction();
                Dialog_SignUp signUpDialog = new Dialog_SignUp();
                signUpDialog.Show(transaction, "dialog fragment");
            };
        }
    }
}