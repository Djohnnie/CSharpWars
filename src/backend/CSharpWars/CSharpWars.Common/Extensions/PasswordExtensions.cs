using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CSharpWars.Common.Extensions
{
    public static class PasswordExtensions
    {
        public static (String Salt, String Hashed) HashPassword(this String password, String salt = null)
        {
            if (salt == null)
            {
                var saltData = new byte[16];
                using (var randomNumberGenerator = RandomNumberGenerator.Create())
                {
                    randomNumberGenerator.GetBytes(saltData);
                }

                salt = Convert.ToBase64String(saltData);
            }

            var x = KeyDerivation.Pbkdf2(password: password, salt: Convert.FromBase64String(salt), prf: KeyDerivationPrf.HMACSHA1, iterationCount: 10000, numBytesRequested: 32);
            var hashed = Convert.ToBase64String(x);

            return (salt, hashed);
        }
    }
}