using System.ComponentModel.DataAnnotations;

namespace ToDoFlutter.Api.Models.Request
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RemoteIpAddress { get; }
    }
}
