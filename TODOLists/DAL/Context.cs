using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TODOLists.Models;

namespace TODOLists.DAL
{
    public class ToDoContext : DbContext
    {
        public ToDoContext() : base("TODOListDBEntities")
        {
        }

        public DbSet<Tbl_User> Users { get; set; }
        public DbSet<Tbl_Task> Tasks { get; set; }
    }
}