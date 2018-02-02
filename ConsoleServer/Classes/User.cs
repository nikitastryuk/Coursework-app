using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; } = "/Images/noUser.jpg";
        public string Facebook { get; set; } = "facebook.com";
        public string Twitter { get; set; } = "twitter.com";
        public string Github { get; set; } = "github.com";
        public string Vk { get; set; } = "vk.com";

        public ICollection<Friend> Friends { get; set; }
        public ICollection<Plan> Plans { get; set; }
        public ICollection<ToDoDate> Dates { get; set; }
        public User()
        {
            Friends = new List<Friend>();
            Plans = new List<Plan>();
        }
    }
}
