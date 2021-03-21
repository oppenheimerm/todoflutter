using System.ComponentModel.DataAnnotations;

namespace TodoFlutter.webapi.Models
{
    public class BaseUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

