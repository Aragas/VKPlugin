using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VKPlugin
{
    static class Friends
    {
        public static string ParseFriendsOnline(string token, string id, int count)
        {
            // Первичные значения (Из-за using).
            string DocumentString = "";

            // Параметры конфигурации.
            string method = "friends.get.xml?";
            string param = "uid=" + id + "&order=hints" + "&fields=first_name,last_name,photo_50,online";

            //Получение документа.
            XmlDocument doc = new XmlDocument();
            doc.Load("https://api.vk.com/method/" + method + param + "&access_token=" + token);

            // Создание Node списков.
            XmlNodeList nodeList;
            XmlNodeList nodeListError;
            XmlNode root = doc.DocumentElement;

            nodeList = root.SelectNodes("/response/user[online='1']"); // Параметр, по которому идет фильтрация.
            nodeListError = root.SelectNodes("error_code"); // Для выявления ошибочного запроса.

            // Выявление ошибочного запрса.
            string sucheck = "";
            string sucheckerror = "<error_code>5</error_code>";

            foreach (XmlNode node in nodeListError)
            {
                sucheck = node.OuterXml;
            }

            if (sucheck == sucheckerror)
            {
                return "error";
            }

            // Фильтрация документа по параметру.
            using (Stream DocumentStream = new MemoryStream())
            {
                // Запись главной ветки (Нужно для Xml парсера).
                string r1 = "<main>";
                byte[] rb1 = Encoding.UTF8.GetBytes(r1);
                DocumentStream.Write(rb1, 0, rb1.Length);

                int x = 0;
                foreach (XmlNode node in nodeList)
                {
                    string susers = node.OuterXml;
                    byte[] ubytes = Encoding.UTF8.GetBytes(susers);
                    DocumentStream.Write(ubytes, 0, ubytes.Length);

                    x = x + 1;
                    if (x == count) break;
                }

                // Запись главной ветки (Нужно для Xml парсера).
                string r2 = "</main>";
                byte[] rb2 = Encoding.UTF8.GetBytes(r2);
                DocumentStream.Write(rb2, 0, rb2.Length);

                // Конвертирование MemoryStream в байты.
                Byte[] byteArray = new byte[DocumentStream.Length];
                DocumentStream.Position = 0;
                DocumentStream.Read(byteArray, 0, (int)DocumentStream.Length);

                // Конвертирование байтов в string.
                DocumentString = Encoding.UTF8.GetString(byteArray);
            }

            return DocumentString;
        }

        public static string ConvertFriendsOnline(string document, string path)
        {
            if (document == "error")
            {
                return "error";
            }

            string text = "";
            // Создание потока и запись в него 
            // последующих пользователей онлайн.
            using (Stream onlinems = new MemoryStream())
            {
                XmlDocument doc0 = new XmlDocument();
                doc0.LoadXml(document);

                // Запись параметров пользователей.
                foreach (XmlNode node in doc0.SelectNodes("//user"))
                {
                    string space = "&";
                    string uids = node["uid"].InnerText;
                    string first = node["first_name"].InnerText;
                    string last = node["last_name"].InnerText;
                    string photo = node["photo_50"].InnerText;
                    string online_m = "";

                    // Проверка существования <online_mobile>.
                    if (node.SelectSingleNode("online_mobile") == null)
                    {
                        online_m = "0";
                    }

                    else
                    {
                        online_m = node["online_mobile"].InnerText;
                    }

                    byte[] ubytes1 = Encoding.UTF8.GetBytes(uids);
                    byte[] ubytes2 = Encoding.UTF8.GetBytes(first);
                    byte[] ubytes3 = Encoding.UTF8.GetBytes(last);
                    byte[] ubytes4 = Encoding.UTF8.GetBytes(photo);
                    byte[] ubytes5 = Encoding.UTF8.GetBytes(online_m);
                    byte[] ubytes6 = Encoding.UTF8.GetBytes(space);

                    onlinems.Write(ubytes6, 0, ubytes6.Length);
                    onlinems.Write(ubytes1, 0, ubytes1.Length);
                    onlinems.Write(ubytes6, 0, ubytes6.Length);
                    onlinems.Write(ubytes2, 0, ubytes2.Length);
                    onlinems.Write(ubytes6, 0, ubytes6.Length);
                    onlinems.Write(ubytes3, 0, ubytes3.Length);
                    onlinems.Write(ubytes6, 0, ubytes6.Length);
                    onlinems.Write(ubytes4, 0, ubytes4.Length);
                    onlinems.Write(ubytes6, 0, ubytes6.Length);
                    onlinems.Write(ubytes5, 0, ubytes5.Length);
                }

                // Добавление в конец последней &.
                string space1 = "&";
                byte[] ubytes51 = Encoding.UTF8.GetBytes(space1);
                onlinems.Write(ubytes51, 0, ubytes51.Length);

                using (StreamReader reader = new StreamReader(onlinems))
                {
                    onlinems.Position = 0;
                    text = reader.ReadToEnd();
                }

                return text;
            }
        }
    }
}
