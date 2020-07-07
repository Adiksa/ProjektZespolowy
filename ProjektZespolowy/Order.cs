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
    class Order
    {
        private Promotion product { get; set; }
        private int amount { get; set; }
        private double price { get; set; }
    }
}