using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatToDoApi.Data;
using WhatToDoApi.Models;

namespace WhatToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller 
    {
        
        private readonly TasksDbContext dbContext;

        public TaskController(TasksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetTasks()
        //{


        //    var response = await dbContext.Tasks_.ToListAsync();
        //    return Ok(response);
        //   // return Ok(await dbContext.Tasks_.ToListAsync());
        //}

      /*  [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await dbContext.Tasks_.Where(a => a.id == id).FirstOrDefaultAsync();
            return Ok(new Response<Tasks>(customer));
        }*/

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await dbContext.Tasks_
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await dbContext.Tasks_.CountAsync();
            return Ok(new PagedResponse<List<Tasks>>(pagedData, validFilter.PageNumber, validFilter.PageSize));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTask([FromRoute] Guid id)
        {
            var contact = await dbContext.Tasks_.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);

        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AddTasks addTasktRequest)
        {
            var task = new Tasks()
            {
                id = Guid.NewGuid(),
                title = addTasktRequest.title,
                description = addTasktRequest.description,
                date = addTasktRequest.date
                


            };

            await dbContext.Tasks_.AddAsync(task);
            await dbContext.SaveChangesAsync();


            return Ok(task);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, UpdateTasks updatetask)
        {
            var task = await dbContext.Tasks_.FindAsync(id);

            if (task != null)
            {
                task.title = updatetask.title;
                task.description = updatetask.description;
                task.date = updatetask.date;


                await dbContext.SaveChangesAsync();
                return Ok(task);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var task = await dbContext.Tasks_.FindAsync(id);
            if (task != null)
            {
                dbContext.Remove(task);
                await dbContext.SaveChangesAsync();

                return Ok(task);
            }
            return NotFound();
        }
    }
}
