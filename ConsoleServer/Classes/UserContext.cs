using ConsoleServer.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamMaterialWpf.Classes
{
   public class UserContext : DbContext
    {
        public UserContext(): base("DbConnection")
        { }

        public  DbSet<User> Users { get; set; }
        public  DbSet<Friend> Friends { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<ToDoDate> Dates { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
