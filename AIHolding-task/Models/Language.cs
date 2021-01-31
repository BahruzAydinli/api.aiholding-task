using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Language
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Locale { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }
}
