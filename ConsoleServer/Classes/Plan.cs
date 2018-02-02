using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Classes
{
   public class Plan
    {
        public int Id { set; get; }
        public string Image { set; get; }
        public string Name { set; get; }
        public int Importance { set; get; }
        public string Note { set; get; }
        public string Time { set; get; }
        public string Date { set; get; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
