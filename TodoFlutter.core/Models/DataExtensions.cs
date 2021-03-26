using System;
using System.Collections.Generic;
using System.Linq;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;
using TodoFlutter.core.Models.Request;
using TodoFlutter.core.Models.Request.ToDo;

namespace TodoFlutter.core.Models
{

    [Serializable]
    public static class DataExtensions
    {
        public static UserDTO ToUserDTO(this AppUser appuser)
        {
            return new UserDTO()
            {
                FirstName = appuser.Firstname,
                Email = appuser.Email,
                UserID = appuser.Id.ToString()
            };
        }

        public static List<ToDoDTO> ToToDoList(this IEnumerable<Todo> list)
        {
            return list.Select( t => new ToDoDTO { 
                Id = t.Id,
                Task = t.Task,
                Completed = t.Completed,
                Date = t.Date,
                UserId = t.UserId
            }).ToList();
        }

        public static Todo ToToDo(this ToDoRequest todo, string userId)
        {
            return new Todo
            {
                //  Id
                Task = todo.Task,
                Completed = todo.Completed,
                Date = DateTime.Now.ToUniversalTime(),
                UserId = userId
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
