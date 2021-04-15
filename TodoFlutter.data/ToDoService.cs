using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;
using TodoFlutter.data.Helpers;

namespace TodoFlutter.data
{
    public class ToDoService : IToDoService
    {
        private readonly ILogger<ToDoService> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly LinkGenerator _linkgenerator;

        public ToDoService(
            ILogger<ToDoService> logger,
            AppDbContext appDbContext,
            LinkGenerator linkgenerator
            )
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _linkgenerator = linkgenerator;
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _appDbContext.ToDos.FindAsync(id);
        }

        /// <summary>
        /// This is the action that users will interact with. The reason
        /// for the distinction from <see cref="GetByIdAsync(int)"/>, it
        /// we need to preform some checking, that the requestor  userId
        /// passed in, is equal to userId of the todo entity
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="todoId"></param>
        /// <returns></returns>
        public async Task<GetToDoResponse> GetToDoForUserById(string userId, int todoId)
        {
            var todo = await GetByIdAsync(todoId);
            if(todo != null)
            {
                if (todo.UserId != userId)
                {
                    var response = new GetToDoResponse(
                        0,
                        string.Empty,
                        false,
                        false,
                        new[] { new Error("authorization error", "not authorized to view this todo") }.ToList(),
                        ResponseMessageTypes.ACTION_NOT_AUTHORIZED
                        );

                    return response;
                }
                else
                {
                    var response = new GetToDoResponse(
                        todo.Id,
                        todo.Task,
                        todo.Completed,
                        true,
                        null,
                        ResponseMessageTypes.GET_TODO_SUCCESS
                        );

                    return response;
                }
            }
            else
            {
                var response = new GetToDoResponse(
                    0,
                    string.Empty,
                    false,
                    false,
                    null,
                    ResponseMessageTypes.GET_TODO_FAILURE
                    );

                return response;
            }

        }

        public Todo Update(Todo todo)
        {
            var entity = _appDbContext.ToDos.Attach(todo);
            entity.State = EntityState.Modified;
            return todo;
        }

        public async Task<AddToDoResponse> AddToDoAsync(Todo todo)
        {
            try
            {
                // Create a new to
                var newTodo = await _appDbContext.AddAsync(todo);
                await _appDbContext.SaveChangesAsync();
                _logger.LogInformation($"Todo added");

                //  Using our _linkGenerator object
                var resourceLocation = _linkgenerator.GetPathByAction(
                    "GetById",
                    "todo",
                    new { id = newTodo.Entity.Id, UserId = todo.UserId });



                var response = new AddToDoResponse(
                    todo.Id.ToString(),
                    resourceLocation,
                    true,
                    null,
                    ResponseMessageTypes.ADD_TODO_SUCCESS
                    );

                return response;
            }
            catch
            {

                var response = new AddToDoResponse(
                    string.Empty,
                    string.Empty,
                    false,
                    new[] { new Error("refresh__token_failure", "Invalid or bad refresh token") }.ToList(),
                    ResponseMessageTypes.ADD_TODO_FAILURE
                    );

                return response;
            }

        }


        public async Task<Todo> DeleteAsync(int id)
        {
            var todoToDelete = await GetByIdAsync(id);
            if (todoToDelete != null)
            {
                _appDbContext.ToDos.Remove(todoToDelete);
            }
            return todoToDelete;
        }

        public async Task<IEnumerable<Todo>> GetAllToDosByUserAsync(string userId)
        {
            var query = await _appDbContext.ToDos
                .Where(t => t.User.Id == userId)
                .OrderBy(t => t.Date)
                .ToListAsync();

            return query;
        }

        public IEnumerable<Todo> Find(Expression<Func<Todo, bool>> predicate)
        {
            return _appDbContext.Set<Todo>().Where(predicate);
        }

        public async Task<int> CommitAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> GetTodosByUserCountAsync(string userId)
        {
            var todos = await _appDbContext.ToDos
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return todos.Count;
        }
    }
}
