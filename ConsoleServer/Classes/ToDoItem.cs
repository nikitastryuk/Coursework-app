using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Classes
{
    public class ToDoItem
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public bool IsChecked { set; get; } 
        public int? ToDoDateId { get; set; }
        public ToDoDate ToDoDate { get; set; }
        
    }
}
