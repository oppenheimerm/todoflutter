using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.core.Models.DTO
{
    /// <summary>
    /// Plain old c# class that just returns the minimal
    /// <see cref="AppUser"/> properties
    /// </summary>
    public class UserDTO
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
