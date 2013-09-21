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

using System.IO;
using System.Threading;

namespace PluginVK
{
    static class GetInfo
    {
        public static void RunGetInfo(string token, string id, string path, int count)
        {
            string text1 = null;
            string text2 = null;

            #region Friends
            Thread t1 = new Thread(delegate()
            {
                text1 = Friends.ConvertFriendsOnline(Friends.ParseFriendsOnline(token, id, count), path);
            });
            t1.Start();
            #endregion

            #region Messages
            Thread t2 = new Thread(delegate()
            {
                text2 = Messages.UnReadMessages(token, id);
            });
            t2.Start();
            #endregion

            t1.Join();
            t2.Join();

            // Error checking.
            if (text1 == null || text2 == null)
            {
                OAuth.OAuthRun();
            }

            string text = text1 + text2 + "&";


            // Check if file exist..
            if (File.Exists(path))
            {
                File.Delete(path);
                using (FileStream stream = File.Create(path)) { }
            }
            else
            {
                using (FileStream stream = File.Create(path)) { }
            }

            // Saving data for Rainmeter WebParser.
            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.Write(text);
            }
        }

    }
}
