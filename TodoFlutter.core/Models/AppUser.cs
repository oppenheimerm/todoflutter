using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoFlutter.core.Models
{
    //  AppUserRole to avoid clashing with  base types
    public enum AppUserRole { Admin, User}

    public class AppUser :  IdentityUser<string>
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<Todo> ToDos { get; set; }

        public AppUserRole UserRole { get; set; }
    }

}
