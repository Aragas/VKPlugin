/*
  Copyright (C) 2013 Aragas (Aragasas)

  This program is free software; you can redistribute it and/or
  modify it under the terms of the GNU General Public License
  as published by the Free Software Foundation; either version 2
  of the License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PluginVK.Forms
{
    public partial class OAuth : Form
    {
        public string _token { get; set; }
        public string _id { get; set; }

        public OAuth()
        {
            InitializeComponent();


            // Переход по ссылке.
            string url = "https://oauth.vk.com/authorize?client_id=3328403"
                + "&redirect_uri=https://oauth.vk.com/blank.html"
                + "&scope=friends,messages&display=popup&response_type=token";
            webBrowser1.Navigate(url);
            return;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Изъятие из URL строки.
            string url = webBrowser1.Url.ToString();
            string l = url.Split('#')[1];

            // Нахождение токена, временя действия токена и id.
            _token = l.Split('&')[0].Split('=')[1];
            _id = l.Split('=')[3];
            Crypto cr = new Crypto();
            string crypto_token = cr.Encrypt(_token, "ididitjustforlulz");
            string crypto_id = cr.Encrypt(_id, "ididitjustforlulz");

            using (FileStream fs = File.OpenWrite(Constants.path_data))
            {
                // Перевод id в байты.
                string idnl = crypto_id + Environment.NewLine;
                byte[] idbyte = UTF8Encoding.UTF8.GetBytes(idnl);

                // Запись id в файл.
                fs.Write(idbyte, 0, idbyte.Length);

                // Создание байтового токена.
                byte[] info =
                    new UTF8Encoding(true).GetBytes(crypto_token);

                // Запись токена в файл.
                fs.Write(info, 0, info.Length);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Verification.GetInfo();
            this.Close();
        }
    }
}
