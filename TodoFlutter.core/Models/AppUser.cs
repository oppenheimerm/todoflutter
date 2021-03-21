using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoFlutter.core.Models
{
    public class AppUser :  IdentityUser<string>
    {
        public string Firstname { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }

}
