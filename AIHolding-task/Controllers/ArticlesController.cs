using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIHolding_task.Data;
using AIHolding_task.Models;
using Microsoft.AspNetCore.Authorization;
using AIHolding_task.DTOs.Write;
using AIHolding_task.DTOs.Read;

namespace AIHolding_task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly DataContext _context;

        public ArticlesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<List<ArticleReadDto>>> GetArticles()
        {
            var lang = base.Request.Headers["accept-language"].Count > 0 ? base.Request.Headers["accept-language"][0] : "az";
            List<ArticleReadDto> articles = await _context.Articles.Include(a=>a.User).Include(a => a.Translations).Where(a=> !a.Deleted).Select(ar => new ArticleReadDto
            {
                Author = ar.User.Name + " " + ar.User.Surname,
                Title = _context.Translations.FirstOrDefault(t=> t.Article.ID == ar.ID && t.Language.Locale == lang).Title,
                Description = _context.Translations.FirstOrDefault(t => t.Article.ID == ar.ID && t.Language.Locale == lang).Description,
                ID = ar.ID,
                PublishedAt = ar.PublishedAt.ToString(),
                isPremium = ar.IsPremium
            }).ToListAsync();
            return articles;
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.ID)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(ArticleDto article)
        {
            Article a = new Article { IsPremium = article.isPremium, Deleted = false, User = _context.Users.First(), PublishedAt = DateTime.Now };
            _context.Articles.Add(a);
            await _context.SaveChangesAsync();
            article.Content.ForEach(c =>
            {
                _context.Translations.Add(new Translation { Title = c.Title, Description = c.Description, Language = _context.Languages.FirstOrDefault(l => l.Locale == c.Locale), Article = a});
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null || article.Deleted)
            {
                return NotFound();
            }
            article.Deleted = true;

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ID == id);
        }
    }
}
