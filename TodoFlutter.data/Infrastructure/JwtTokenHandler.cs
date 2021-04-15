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
using TodoFlutter.data.Infrastructure.Helpers;

namespace TodoFlutter.data.Infrastructure
{
    /// <summary>
    /// Handles token validation. <see cref="ValidateToken(string, TokenValidationParameters)"/>
    /// </summary>
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenHandler> _logger;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public JwtTokenHandler(ILogger<JwtTokenHandler> logger, IConfiguration configuration)
        {
            if (_jwtSecurityTokenHandler == null)
                _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            _logger = logger;
            _configuration = configuration;
            _jwtTokenHelper = new JwtTokenHelper();
        }

        public string WriteToken(JwtSecurityToken jwt)
        {
            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }
    }
}
