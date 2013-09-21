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
using System.IO;
using System.Text;
using System.Threading;

namespace PluginVK
{
    public class Verification
    {
        string crypted_id = null;
        string id = null;
        string token = null;

        public void Main()
        {
            #region Dir
            // Проверка файла на существование.
            if (!Directory.Exists(Constants.dir))
            {
                Directory.CreateDirectory(Constants.dir);
            }
            if (!File.Exists(Constants.path_data))
            {
                using (FileStream stream = File.Create(Constants.path_data)) { }
            }
            #endregion

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
                OAuth.OAuthRun();
            }
            else
            {
                Get g = new Get();
                g.token = token;
                g.id = id;
                g.GetInfo();
                //Thread t = new Thread(new ThreadStart(g.GetInfo));
                //t.Start();
                //t.Join();
            }

        }
    }
}
