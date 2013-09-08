using System;
using System.IO;
using System.Text;

namespace PluginVK
{
    static class Verification
    {
        // Главные параметры конфигурции.
        public static string data_name = "Data.rmg";
        public static string onlineusers_name = "OnlineUsers.rmg";
        public static string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rainmeter\\Plugins\\VKPugin\\";
        public static string path_data = dir + data_name;
        public static string path_onlineusers = dir + onlineusers_name;
        public static int count = 5;

        public static void Main()
        {
            // Первичные значения (Из-за using).
            string id = "";
            string token = "";

            // Проверка файла на существование.
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(path_data))
            {
                using (FileStream stream = File.Create(path_data)) { }
            }

            // Чтение параметров.
            using (StreamReader sr = new StreamReader(path_data, Encoding.UTF8))
            {
                id = Crypto.Decrypt(sr.ReadLine(), "ididitjustforlulz");
                token = Crypto.Decrypt(sr.ReadLine(), "ididitjustforlulz");
            }

            // Проверка существования данных.
            if (id == null)
            {
                OAuth.OAuthRun();
            }
            else
            {
                Check.CheckRun(token, id, path_onlineusers, count);
            }

        }

    }
}
