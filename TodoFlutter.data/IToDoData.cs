using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.GatwayResponses.Repositories;

namespace TodoFlutter.data
{
    public interface IToDoData
    {
        //  Users
        Task<CreateUserResponse> CreateUserAsync(string userName, string emailaddress, string Firsname, string password);
        //  Refresh Tokens
        Task<RefreshTokenRespone> ExchangeRefreshTokenAsync(ExchangeRefreshTokenRequest exchangeRefreshToken);
        //  ToDo's
        Task<Todo> GetByIdAsync(int id);
        Todo Update(Todo todo);
        Task<Todo> AddAsync(Todo todo);
        Task<Todo> DeleteAsync(int id);
        Task<IEnumerable<Todo>> GetAllToDosByUserAsync(string userId);
        IEnumerable<Todo> Find(Expression<Func<Todo, bool>> predicate);
        Task<int> CommitAsync();
        Task<int> GetTodosByUserCountAsync(string userId);
        Task AddRefreshTokenAsync(string refreshToken, Guid appUserId, string remoteIPAddress, double daysToExpire = 5);
        Task<CreateUserLoginResponse> LoginUserAsync(string username, string password, string ipAddress);
    }
}
