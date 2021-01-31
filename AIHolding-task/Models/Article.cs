using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Article
    {
        [Key]
        public int ID { get; set; }

        public DateTime PublishedAt { get; set; }
        public bool Deleted { get; set; }
        public bool IsPremium { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }
}
