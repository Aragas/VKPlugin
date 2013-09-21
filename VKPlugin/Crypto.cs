﻿/*
  Copyright (C) 2011 Bartez

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
using System.Security.Cryptography;
using System.Text;

namespace PluginVK
{
    static class Crypto
    {
        // Encryption.
        public static string Encrypt(string str, string keyCrypt)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(str), keyCrypt));
        }

        // Decryption.
        // Return "error" by error.
        public static string Decrypt(string str, string keyCrypt)
        {
            if (str != null)
            {
                using (CryptoStream Cs = InternalDecrypt(Convert.FromBase64String(str), keyCrypt))
                using (StreamReader Sr = new StreamReader(Cs))
                {
                    return Sr.ReadToEnd();
                }
            }
            else
            {
                return null;
            }
        }

        private static byte[] Encrypt(byte[] key, string value)
        {
            using (SymmetricAlgorithm Sa = Rijndael.Create())
            using (ICryptoTransform Ct = Sa.CreateEncryptor((new PasswordDeriveBytes(value, null)).GetBytes(16), new byte[16]))
            using (MemoryStream Ms = new MemoryStream())
            using (CryptoStream Cs = new CryptoStream(Ms, Ct, CryptoStreamMode.Write))
            {

                Cs.Write(key, 0, key.Length);
                Cs.FlushFinalBlock();

                byte[] Result = Ms.ToArray();
                return Result;
            }
        }

        private static CryptoStream InternalDecrypt(byte[] key, string value)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor((new PasswordDeriveBytes(value, null)).GetBytes(16), new byte[16]);

            MemoryStream ms = new MemoryStream(key);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
}
