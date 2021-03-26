using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.core.Models.DTO;
using TodoFlutter.core.Models.GatwayResponses.Repositories;
using TodoFlutter.core.Models.Request.ToDo;
using TodoFlutter.data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoFlutter.webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoData _itodoData;
        private readonly LinkGenerator _linkgenerator;
        private readonly IConfiguration _configuration;

        public ToDosController(
            IToDoData itodoData,
            LinkGenerator linkgenerator,
            IConfiguration configuration)
        {
            _linkgenerator = linkgenerator;
            _itodoData = itodoData;
            _configuration = configuration;
        }

                
        // GET api/todos/all
        [HttpGet("all")]        
        public async Task<ActionResult<List<ToDoDTO>>> Get([FromBody]GetAllTodoRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            //  Get user
            var user = await _itodoData.GetUserFromToken(model.AccessToken, _configuration["AuthSettings:SecretKey"]);
            if(user.Success)
            {
                // get the requested date
                //  return loginResponse.Success ? Ok(loginResponse) : BadRequest(loginResponse);
                var response =  await _itodoData.GetAllToDosByUserAsync(user.User.UserID);
                return DataExtensions.ToToDoList(response);
            }
            return BadRequest(user);
        }

        // GET api/<ToDosController>/5
        [HttpGet("{todoId}")]
        public string Get(string todoId, string AccessToken)
        {
            return "value";
        }

        // POST api/<ToDosController>
        [HttpPost("create")]
        public async Task<ActionResult<AddToDoResponse>> Post([FromBody] ToDoRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            //  Get user
            var user = await _itodoData.GetUserFromToken(model.AccessToken, _configuration["AuthSettings:SecretKey"]);
            if(user.Success)
            {
                //  create new todo
                var todoResponse = await _itodoData.AddToDoAsync(DataExtensions.ToToDo(model, user.User.UserID));
                return todoResponse.Success ? Ok(todoResponse) : BadRequest(todoResponse);

            }
            return BadRequest(user);
        }

        // PUT api/<ToDosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ToDosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
