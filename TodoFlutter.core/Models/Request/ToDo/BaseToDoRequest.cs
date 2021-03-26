using System.ComponentModel.DataAnnotations;

namespace TodoFlutter.core.Models.Request.ToDo
{
    public class BaseToDoRequest
    {
        [Required]
        public string AccessToken { get; set; }

        public BaseToDoRequest(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
