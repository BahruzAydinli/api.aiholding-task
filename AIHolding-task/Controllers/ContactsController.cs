using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIHolding_task.Data;
using AIHolding_task.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AIHolding_task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly DataContext _context;

        public ContactsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _context.Contacts.Where(c => !c.Deleted).ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null || contact.Deleted)
            {
                return NotFound();
            }

            return contact;
        }

        // POST: api/Contacts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            string htmlString = string.Format("<html><table border=\"1\"><thead><tr><th>Email</th><th>Ad</th><th>Soyad</th><th>Mövzu</th><th>Əlaqə nömrəsi</th><th>Mesaj</th></tr></thead><tbody><tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr></tbody></table></html>", contact.Email, contact.Name, contact.Surname, contact.Subject, contact.Phone, contact.Message);
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("info@3sual.az", "Bahruz Aydinli");
            var subject = "AIHoldingTask | Contact";
            var to = new EmailAddress("tmamedzadeh@gmail.com", "Taleh Məmmədzadə");
            var htmlContent = htmlString;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            await client.SendEmailAsync(msg);

            contact.CreatedAt = DateTime.Now;
            contact.Deleted = false;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.ID }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            contact.Deleted = true;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ID == id);
        }
    }
}
