using System;

namespace TodoFlutter.core.Models.Request.ToDo
{
    public class GetAllTodoRequest : BaseToDoRequest
    {
        public GetAllTodoRequest(string accessToken) : base(accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
