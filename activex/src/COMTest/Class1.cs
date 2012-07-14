using System;
using System.Collections.Generic;
using System.Text;

namespace COMTest
{
    class Class1
    {
        static void Main(string[] args)
        {
            ICOMObject lfl = new COMObject();
            Console.WriteLine(lfl.COMObjectFunction());
        }
    }
}
