﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TodoFlutter.data.Infrastructure
{
    public interface IJwtTokenHandler
    {
        string WriteToken(JwtSecurityToken jwt);        
    }
}
