using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.webapi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoFlutter.data;
using TodoFlutter.core.Models.Request;


//  See: https://medium.com/swlh/securing-your-net-core-3-api-using-identity-93d6426d6311

namespace TodoFlutter.webapi.Controllers
{
    //  Specify the route. Read below as api/auth/...
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly LinkGenerator _linkgenerator;        
        private readonly IToDoData _itodoData;
        private readonly IConfiguration _configuration;
        /* Not using Roles for this app
        private readonly RoleManager<Role> _roleManager;*/

        public AccountController(
            IToDoData itodoData,
            LinkGenerator linkGenerator,
            IConfiguration configuration
            //RoleManager<Role> roleManager
            )
        {
            this._itodoData = itodoData;
            this._linkgenerator = linkGenerator;
            this._configuration = configuration;
            //this._roleManager = roleManager;
        }

        // POST api/account/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //  var to hold our CreateUserResponse
            var userResponse = await _itodoData.CreateUserAsync(model.Email, model.Email, model.FirstName, model.Password);
                        
            return userResponse.Success ? Ok(userResponse) : BadRequest(userResponse);


        }

        // POST api/account/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var loginResponse = await _itodoData.LoginUserAsync(
                model.Email,
                model.Password,
                Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                );

            return loginResponse.Success ? Ok(loginResponse) : BadRequest(loginResponse);
        }

        // POST api/account/refreshtoken
        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody] ExchangeRefreshTokenRequestInput model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState);}

            var refreshTokenResponse = await _itodoData.ExchangeRefreshTokenAsync(
                DataExtensions.ToExchangeRefreshTokenRequest(
                    model, _configuration["AuthSettings:SecretKey"]
                    )
                );
            return refreshTokenResponse.Success ? Ok(refreshTokenResponse) : BadRequest(refreshTokenResponse);
        }

        // POST api/account/user
        [HttpPost("user")]
        public async Task<ActionResult> GetUser([FromBody] GetUserRequest model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var response = await _itodoData.GetUserFromToken(model.AccessToken, _configuration["AuthSettings:SecretKey"]);
            return response.Success ? Ok(response) : BadRequest(response);
        }


    }
}
