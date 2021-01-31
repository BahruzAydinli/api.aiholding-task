using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Media
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Folder { get; set; }

        [Required]
        public string File { get; set; }

        [ForeignKey("Article")]
        public int? ArticleID { get; set; }
        public Article Article { get; set; }
    }
}