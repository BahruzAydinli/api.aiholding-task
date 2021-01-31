using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Models
{
    public class Translation
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("Article")]
        public int? ArticleID { get; set; }
        public Article Article { get; set; }

        [ForeignKey("Language")]
        public int LanguageID { get; set; }
        public Language Language { get; set; }
    }
}
