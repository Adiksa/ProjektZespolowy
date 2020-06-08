using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    public class Promotion
    {
        public string id { get; set; }
        public string desc { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string price { get; set; }

        public bool Correct()
        {

            if (this.desc == null)
                return false;
            if (this.image == null)
                return false;
            if (this.title == null)
                return false;
            if (this.price == null)
                return false;
            
            return true;
        }
        public Bitmap convertBase64ToBitmap(String b64)
        {
            if (b64 != null)
            {
                byte[] imageAsBytes = Convert.FromBase64String(b64);
                return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
            }
            return null;
        }
    }
}