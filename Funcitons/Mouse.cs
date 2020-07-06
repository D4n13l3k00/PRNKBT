using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PRNKBT
{
    class Mouse
    {
        internal static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        internal static bool freeze_m = false;
        [DllImport("user32.dll")]
        internal static extern void SetCursorPos(int x, int y);
        internal static void freezethread()
        {
            while (true)
            {
                if (freeze_m)
                {
                    SetCursorPos(0, 0);
                    Thread.Sleep(1);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
        internal static void tpcursor()
        {
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            SetCursorPos(RandomNumber(0, resolution.Width), RandomNumber(0, resolution.Height));
        }
    }
}
