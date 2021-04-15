using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.GatwayResponses.Repositories;

namespace TodoFlutter.data
{
    public interface IToDoService
    {
        Task<Todo> GetByIdAsync(int id);
        Task<GetToDoResponse> GetToDoForUserById(string userId, int todoId);
        Todo Update(Todo todo);
        Task<AddToDoResponse> AddToDoAsync(Todo todo);
        Task<Todo> DeleteAsync(int id);
        Task<IEnumerable<Todo>> GetAllToDosByUserAsync(string userId);
        IEnumerable<Todo> Find(Expression<Func<Todo, bool>> predicate);
        Task<int> CommitAsync();
        Task<int> GetTodosByUserCountAsync(string userId);
    }
}
