using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;
using TodoFlutter.data.Helpers;

namespace TodoFlutter.data
{
    public interface IAccountService
    {
        Task<CreateUserResponse> CreateUserAsync(string userName, string emailaddress, string Firsname, string password);
        Task<CreateUserLoginResponse> AuthenticateUserAsync(string username, string password, string ipAddress);
        Task AddRefreshTokenAsync(string refreshToken, Guid appUserId, string remoteIPAddress);
        Task RemoveOldRefreshTokensAsync(string userId);
        Task<RefreshTokenRespone> ExchangeRefreshTokenAsync(string ipAddress, string refreshToken);
    }
}
