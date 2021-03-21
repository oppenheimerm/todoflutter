using System.Collections.Generic;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class RefreshTokenRespone : BaseGatewayResponse
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }

        public RefreshTokenRespone(
            AccessToken accessToken, 
            string refreshToken, 
            bool success = false,
            IEnumerable<Error> errors = null,
            string message = null) : base(success, errors, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
