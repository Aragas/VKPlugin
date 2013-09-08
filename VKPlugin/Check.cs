using System.IO;

namespace VKPlugin
{
    static class Check
    {
        public static void CheckRun(string token, string id, string path, int count)
        {
            string text1 = Friends.ConvertFriendsOnline(Friends.ParseFriendsOnline(token, id, count), path);
            if (text1 == "error")
            {
                Aauth.AauthRun();
            }

            string text2 = Messages.UnReadMessages(token, id);
            if (text2 == "error")
            {
                Aauth.AauthRun();
            }

            string text = text1 + text2 + "&";


            // Проверка файла на существование.
            if (File.Exists(path))
            {
                File.Delete(path);
                using (FileStream stream = File.Create(path)) { }
            }
            else
            {
                using (FileStream stream = File.Create(path)) { }
            }

            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.Write(text);
            }
        }

    }
}
