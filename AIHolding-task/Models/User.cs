using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }

        public bool Confirmed { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime Registered { get; set; }


        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public Role Role { get; set; }


        [ForeignKey("Media")]
        public int? PhotoID { get; set; }
        public Media Photo { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
