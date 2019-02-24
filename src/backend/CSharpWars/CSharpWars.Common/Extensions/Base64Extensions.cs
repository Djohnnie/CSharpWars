using System;
using System.Text;

namespace CSharpWars.Common.Extensions
{
    public static class Base64Extensions
    {
        public static String Base64Encode(this String data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(this String encodedData)
        {
            var bytes = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}