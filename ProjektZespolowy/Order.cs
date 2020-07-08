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
    public class Order
    {
        public Promotion product { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public void priceCount()
        {
            this.price = Double.Parse(this.product.price)*this.amount;
        }
        public static List<Order> AddToList(List<Order> list, Promotion promotion)
        {
            if (list != null)
            {
                foreach(Order o in list)
                {
                    if(o.product == promotion)
                    {
                        o.amount++;
                        o.priceCount();
                        return list;
                    }
                }
            }
            else list = new List<Order>();
            Order order = new Order()
            {
                product = promotion,
                amount = 1,
                price = Double.Parse(promotion.price.Replace('.',','))
            };
            list.Add(order);
            return list;
        }
    }
    
    
}