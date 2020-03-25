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
        public int dataInsert(UserLogins obj)
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
        public int checkLogin(UserLogins login)
        {
            try
            {
                var resault = client.Get("Login/" + login.login);
                UserLogins res = resault.ResultAs<UserLogins>();
                if (res != null)
                {
                    System.Console.WriteLine(res.login + " " + res.userPassword);
                    if (login.login == res.login && login.userPassword == res.userPassword)
                    {
                        if (login.admin)
                            return 2;
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
        public int checkLoginPossibility(UserLogins login)
        {
            try
            {
                var resault = client.Get("Login/" + login.login);
                UserLogins res = resault.ResultAs<UserLogins>();
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
            Dictionary<string, UserLogins> userDictionary;
            List<UserLogins> listauser = new List<UserLogins>();
            userDictionary = resault.ResultAs<Dictionary<string, UserLogins>>();
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