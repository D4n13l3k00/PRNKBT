using System;
using System.IO;
using System.Management;
using System.Reflection;

namespace PRNKBT.Requirements
{
    internal class Helper
    {
        public static string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }
        public static string GetHwid() // Works
        {
            string HoldingAdress = "";
            try
            {
                string drive = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1);
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
                disk.Get();
                string diskLetter = (disk["VolumeSerialNumber"].ToString());
                HoldingAdress = diskLetter;

            }
            catch (Exception)
            {

            }

            return HoldingAdress;
        }
    }
}
