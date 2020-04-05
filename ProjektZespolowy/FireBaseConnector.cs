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
        private readonly IFirebaseConfig fcon;
        private readonly IFirebaseClient client;
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
        public int dataInsert(Furniture obj)
        {
            try
            {
                var setter = client.Set("Furniture/" + obj.id, obj);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public int dataInsert(Complaint obj)
        {
            try
            {
                if (obj.Correct())
                {
                    obj.id = getComplaintLastId();
                    List<string> complaintProgress = new List<string>();
                    complaintProgress.Add(DateTime.Now.ToString() + " - Dodano reklamację.");
                    obj.complaintProgress = complaintProgress;
                    var setter = client.Set("Complaint/" + obj.id, obj);
                    List<String> complaintList = getFurnitureComplaintList(obj.furnitureId);
                    complaintList.Add(obj.id);
                    setter = client.Set("Furniture/" + obj.furnitureId + "/complaintList", complaintList);
                    return 1;
                }
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public List<Complaint> GetComplaints(List<String> furnitureComplaintList)
        {
            try
            {
                var resault = client.Get("Complaint/");
                List<Complaint> complaintList = new List<Complaint>();
                complaintList = resault.ResultAs<List<Complaint>>();
                List<Complaint> list = new List<Complaint>();
                foreach (Complaint item in complaintList)
                {
                    if (item != null)
                    {
                        if (furnitureComplaintList.Contains(item.id))
                            list.Add(item);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
        public List<String> getFurnitureComplaintList(string id)
        {
            var resault = client.Get("Furniture/" + id + "/complaintList");
            var res = resault.ResultAs<List<String>>();
            if (res == null)
            {
                return new List<String>();
            }
            else
                return res;
        }
        public string getComplaintLastId()
        {
            var resault = client.Get("Complaintid/id");
            var res = resault.ResultAs<string>();
            if (res != null)
            {
                client.Set("Complaintid/id", (int.Parse(res) + 1).ToString());
                return (int.Parse(res) + 1).ToString();
            }
            client.Set("Complaintid/id", "0");
            return 0.ToString();
        }
        public Furniture getFurniture(string id)
        {
            try
            {
                var resault = client.Get("Furniture/" + id);
                return resault.ResultAs<Furniture>();
            }
            catch
            {
                return null;
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
                        if (res.admin == true)
                        {
                            return 2;
                        }
                        else
                        {
                            return 1;
                        }
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
        public int checkFurniturePossibility(Furniture obj)
        {
            try
            {
                var resault = client.Get("Furniture/" + obj.id);
                Furniture res = resault.ResultAs<Furniture>();
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
    }
}