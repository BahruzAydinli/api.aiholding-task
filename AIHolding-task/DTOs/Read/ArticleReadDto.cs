using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.DTOs.Read
{
    public class ArticleReadDto
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PublishedAt { get; set; }
        public bool isPremium { get; set; }
    }
}
