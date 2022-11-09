using Microsoft.EntityFrameworkCore;
using WhatToDoApi.Models;

namespace WhatToDoApi.Data
{
    public class TasksDbContext:DbContext
    {
        public TasksDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Tasks> Tasks_ { get; set; }
    }
}
