using System;
using System.Collections.Generic;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class GetUserFromTokenResponse : BaseGatewayResponse
    {

        public UserDTO  User {get;}
        public GetUserFromTokenResponse(UserDTO user, bool success = false,
            IEnumerable<Error> errors = null,
            string message = null) : base(success, errors, message)
        {
            User = user;
        }
    }
}
