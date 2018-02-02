using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Classes
{
    public class Client
    {
        public static int Counter { get; set; } = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public IClientCallback Callback { get; set; }
    }
}
