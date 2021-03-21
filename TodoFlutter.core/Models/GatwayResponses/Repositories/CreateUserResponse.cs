using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class CreateUserResponse : BaseGatewayResponse
    {
        //  UserId
        public string Id { get; }
        public CreateUserResponse(
            string id, 
            bool success = false, 
            IEnumerable<Error> errors = null, 
            string message = null) : base(success, errors, message)
        {
            Id = id;
        }
    }
}
