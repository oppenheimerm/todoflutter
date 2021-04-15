using System.ComponentModel.DataAnnotations;

namespace TodoFlutter.core.Models.Request.ToDo
{
    public class BaseToDoRequest
    {
        [Required]
        public string UserId { get; set; }

        public BaseToDoRequest(string userId)
        {
            UserId = userId;
        }
    }
}
