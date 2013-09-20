/*
 * Constants.cs
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
using System.Text;

namespace PluginVK
{
    /// <summary>
    /// Contains various constants used by project.
	/// </summary>
	internal static class Constants
	{
		#region Content
        internal static string data_name = "Data.tmp";
        internal static string onlineusers_name = "OnlineUsers.tmp";
        internal static string dir = Environment.GetEnvironmentVariable("TEMP") + "\\";
        internal static string path_data = dir + data_name;
        internal static string path_onlineusers = dir + onlineusers_name;
        internal static int count = 5;
		#endregion

		#region Configuration
		/// <summary>
		/// Directory where all settings are stored.
		/// </summary>
		internal const string SettingsDirectory = "settings";
		/// <summary>
		/// File containing info about the git commit from which this version was built.
		/// </summary>
		internal static string GitInfoFile { get { return SettingsDirectory + "\\gitinfo"; } }
		#endregion

		#region Data
		/// <summary>
		/// Directory where various data is stored.
		/// </summary>
		internal const string DataDirectory = "data";
		#endregion
	}
}
