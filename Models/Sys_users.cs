using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBTEST.Controllers
{
    public class Sys_users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int DepartId { get; set; }
        public bool CanLogin { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsShow { get; set; }
        public string Mail { get; set; }
    }



    public class UserInfo
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public int DepartId { get; set; }
        public bool CanLogin { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsShow { get; set; }
        public string Mail { get; set; }
    }
}
