using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FootsiteEngine.Types
{
    public class FootlockerProduct
    {
        public string Name { get; }
        public string Brand { get; }
        public string Description { get; }
        public string Model { get; }
        public string Sku { get; }
        public Uri Image { get; }
        public Uri Url { get; }
        public List<FootLockerSize> Sizes { get; }

        public FootlockerProduct(string name, string brand, string description, string model, string sku, Uri image, Uri url, List<FootLockerSize> sizes)
        {
            Name = name;
            Brand = brand;
            Description = description;
            Model = model;
            Sku = sku;
            Image = image;
            Url = url;
            Sizes = sizes;
        }

    }
}
