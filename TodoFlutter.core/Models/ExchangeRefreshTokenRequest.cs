using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.core.Models
{
    public class ExchangeRefreshTokenRequest
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public string SigningKey { get; }

        public ExchangeRefreshTokenRequest(string accessToken, string refreshToken, string signingKey)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            SigningKey = signingKey;
        }
    }
}
