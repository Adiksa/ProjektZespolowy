﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ProjektZespolowy
{
    public class Complaint
    {
        public string id { get; set; }
        public string description { get; set; }
        public List<string> photo { get; set; }
        public string furnitureId { get; set; }
        public List<string> complaintProgress{ get; set; }
        public string senderName { get; set; }
        public string madeBy { get; set; }

        public bool Correct()
        {
            if (description == null || description == "")
                return false;
            if (photo.Count == 0)
            {
                return false;
            }
            if (furnitureId == null)
                return false;
            if (madeBy == null)
                return false;
            return true;
        }
        public static Bitmap convertBase64ToBitmap(String b64)
        {
            byte[] imageAsBytes = Convert.FromBase64String(b64);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
    }
}