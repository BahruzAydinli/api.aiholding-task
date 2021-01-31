using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Action
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Key { get; set; }
    }
}
