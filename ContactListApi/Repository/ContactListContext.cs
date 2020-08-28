using ContactListApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi.Repository
{
    public class ContactListContext : DbContext
    {
        public ContactListContext(DbContextOptions<ContactListContext> options) : base(options)
        {
        }
        public DbSet<ContactListItem> ContactListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
