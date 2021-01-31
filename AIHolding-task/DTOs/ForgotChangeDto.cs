using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.DTOs
{
    public class ForgotChangeDto
    {
        public string Token { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
