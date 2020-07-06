using System;
using System.Diagnostics;
using System.Threading;

namespace PRNKBT
{
    class Blocker
    {
        internal static bool state = false;
        internal static void BlockThread()
        {

            while (true)
            {
                if (state)
                {
                    Process[] processlist = Process.GetProcesses();
                    foreach (Process process in processlist)
                    {
                        int processID = process.Id;
                        string windowTitle = process.MainWindowTitle;
                        string processName = process.ProcessName;
                        if (!String.IsNullOrEmpty(windowTitle))
                        {
                            foreach (string word in Config.blacklist)
                            {
                                if (windowTitle.ToUpper().Contains(word.ToUpper()))
                                {
                                    try
                                    {
                                        Process blacklistedProgram = Process.GetProcessById(processID);
                                        blacklistedProgram.Kill();
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    Thread.Sleep(1500);
                }
                else {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
