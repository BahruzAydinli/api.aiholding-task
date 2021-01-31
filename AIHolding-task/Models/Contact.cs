using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Subject { get; set; }

        [Required]
        [MinLength(20)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
