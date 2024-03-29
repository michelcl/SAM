﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SAM.Util.Criptografia
{
    public class CriptografiaSimples
    {
        private static byte[] key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private static byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };
        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public CriptografiaSimples()
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Criptografar(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Descriptografar(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public string CriptografarParaUrl(string unencrypted)
        {
            return HttpUtility.UrlEncode(Criptografar(unencrypted));
        }

        public string DescriptografarDaUrl(string encrypted)
        {
            return Descriptografar(HttpUtility.UrlDecode(encrypted));
        }

        private byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        private byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        private byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
