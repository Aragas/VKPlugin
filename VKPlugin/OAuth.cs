using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PluginVK
{
    public partial class OAuth : Form
    {
        string token = "";
        string id = "";

        public static void OAuthRun()
        {
            Application.Run(new OAuth());
        }

        public OAuth()
        {
            InitializeComponent();


            // Переход по ссылке.
            string url = "https://oauth.vk.com/authorize?client_id=3328403"
                + "&redirect_uri=https://oauth.vk.com/blank.html"
                + "&scope=friends,messages&display=popup&response_type=token";
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string path_data = Verification.path_data;

            // Изъятие из URL строки.
            string url = webBrowser1.Url.ToString();
            string l = url.Split('#')[1];

            // Нахождение токена, временя действия токена и id.
            token = l.Split('&')[0].Split('=')[1];
            id = l.Split('=')[3];
            //string crypto_token = Crypto.Encrypt(token, "ididitjustforlulz");
            //string crypto_id = Crypto.Encrypt(id, "ididitjustforlulz");

            using (FileStream fs = File.OpenWrite(path_data))
            {
                // Перевод id в байты.
                string idnl = id + Environment.NewLine;
                byte[] idbyte = UTF8Encoding.UTF8.GetBytes(idnl);

                // Запись id в файл.
                fs.Write(idbyte, 0, idbyte.Length);

                // Создание байтового токена.
                byte[] info =
                    new UTF8Encoding(true).GetBytes(token);

                // Запись токена в файл.
                fs.Write(info, 0, info.Length);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Check.CheckRun(token, id, Verification.path_onlineusers, Verification.count);
            this.Close();
        }
    }
}
