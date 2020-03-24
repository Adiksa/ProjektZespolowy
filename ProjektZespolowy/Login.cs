﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using ProjektZespolowy.Fragments;

namespace ProjektZespolowy
{
    [Activity(Label = "Login", MainLauncher = true)]
    public class Login : Activity
    {
        private EditText userLogin;
        private EditText userPassword;
        private Button loginBtn;
        private Button signupBtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            ComponentLocalizer();
            ActionHooker();
        }

        private void ComponentLocalizer()
        {
            userLogin = FindViewById<EditText>(Resource.Id.login);
            userPassword = FindViewById<EditText>(Resource.Id.password);
            loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            signupBtn = FindViewById<Button>(Resource.Id.singUpButton);
        }

        private void ActionHooker()
        {
            loginBtn.Click += delegate
            {
                userLogins login = new userLogins()
                {
                    login = userLogin.Text,
                    userPassword = userPassword.Text
                };
                FireBaseConnector connection = new FireBaseConnector();
                if (connection.connected)
                {
                    if (connection.checkLogin(login) && login.login.Length > 0 && login.userPassword.Length > 0)
                    {
                        Finish();
                        StartActivity(typeof(MainActivity));
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
                }
                else
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
            };
            signupBtn.Click += delegate
            {
                FireBaseConnector connection = new FireBaseConnector();
                if (connection.connected)
                {
                    var transaction = FragmentManager.BeginTransaction();
                    Dialog_SignUp signUpDialog = new Dialog_SignUp();
                    signUpDialog.Show(transaction, "dialog fragment");

                }
                else
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
            };
        }
    }
}