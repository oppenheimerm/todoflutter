using System;
using System.Collections.Generic;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class GetUserResponse : BaseGatewayResponse
    {
        public string Email { get; }
        public string Firstname { get; }
        public string UserId { get; }

        public IEnumerable<ToDoDTO> ToDoDTOs { get; set; }

        public GetUserResponse(
            string email,
            string firstName,
            string userId,
            bool success = false,
            IEnumerable<Error> errors = null,
            string message = null
            ):base(success, errors, message)
        {
            Email = email;
            Firstname = firstName;
            UserId = userId;
        }
    }
}
