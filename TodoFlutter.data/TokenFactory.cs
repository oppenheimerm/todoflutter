using System;
using System.Security.Cryptography;

namespace TodoFlutter.data
{
    public class TokenFactory : ITokenFactory
    {
        /// <summary>
        /// Creates a cryptographically fortified random value which is used for
        /// generating a refresh token.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
