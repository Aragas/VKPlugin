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


            // Follow link.
            string url = "https://oauth.vk.com/authorize?client_id=3328403"
                + "&redirect_uri=https://oauth.vk.com/blank.html"
                + "&scope=friends,messages&display=popup&response_type=token";
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Get string from the URL.
            string url = webBrowser1.Url.ToString();
            string l = url.Split('#')[1];

            // Get token and user_id.
            token = l.Split('&')[0].Split('=')[1];
            id = l.Split('=')[3];

            // Data encrypting.
            string crypto_token = Crypto.Encrypt(token, "ididitjustforlulz");
            string crypto_id = Crypto.Encrypt(id, "ididitjustforlulz");

            using (FileStream fs = File.OpenWrite(Verification.path_data))
            {
                // Converting id to byte.
                id = crypto_id + Environment.NewLine;
                byte[] id_byte = UTF8Encoding.UTF8.GetBytes(id);

                // Recording id.
                fs.Write(id_byte, 0, id_byte.Length);

                // Converting token to byte.
                byte[] token_byte =
                    new UTF8Encoding(true).GetBytes(crypto_token);

                // Recording token.
                fs.Write(token_byte, 0, token_byte.Length);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetInfo.RunGetInfo(token, id, Verification.path_onlineusers, Verification.count);
            this.Close();
        }
    }
}
