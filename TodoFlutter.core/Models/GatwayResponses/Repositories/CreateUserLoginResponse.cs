using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class CreateUserLoginResponse : BaseGatewayResponse
    {
        //  User's access token
        public AccessToken AccessToken { get; }
        //  User's refresh token
        public string RefreshToken { get; }

        public string Email { get;  }
        public string Firstname { get; }

        public Guid UserId { get; }

        public CreateUserLoginResponse(
            Guid id,
            string email,
            string firstname,
            AccessToken accessToken, 
            string refreshToken, 
            bool success = false, 
            IEnumerable<Error> errors = null, 
            string message = null) : base(success, errors, message
        )

        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Email = email;
            Firstname = firstname;
            UserId = id;
        }
    }
}
