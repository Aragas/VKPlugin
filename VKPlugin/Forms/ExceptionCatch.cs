/*
 * ExeptionCatch.cs
 * 
 * Copyright © 2011-2012 by Sijmen Schoon and Adam Hellberg.
 * 
 * This file is part of Sharpcraft.
 * 
 * Sharpcraft is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Sharpcraft is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Sharpcraft.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * Disclaimer: Sharpcraft is in no way affiliated with Mojang AB and/or
 * any of its employees and/or licensors.
 * Sijmen Schoon and Adam Hellberg do not take responsibility for
 * any harm caused, direct or indirect, to your Minecraft account
 * via the use of Sharpcraft.
 * 
 * "Minecraft" is a trademark of Mojang AB.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VKPlugin.Forms;

namespace PluginVK
{
    class ExceptionCatch
    {
        private const string ExceptionFile = @"logs\exception.log";
        private static log4net.ILog _log;

        private static void WriteExceptionToFile(Exception ex)
        {
            try
            {
                var writer = new StreamWriter(ExceptionFile, false);
                string date = DateTime.Now.ToString();
                writer.WriteLine("Fatal exception occurred at " + date);
                writer.WriteLine("The exception thrown was " + ex.GetType());
                writer.WriteLine("ToString() => " + ex);
                writer.WriteLine("Exception message: " + ex.Message);
                writer.WriteLine("Stack Trace:\n" + ex.StackTrace);
                writer.WriteLine("\nDone writing exception info.");
                writer.Flush();
                writer.Close();
            }
            catch (IOException)
            {
                _log.Error("Unable to write exception info to file.");
            }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            _log.Fatal("Unknown exception " + ex.GetType() + " thrown. Writing exception info to logs\\exception.log");
            WriteExceptionToFile(ex);
            string author = null;
            using (var reader = new StreamReader(Constants.GitInfoFile))
            {
                var readLine = reader.ReadLine();
                if (readLine != null)
                {
                    string[] fields = readLine.Split(':');
                    if (fields.Length >= 3)
                    {
                        author = fields[2];
                        if (author.Contains(" "))
                            author = author.Split(' ')[0];
                    }
                }
            }
            new ExceptionDialog(ex, author).ShowDialog();
        }

    }
}
