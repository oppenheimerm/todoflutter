using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.data.Infrastructure;
using TodoFlutter.data.Infrastructure.Helpers;

namespace TodoFlutter.data.Infrastructure
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly IConfiguration _configuration;

        public JwtFactory(
            IJwtTokenHandler jwtTokenHandler,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _jwtTokenHelper = new JwtTokenHelper();
            _jwtOptions = new JwtOptions();
        }

        public async Task<AccessToken> GenerateEncodedToken(string userId, string userName, string email)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenHelper.AuthSettingSecretKey.Value));
            var signInCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var identity = GenerateClaimsIdentity(userId, userName);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Email, email),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 new Claim(JwtRegisteredClaimNames.Aud, _jwtTokenHelper.JwtAudience.Value),
                 new Claim(JwtRegisteredClaimNames.Iss, _jwtTokenHelper.JwtIssuer.Value),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer:_jwtTokenHelper.JwtIssuer.Value,
                audience:_jwtTokenHelper.JwtAudience.Value,
                claims:claims,
                notBefore:_jwtOptions.NotBefore,
                expires:_jwtOptions.Expiration, // 2 hours from now
                signingCredentials:signInCredentials);

            var _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            return new AccessToken(_jwtSecurityTokenHandler.WriteToken(jwt), (int)_jwtOptions.ValidFor.TotalSeconds);
        }

        private static ClaimsIdentity GenerateClaimsIdentity(string id, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id),
            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

    }
}
