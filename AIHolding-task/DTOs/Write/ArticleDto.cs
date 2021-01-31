using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.DTOs.Write
{
        public class ArticleLanguageDto
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Locale { get; set; }
        }

        public class ArticleDto
        {
            public List<ArticleLanguageDto> Content { get; set; }
            public bool isPremium { get; set; }
        }
}
