﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace ProjektZespolowy
{
    class FireBaseConnector
    {
        private IFirebaseConfig fcon;
        private IFirebaseClient client;
        public FireBaseConnector()
        {
            fcon = new FirebaseConfig()
            {
                AuthSecret = "KVFiAucq7n7LKlaubP47a30fXKeNDopS1u2nL1NU",
                BasePath = "https://fragmenttest-343f9.firebaseio.com/"
            };
            try
            {
                client = new FireSharp.FirebaseClient(fcon);
            }
            catch
            {
            }
        }
        public int dataInsert(userLogins obj)
        {
            try
            {
                var setter = client.Set("Login/" + obj.login, obj);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        public int checkLogin(userLogins login)
        {
            try
            {
                var resault = client.Get("Login/" + login.login);
                userLogins res = resault.ResultAs<userLogins>();
                if (res != null)
                {
                    System.Console.WriteLine(res.login + " " + res.userPassword);
                    if (login.login == res.login && login.userPassword == res.userPassword)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }
        public int checkLoginPossibility(userLogins login)
        {
            try
            {
                var resault = client.Get("Login/" + login.login);
                userLogins res = resault.ResultAs<userLogins>();
                if (res == null)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }
        public void test()
        {
            var resault = client.Get("Login");
            Dictionary<string, userLogins> userDictionary;
            List<userLogins> listauser = new List<userLogins>();
            userDictionary = resault.ResultAs<Dictionary<string, userLogins>>();
            if (userDictionary != null)
            {
                foreach (var item in userDictionary)
                {
                    listauser.Add(item.Value);
                }
            }
            foreach (var x in listauser)
            {
                System.Console.WriteLine(x.login);
            }
        }
    }
}