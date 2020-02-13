using System;
using System.Text;

namespace CSharpWars.Common.Extensions
{
    public static class Base64Extensions
    {
        public static string Base64Encode(this string data)
        {
            if (data == null) return null;
            var bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(this string encodedData)
        {
            if (encodedData == null) return null;
            var bytes = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}