using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.data;
using TodoFlutter.data.Infrastructure.Helpers;

namespace ToDoFlutter.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly AppSettings _appSettings;
        private readonly JwtTokenHelper _jwtTokenHelper;


        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
            _jwtTokenHelper = new JwtTokenHelper();
        }

        public async Task Invoke(HttpContext context, AppDbContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, AppDbContext dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtTokenHelper.AuthSettingSecretKey.Value);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtTokenHelper.JwtIssuer.Value,
                    ValidAudience = _jwtTokenHelper.JwtAudience.Value,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);


                //  NEED TO DO SOME NULL CHECKING HERE!!!
                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;
                //return principal;                
                var user = await dataContext.Users.FindAsync(accountId); //_userManager.FindByIdAsync(accountId.ToString());
                if (user != null)
                {
                    // attach account to context on successful jwt validation
                    context.Items["AppUser"] = user;
                }
            }
            catch(Exception err)
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
                var error = err.Message.ToString();
            }
        }
    }
}
