using Android.Graphics;
using Android.Widget;
using System;
using System.Buffers.Text;
using System.Collections.Generic;

namespace ProjektZespolowy
{
    public class Furniture
    {
        public string id { get; set; }
        public string warentyImage { get; set; }
        public string warentyText { get; set; }
        public string specImage { get; set; }
        public string specText { get; set; }
        public string name { get; set; }
        public List<string> complaintList { get; set; }

        public bool Correct()
        {
            if (this.id == null || this.id == "skanowanie" || this.id == "Włącz skanowanie" || this.id == "Błąd NFC" )
                return false;
            if (this.warentyText == "")
                return false;
            if (this.warentyImage == null)
                return false;
            if (this.specImage == null)
                return false;
            if (this.specText == "")
                return false;
            if (this.name == "")
                return false;
            return true;
        }
        public Bitmap convertBase64ToBitmap(String b64)
        {
            if(b64 != null)
            {
                byte[] imageAsBytes = Convert.FromBase64String(b64);
                return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
            }
            return null;
        }
    }
}