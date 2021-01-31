using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.DTOs.Write
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class UserUpdateDto : UserDto
    {
        public int ID { get; set; }
    }
}
