using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoAPi_Dapper.Models;
using TodoAPi_Dapper.Models.Persistance;

namespace TodoAPi_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController: ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public TodoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            if(_unitOfWork.TodoItems.Count() == 0)
            {
                _unitOfWork.TodoItems.Add(new TodoItem { Name = "Item1", IsComplete = false });
            }
        }

        // GET: api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return Ok(await _unitOfWork.TodoItems.FindAllAsync());
        }

        // GET: api/todo/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(String id)
        {
            var todoItem = await _unitOfWork.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            await _unitOfWork.TodoItems.Add(todoItem);

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/todo/1
        [HttpPut]
        public async Task<IActionResult> PutTodoItem(String id, TodoItem todoItem)
        {
            await _unitOfWork.TodoItems.Update(todoItem);

            return NoContent();
        }

        // DELETE: api/todo/1
        [HttpDelete]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(String id)
        {
            var todoItem = await _unitOfWork.TodoItems.FindAsync(id);

            if(todoItem == null)
            {
                return NotFound();
            }

            await _unitOfWork.TodoItems.Remove(todoItem);

            return todoItem;
        }
    }
}
