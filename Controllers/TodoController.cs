using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Persistance;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            //this creates a new todo item if its null

            if(_context.TodoItems.Count() == 0)
            {
                var toDoItem = new TodoItem{
                    Name = "Testing"
                };
                _context.TodoItems.Add(toDoItem); 
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> getTodDoItems()
        {
            var _toDoItems = await _context.TodoItems.ToListAsync();

            return _toDoItems;
        }

        [HttpGet("getItem/{id}")]
        public async Task<ActionResult<TodoItem>> getTodoItem(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if(item == null)
            {
                return NotFound();
            }
            
            return item;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> SaveTodoItem(TodoItem item)
        {
           if(ModelState.IsValid){
               var result = _context.TodoItems.Add(item);
               await _context.SaveChangesAsync();
            
                return Ok();
           }
            return BadRequest();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var item = _context.TodoItems.Find(id);
            if(item == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }
    
    }
}