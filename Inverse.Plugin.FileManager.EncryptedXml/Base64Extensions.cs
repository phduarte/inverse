using System;
using System.Text;

namespace Inverse.Plugin.FileManager.EncryptedXml
{
    internal static class Base64Extensions
    {
        public static string ToBase64(this string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string FromBase64(this string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return base64EncodedData;

            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return base64EncodedData;
            }
        }

        public static bool IsBase64String(this string text)
        {
            if (text is null)
                return false;

            Span<byte> buffer = new Span<byte>(new byte[text.Length]);
            return Convert.TryFromBase64String(text, buffer, out var _);
        }
    }
}