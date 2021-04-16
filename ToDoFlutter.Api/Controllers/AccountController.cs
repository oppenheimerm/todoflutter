using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.data;
using ToDoFlutter.Api.Models.Request;
using ToDoFlutter.Api.Helpers;
using TodoFlutter.core.Models.Request;

namespace ToDoFlutter.Api.Controllers
{
    //  Specify the route. Read below as api/auth/...
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _iaccountService;
        private readonly IToDoService _toDoService;

        public AccountController(
            IAccountService iaccountService,
            IToDoService toDoService
            )
        {
            _iaccountService = iaccountService;
            _toDoService = toDoService;
        }


        // POST api/account/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var loginResponse = await _iaccountService.AuthenticateUserAsync(
                model.Email,
                model.Password,
                ipAddress()
                );

            return loginResponse.Success ? Ok(loginResponse) : BadRequest(loginResponse);
        }

        // GET api/account/user
        [Authorize(AppUserRole.User)]
        [HttpGet("user")]
        public async Task<ActionResult> GetById(string userId)
        {
            bool isGuidValid = Guid.TryParse(userId, out var guidOutput);

            if (!isGuidValid)
                return BadRequest();

            if (guidOutput.ToString() != AppUser.Id)
                return Unauthorized(new { message = "Unauthorized" });


            var userResponse = await _iaccountService.GetUserProfile(userId, true);

            return userResponse.Success ? Ok(userResponse) : BadRequest(User);
        }

        // POST api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Register model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //  var to hold our CreateUserResponse
            var userResponse = await _iaccountService.CreateUserAsync(model.Email, model.Email, model.FirstName, model.Password);
                        
            return userResponse.Success ? Ok(userResponse) : BadRequest(userResponse);


        }

        // POST api/account/refreshtoken
        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody] ExchangeRefreshTokenRequestInput model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState);}

            var refreshTokenResponse = await _iaccountService.ExchangeRefreshTokenAsync(
                ipAddress(),
                model.RefreshToken
                );
            return refreshTokenResponse.Success ? Ok(refreshTokenResponse) : BadRequest(refreshTokenResponse);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
