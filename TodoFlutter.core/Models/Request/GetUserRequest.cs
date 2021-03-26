using System.ComponentModel.DataAnnotations;

namespace TodoFlutter.core.Models.Request
{
    public class GetUserRequest
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
