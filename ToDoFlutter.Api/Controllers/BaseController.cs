using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;

namespace ToDoFlutter.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public AppUser AppUser => (AppUser)HttpContext.Items["AppUser"];
    }
}
