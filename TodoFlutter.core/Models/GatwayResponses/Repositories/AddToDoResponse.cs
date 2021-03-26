
using System.Collections.Generic;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class AddToDoResponse : BaseGatewayResponse
    {
        public string ToDoId { get; set; }
        public string ResourceLocation { get; set; }

        public AddToDoResponse(
            string todoId,
            string resourceLocation,
            bool success = false,
            IEnumerable<Error> errors = null,
            string message = null
            ) : base(success, errors, message)
        {
            ToDoId = todoId;
            ResourceLocation = resourceLocation;
        }
    }
}
