using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIHolding_task.Data;
using AIHolding_task.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIHolding_task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly DataContext _context;

        public OptionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("languages")]
        public IActionResult Languages()
        {
            return Ok(_context.Languages.ToList());
        }
    }
}
