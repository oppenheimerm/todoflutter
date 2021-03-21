using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.data.Infrastructure
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenHandler> _logger;

        public JwtTokenHandler(ILogger<JwtTokenHandler> logger, IConfiguration configuration)
        {
            if (_jwtSecurityTokenHandler == null)
                _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            _logger = logger;
            _configuration = configuration;
        }

        public string WriteToken(JwtSecurityToken jwt)
        {
            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }

        public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            try
            {
                /*var principal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;*/
                var key = Encoding.ASCII.GetBytes(_configuration["AuthSettings:SecretKey"]);
                var issuer = _configuration["JwtIssuerOptions:Issuer"];
                var audience = _configuration["JwtIssuerOptions:Audience"];

                var principal = _jwtSecurityTokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer.ToString(),
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero,

                    }, out SecurityToken validToken);

                if (!(validToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch (Exception e)
            {
                _logger.LogError($"Token validation failed: {e.Message}");
                return null;
            }
        }
    }
}
