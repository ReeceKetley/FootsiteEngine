using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FootsiteEngine.Types
{
    public class FootLockerSize
    {
        public FootLockerSize(string sku, double price, bool available, string size)
        {
            Sku = sku;
            Price = price;
            Available = available;
            Size = size;
        }

        public string Sku { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string Size { get; set; }
    }

}
