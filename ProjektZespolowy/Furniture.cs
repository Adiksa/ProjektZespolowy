using Android.Graphics;
using Android.Widget;

namespace ProjektZespolowy
{
    class Furniture
    {
        public string id { get; set; }
        public string warentyImage { get; set; }
        public string warentyText { get; set; }
        public string specImage { get; set; }
        public string specText { get; set; }

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
            return true;
        }
    }
}