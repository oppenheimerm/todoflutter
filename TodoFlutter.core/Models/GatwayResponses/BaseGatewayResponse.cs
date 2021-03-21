using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.core.Models.GatwayResponses
{
    public abstract class BaseGatewayResponse
    {

        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }


        public string Message { get; }

        protected BaseGatewayResponse(bool success = false, IEnumerable<Error> errors = null, string message = null)
        {
            Success = success;
            Errors = errors;
            Message = message;            
        }
    }
}
