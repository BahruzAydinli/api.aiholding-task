using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Confirmation
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public DateTime Expiration { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User{get;set;}
    }
}
