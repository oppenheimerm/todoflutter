using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;

namespace TodoFlutter.webapi.Models
{

    [Serializable]
    public static class DataExtensions
    {
        public static AppUserVM ToAppUserVm(this AppUser  appuser)
        {
            return new AppUserVM()
            {
                Firstname = appuser.Firstname,
                Email = appuser.Email
            };
        }

        public static ExchangeRefreshTokenRequest ToExchangeRefreshTokenRequest(
            this ExchangeRefreshTokenRequestInput exchangeRefreshTokenRequestInput,
            string signInKey)
        {
            return new ExchangeRefreshTokenRequest(
                exchangeRefreshTokenRequestInput.AccessToken,
                exchangeRefreshTokenRequestInput.RefreshToken,
                signInKey
                )
            {};
        }

    }
}
