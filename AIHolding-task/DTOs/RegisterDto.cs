using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool isPremium { get; set; }
        public int RenewalType { get; set; } = 1;
    }
}
