using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;
using TodoFlutter.core.Models.Request.ToDo;
using TodoFlutter.data;
using ToDoFlutter.Api.Helpers;

namespace ToDoFlutter.Api.Controllers
{
    //  Specify the route. Read below as api/todo/...
    [Route("api/[controller]")]
    public class ToDoController : BaseController
    {
        private readonly IToDoService _toDoService;

        public ToDoController(
            IToDoService toDoService
            )
        {
            _toDoService = toDoService;
        }


        // POST api/<ToDosController>
        [HttpPost("create")]
        [Authorize(AppUserRole.User)]
        public async Task<ActionResult<AddToDoResponse>> Post([FromBody] ToDoRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            bool isValid = Guid.TryParse(model.UserId, out var guidOutput);

            if(!isValid)
                return BadRequest(model);

            if (guidOutput.ToString() != AppUser.Id)
            return Unauthorized(new { message = "Unauthorized" });

            //  create new todo
            var todoResponse = await _toDoService.AddToDoAsync(DataExtensions.ToToDo(model, AppUser.Id));
            return todoResponse.Success ? Ok(todoResponse) : BadRequest(todoResponse);
        }

        // GET api/todos/get
        [HttpGet("GetById")]
        [Authorize(AppUserRole.User)]
        public async Task<ActionResult<GetToDoResponse>> GetById(int? id, string userId)
        {

            bool isGuidValid = Guid.TryParse(userId, out var guidOutput);
            bool isIdValid = id.HasValue ? true : false;

            if(!isGuidValid || !isIdValid)
                return BadRequest();

            if (guidOutput.ToString() != AppUser.Id)
                return Unauthorized(new { message = "Unauthorized" });

            var response = await _toDoService.GetToDoForUserById(userId, id.Value);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // GET api/todos/all
        [HttpGet("all")]
        [Authorize(AppUserRole.User)]
        public async Task<ActionResult<List<ToDoDTO>>> Get(string userId)
        {
            bool isGuidValid = Guid.TryParse(userId, out var guidOutput);
            if (!isGuidValid)
                return BadRequest();


            if (guidOutput.ToString() != AppUser.Id)
                return Unauthorized(new { message = "Unauthorized" });

            var response = await _toDoService.GetAllToDosByUserAsync(userId);
            return DataExtensions.ToToDoList(response);
        }

    }
}
