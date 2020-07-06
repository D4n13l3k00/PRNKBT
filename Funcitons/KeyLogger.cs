using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRNKBT.Funcitons
{
    class KeyLogger
    {
        internal static bool state = false;
        internal static void Run()
        {
            while(true)
            {
                if(state)
                {
                    
                } else
                {
                    Thread.Sleep(5000);
                }
            }
        } 
    }
}
