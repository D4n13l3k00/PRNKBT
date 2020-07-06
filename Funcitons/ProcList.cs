using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRNKBT.Funcitons
{
    internal class ProcList
    {
        internal static string get()
        {
            Process[] pcs = Process.GetProcesses();
            string psl = "Process list:\n\n PROC | ID | WINDOW_TITLE\n";
            foreach(Process i in pcs)
            {
                psl += "\n" + i.ProcessName + " | " + i.Id + " | " + i.MainWindowTitle;
            }
            return psl;
        }
    }
}
