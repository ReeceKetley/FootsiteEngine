using System;
using System.Collections.Generic;
using FootsiteEngine.Configuration;
using FootsiteEngine.Footlocker;
using FootsiteEngine.Items;
using FootsiteEngine.Modules;
using FootsiteEngine.Types;

namespace FootsiteEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            FootLockerCore footLockerCore = new FootLockerCore("https://www.footlocker.co.uk/en/product/adidas-stan-smith-womenshoes/315345874002.html", "40 2/3");
            Console.ReadLine();
        }
    }
}
