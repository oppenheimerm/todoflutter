using System;
using System.Collections.Generic;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses.Repositories
{
    public class GetToDoResponse : BaseGatewayResponse
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool Completed { get; set; }


        public GetToDoResponse(
            int id,
            string task,
            bool completed,
            bool success = false,
            IEnumerable<Error> errors = null,
            string message = null) : base(success, errors, message
            )
        {
            Id = id;
            Task = task;
            Completed = completed;
        }
    }
}
