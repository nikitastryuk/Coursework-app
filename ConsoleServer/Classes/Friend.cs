using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Classes
{
    public class Friend
    {
        public int Id { set; get; }
        public string Email { set; get; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
