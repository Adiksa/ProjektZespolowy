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

namespace ProjektZespolowy
{
    class UserLogins
    {
        public string login { get; set; }
        public string userPassword { get; set; }
        public bool admin { get; set; }
        public List<String> WhishList { get; set; }
    }
}