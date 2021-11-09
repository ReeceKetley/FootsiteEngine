using System;
using System.Collections.Generic;
using System.Text;

namespace FootsiteEngine.Various
{
    public static class CE
    {
        public static void WriteLine(string msg)
        {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] | " + msg);
        }
    }
}
