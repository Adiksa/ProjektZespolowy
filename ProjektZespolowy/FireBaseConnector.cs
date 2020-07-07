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
using System.Threading.Tasks;

namespace ProjektZespolowy
{
    class FireBaseConnector
    {
        private readonly IFirebaseConfig fcon;
        private readonly IFirebaseClient client;
        public bool connection = false;
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
                this.testCon();
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
                this.testCon();
                var setter = client.Set("Furniture/" + obj.id, obj);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public int dataInsert(Promotion obj)
        {
            try
            {
                this.testCon();
                if (this.connection == false)
                    return -1;
                obj.id = this.getPromotionLastId();
                var setter = client.Set("Promotion/" + obj.id, obj);
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
                this.testCon();
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
                this.testCon();
                List<Complaint> list = new List<Complaint>();
                Task[] tasks = new Task[furnitureComplaintList.Count];
                int i = 0;
                foreach (String item in furnitureComplaintList)
                {
                    tasks[i] = Task.Run(async () =>
                    {
                        var resault = await client.GetAsync("Complaint/" + item);
                        if (resault.ResultAs<Complaint>() != null)
                        {
                            list.Add(resault.ResultAs<Complaint>());
                        }
                    });
                    i++;
                }
                Task.WaitAll(tasks);
                list.Sort(Comparer<Complaint>.Create((x, y) => (int.Parse(x.id).CompareTo(int.Parse(y.id)))));
                return list;
            }
            catch
            {
                return null;
            }
        }
        public List<Promotion> GetPromotions()
        {
            try
            {
                this.testCon();
                var resault = client.Get("Promotion");
                if (resault.ResultAs<List<Promotion>>() != null)
                {
                    var list = resault.ResultAs<List<Promotion>>();
                    if (list.Contains(null)) list.Remove(null);
                    return list;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public List<String> getFurnitureComplaintList(string id)
        {
            this.testCon();
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
            this.testCon();
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
        public string getPromotionLastId()
        {
            this.testCon();
            var resault = client.Get("Promotionid/id");
            var res = resault.ResultAs<string>();
            if (res != null)
            {
                client.Set("Promotionid/id", (int.Parse(res) + 1).ToString());
                return (int.Parse(res) + 1).ToString();
            }
            client.Set("Promotionid/id", "0");
            return 0.ToString();
        }
        public Furniture getFurniture(string id)
        {
            try
            {
                this.testCon();
                var resault = client.Get("Furniture/" + id);
                if (resault == null) return null;
                else return resault.ResultAs<Furniture>();
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
                this.testCon();
                var resault = client.Get("Login/" + login.login);
                UserLogins res = resault.ResultAs<UserLogins>();
                if (res != null)
                {
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
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }
        public int checkLoginPossibility(UserLogins login)
        {
            try
            {
                this.testCon();
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
                this.testCon();
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
        public int testCon()
        {
            var response = Task.Run(() => client.GetAsync("test"));
            try
            {
                if (response.Result.ResultAs<int>() == 1)
                {
                    this.connection = true;
                    return 1;
                }
                else this.connection = false;
            }
            catch
            {
                this.connection = false;
                return 0;
            }
            return 0;
        }
        public bool AddToWhishList(string promotionID, string user, bool delete)
        {

            try
            {
                List<String> promotionWhishList = null;
                List<String> userWhishList = null;
                this.testCon();
                Task[] tasks = new Task[2];
                tasks[0] = Task.Run(async () =>
                {
                    var resPromotionWhishList = await client.GetAsync("Promotion/" + promotionID + "/WhishList");
                    if (resPromotionWhishList.ResultAs<List<String>>() != null)
                    {
                        promotionWhishList = resPromotionWhishList.ResultAs<List<String>>();
                    }
                    else promotionWhishList = new List<String>();
                });
                tasks[1] = Task.Run(async () =>
                {
                    var resUserWhishList = await client.GetAsync("Login/" + user + "/WhishList");
                    if (resUserWhishList.ResultAs<List<String>>() != null)
                    {
                        userWhishList = resUserWhishList.ResultAs<List<String>>();
                    }
                    else userWhishList = new List<String>();
                });
                Task.WaitAll(tasks);
                if (delete == false)
                {
                    if (!promotionWhishList.Contains(user)) promotionWhishList.Add(user);
                    if (!userWhishList.Contains(promotionID)) userWhishList.Add(promotionID);
                }
                else
                {
                    if (promotionWhishList.Contains(user)) promotionWhishList.Remove(user);
                    if (userWhishList.Contains(promotionID)) userWhishList.Remove(promotionID);
                }
                Task[] tasks_data_insert = new Task[2];
                tasks_data_insert[0] = Task.Run(async () =>
                {
                    var resPromotionWhishList = await client.SetAsync("Promotion/" + promotionID.ToString() + "/WhishList", promotionWhishList);
                });
                tasks_data_insert[1] = Task.Run(async () =>
                {
                    var resPromotionWhishList = await client.SetAsync("Login/" + user + "/WhishList", userWhishList);
                });
                Task.WaitAll(tasks_data_insert);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Promotion> GetWhishList(string user)
        {
            try
            {
                this.testCon();
                var resUserWhishList = client.Get("Login/" + user + "/WhishList");
                var userWhishList = resUserWhishList.ResultAs<List<int>>();
                List<Promotion> whishList = new List<Promotion>();
                int i = 0;
                if (userWhishList != null)
                {
                    Task[] tasks = new Task[userWhishList.Count];
                    foreach (int id in userWhishList)
                    {
                        tasks[i] = Task.Run(async () =>
                        {
                            var resPromotionWhishList = await client.GetAsync("Promotion/" + id.ToString() + "/");
                            if (resPromotionWhishList.ResultAs<Promotion>() != null)
                            {
                                whishList.Add(resPromotionWhishList.ResultAs<Promotion>());
                            }
                        });
                        i++;
                    }
                    Task.WaitAll(tasks);
                }
                return whishList;
            }
            catch
            {
                return null;
            }
        }
    }
}