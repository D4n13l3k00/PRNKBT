using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Media;

namespace PRNKBT.Funcitons
{
    class Flash
    {
        private static SoundPlayer tfl = new SoundPlayer(Properties.Resources.thflash);
        private static SoundPlayer fl = new SoundPlayer(Properties.Resources.flash);
        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        internal static void Do(Color col) {
            tfl.PlaySync();
            fl.Play();
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(desktopPtr);
            for(int i = 0; i < 500; i++)
            {
                g.Clear(col);
                Thread.Sleep(10);
            }
            g.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }
    }
}
