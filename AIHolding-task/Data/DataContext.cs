using AIHolding_task.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Models.Action> Actions { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Media> Files { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Translation> Translations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Confirmation> Confirmations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
  }
}
