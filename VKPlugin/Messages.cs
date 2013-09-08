using System;
using System.Xml;

namespace VKPlugin
{
    public static class Messages
    {
        public static string UnReadedMessages(string token, string id)
        {
            // Параметры.
            string method = "messages.get.xml?";
            string param = "&filters=1";

            XmlDocument doc = new XmlDocument();
            doc.Load("https://api.vk.com/method/" + method + param + "&access_token=" + token);

            XmlNodeList nodeListError;
            XmlNode root = doc.DocumentElement;
            nodeListError = root.SelectNodes("error_code"); // Для выявления ошибочного запроса.

            // Выявление ошибочного запрса.
            string sucheck = "";
            string sucheckerror = "<error_code>5</error_code>";
            string sucheckerror2 = "<error_code>7</error_code>";

            foreach (XmlNode node in nodeListError)
            {
                sucheck = node.OuterXml;
            }

            if (sucheck == sucheckerror)
            {
                return "error";
            }

            if (sucheck == sucheckerror2)
            {
                return "error";
            }

            string countstring = "0";

            try
            {
                countstring = root["count"].InnerText;
            }
            catch { }

            int i = Convert.ToInt32(countstring);
            if (i > 1)
            {
                i = 1;
            }

            countstring = Convert.ToString(i);

            return countstring;
        }

    }
}
