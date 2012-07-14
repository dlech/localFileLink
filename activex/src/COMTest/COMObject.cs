using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace COMTest
{
    [System.Runtime.InteropServices.Guid("0162FB34-E8A1-4222-969A-53FE85C60901")]
    public interface ICOMObject
    {
        string COMObjectFunction();
    }

    [System.Runtime.InteropServices.Guid("B5CC31BB-752B-406e-8C22-4A1045F4E5D0")]
    public class COMObject : ICOMObject
    {
        public string COMObjectFunction()
        {
            return "Hello, Lady!";
        }
    }
}
