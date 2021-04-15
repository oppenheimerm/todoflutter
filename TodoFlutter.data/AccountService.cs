﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;
using TodoFlutter.core.Models.Request;
using TodoFlutter.data.Helpers;

namespace TodoFlutter.data
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenFactory _tokenFactory;
        private readonly AppDbContext _appDbContext;
        private readonly IJwtFactory _jwtFactory;
        private readonly IConfiguration _configuration;

        public AccountService(
            UserManager<AppUser> userManager,
            ITokenFactory tokenFactory,
            AppDbContext appDbContext,
            IJwtFactory jwtFactory,
            IConfiguration configuration,
            ILogger<AccountService> logger
            )
        {
            _userManager = userManager;
            _tokenFactory = tokenFactory;
            _appDbContext = appDbContext;
            _jwtFactory = jwtFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CreateUserResponse> CreateUserAsync(string userName, string emailaddress, string Firsname, string password)
        {
            var appUser = new AppUser { Email = emailaddress, UserName = userName, Firstname = Firsname };
            //  To fix String and byte array keys are not client-generated by default errof
            //  See:https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.x/breaking-changes#string-and-byte-array-keys-are-not-client-generated-by-default
            appUser.Id = Guid.NewGuid().ToString();

            // add to user role
            appUser.UserRole = AppUserRole.User;

            var identityResult = await _userManager.CreateAsync(appUser, password);

            //  We have a probelm...
            if (!identityResult.Succeeded)
                return new CreateUserResponse(
                    appUser.Id,
                    false,
                    identityResult.Errors.Select(e => new Error(e.Code, e.Description)),
                    ResponseMessageTypes.USER_CREATED_FAILURE
                    );

            //  No issue, so add user to database
            _logger.LogInformation($"User{appUser.Email} account created");

            return new CreateUserResponse(
                appUser.Id,
                identityResult.Succeeded,
                identityResult.Succeeded ? null : identityResult.Errors.Select(e => new Error(e.Code, e.Description)),
                ResponseMessageTypes.USER_CREATED_SUCCESS);

        }


        public async Task<CreateUserLoginResponse> AuthenticateUserAsync(string username, string password, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if (user != null)
            {
                //  Validate password
                var result = await _userManager.CheckPasswordAsync(user, password);
                if (result)
                {

                    //  Generate refresh token
                    var refreshToken = _tokenFactory.GenerateToken();
                    //  Add refresh token for user
                    await AddRefreshTokenAsync(
                        refreshToken,
                        Guid.Parse(user.Id),
                        ipAddress);

                    //  Remove old refresh tokens
                    await RemoveOldRefreshTokensAsync(user.Id);

                    //  Generate accesss token
                    return new CreateUserLoginResponse(
                        user.Email,
                        user.Firstname,
                        await _jwtFactory.GenerateEncodedToken(
                            user.Id,
                            user.UserName,
                            user.Email
                            ),
                        refreshToken,
                        true,
                        null,
                        ResponseMessageTypes.USER_LOGIN_SUCCESS
                     );
                }
                //  Error
            }

            //error
            return new CreateUserLoginResponse(
                string.Empty,
                string.Empty,
                null,
                null,
                false,
                new[] { new Error("login_failure", "Invalid username or password.") }.ToList(),
                ResponseMessageTypes.USER_LOGIN_FAILURE
             );
        }

        public async Task AddRefreshTokenAsync(
            string refreshToken,
            Guid appUserId,
            string remoteIPAddress
            )
        {
            var daysToExpire = int.Parse(_configuration["TokenSettings:DaysToExpire"]);
            RefreshToken newRefreshToken = new()
            {
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(daysToExpire),
                AppUserId = appUserId,
                RemoteIpAddress = remoteIPAddress
            };

            await _appDbContext.RefreshTokens.AddAsync(newRefreshToken);
            await _appDbContext.SaveChangesAsync();
            _logger.LogInformation($"RefreshToken: {refreshToken} added");
        }


        public async Task RemoveOldRefreshTokensAsync(string userId)
        {
            var staleTokens = await _appDbContext.RefreshTokens.Where(t => t.AppUserId.ToString() == userId).ToListAsync();
            var daysToExpire = double.Parse(_configuration["TokenSettings:DaysToExpire"]);

            if (staleTokens.Any())
            {
                //  ForEach is essentially fire-and-forget; it does not ensure all tasks run to completion.
                //   use a for loop in reverse, and keep the tasks so they can be awaited
                var task = new List<Task>();
                for (int i = staleTokens.Count - 1; i >= 0; i--)
                {
                    var staleToken = staleTokens[i];
                    if (!staleToken.Active)
                    {
                        task.Add(RemoveTokenAsync(staleToken));
                    }
                }

                await Task.WhenAll(task);
            }

            return;
        }

        private Task RemoveTokenAsync(RefreshToken token)
        {
            return Task.Run(() => {
                _logger.LogInformation($"RefreshToken: {token} removed");
                _appDbContext.RefreshTokens.Remove(token);
                _appDbContext.SaveChangesAsync();
                return;

            });
        }


        private async Task<AppUser> GetUserFromRefreshTokenAsync(string token)
        {
            var account = await _appDbContext.RefreshTokens.Include(a => a.AppUser).Where(t => t.Token == token).FirstOrDefaultAsync();
            var appUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == account.AppUserId.ToString());
            return appUser;
        }



        public async Task<RefreshTokenRespone> ExchangeRefreshTokenAsync(
            string ipAddress,
            string refreshToken
            )
        {

            var user = await GetUserFromRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                var failureRespone = new RefreshTokenRespone(
                    null,
                    null,
                    false,
                    new[] { new Error("refresh__token_failure", "Invalid or bad refresh token") }.ToList(),
                    ResponseMessageTypes.REFRESH_TOKEN_FAILURE
                    );
                return failureRespone;
            }   
            //  new jwttoken
            var jwtToken = await _jwtFactory.GenerateEncodedToken(user.Id, user.UserName, user.Email);
            var newRefreshToken = _tokenFactory.GenerateToken();
            // delete the old token we've exchanged
            //   remove old refresh tokens
            await RemoveOldRefreshTokensAsync(user.Id.ToString());
            // add the new one
            await AddRefreshTokenAsync(newRefreshToken, Guid.Parse(user.Id), ipAddress);
            var response = new RefreshTokenRespone(
                jwtToken,
                newRefreshToken,
                true,
                null,
                ResponseMessageTypes.REFRESH_TOKEN_SUCCESS);

            return response;
        }
    }
}