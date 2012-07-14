using System;
using System.Collections.Generic;
using System.Text;
using LocalFileLink;

namespace LocalFileLinkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ActiveX lfl = new ActiveX();
            lfl.OpenPath(@"C:\Users\davidl\Documents\Visual Studio 2010\Projects\LocalFileLink\localfilelink.vbs");
        }
    }
}
