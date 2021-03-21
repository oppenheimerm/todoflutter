using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;

namespace TodoFlutter.webapi.Controllers
{
    //  Specify the route. Read below as api/auth/...
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly LinkGenerator _linkgenerator;

        public AuthController(UserManager<AppUser> userManager, LinkGenerator linkgenerator)
        {
            this.userManager = userManager;
            _linkgenerator = linkgenerator;
        }

    }
}
