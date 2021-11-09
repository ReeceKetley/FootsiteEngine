using System;
using System.Collections.Generic;
using System.Text;
using FootsiteEngine.Interfaces;
using FootsiteEngine.Types;

namespace FootsiteEngine.Footlocker
{
    public class AddToCartHandler
    {
        public FootlockerProduct Product { get; }
        public FootLockerSize Size { get; }
        private readonly IHttpServiceModule _httpService;

        public AddToCartHandler(IHttpServiceModule httpService, FootlockerProduct product, FootLockerSize size)
        {
            Product = product;
            Size = size;
            _httpService = httpService;
        }

        public bool AddToCart()
        {
            return false;
        }
    }
}
