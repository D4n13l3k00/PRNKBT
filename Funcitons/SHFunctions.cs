using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PRNKBT.Funcitons
{
    class SHFunctions
    {
        private static IntPtr TaskbarHWnd;  // Описатель панели задач
        private static IntPtr TrayNotifyHWnd; // Описатель области уведомлений
        private static IntPtr ClockHwnd; // Описатель области системных часов
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string ClassName, string WindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(
            IntPtr hwndParent, IntPtr hwndChildAfter,
            string className, string windowName);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        internal static void TaskBar(int state)
        {
            TaskbarHWnd = FindWindow("Shell_TrayWnd", null);
            if (TaskbarHWnd != IntPtr.Zero)
            {
                ShowWindow(TaskbarHWnd, state);
            }
        }
        internal static void Clock(int state)
        {
            TaskbarHWnd = FindWindow("Shell_TrayWnd", null);
            TrayNotifyHWnd = FindWindowEx(TaskbarHWnd, IntPtr.Zero, "TrayNotifyWnd", null);
            ClockHwnd = FindWindowEx(TrayNotifyHWnd, IntPtr.Zero, "TrayClockWClass", null);
            ShowWindow(ClockHwnd, state);
        }
    }
}
