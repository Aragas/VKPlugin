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

using PluginVK.Forms;
using PluginVK.Methods;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PluginVK
{
    static class Verification
    {
        private static string crypted_id { get; set; }
        private static string id { get; set; }
        private static string token { get; set; }

        public static void Run()
        {
            // Проверка файла на существование.
            if (!Directory.Exists(Constants.dir))
            {
                Directory.CreateDirectory(Constants.dir);
            }
            if (!File.Exists(Constants.path_data))
            {
                using (FileStream stream = File.Create(Constants.path_data)) { }
            }

            // Чтение параметров.
            using (StreamReader sr = new StreamReader(Constants.path_data, Encoding.UTF8))
            {
                crypted_id = sr.ReadLine();

                if (crypted_id != null)
                {
                    Crypto cr = new Crypto();
                    id = cr.Decrypt(crypted_id, "ididitjustforlulz");
                    token = cr.Decrypt(sr.ReadLine(), "ididitjustforlulz");
                }
            }

            // Проверка существования данных.
            if (crypted_id == null)
            {
                OAuth oa = new OAuth();
                oa._token = token;
                oa._id = id;
                Application.Run(oa);
            }
            else
            {
                GetInfo();
            }

        }

        public static void GetInfo()
        {
            var path = Constants.path_onlineusers;
            string text = Friends() + Messages() + "&";
            if (Friends() == null || Messages() == null)
            {
                OAuth oa = new OAuth();
                oa._token = token;
                oa._id = id;
                Application.Run(oa);
            }

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

        public static string Friends()
        {
            Friends fr = new Friends();
            fr.token = token;
            fr.id = id;
            fr.path = Constants.path_onlineusers;
            fr.count = Constants.count;
            return fr.ConvertFriendsOnline();
        }

        public static string Messages()
        {
            Messages ms = new Messages();
            ms.token = token;
            ms.id = id;
            return ms.UnReadedMessages();
        }

    }
}
