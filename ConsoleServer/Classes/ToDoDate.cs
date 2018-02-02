using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Classes
{
    public class ToDoDate
    {
        public int Id { set; get; }
        public string Date { set; get; }
        public string Name { set; get; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
