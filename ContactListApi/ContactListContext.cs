using ContactListApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi
{
    public class ContactListContext : DbContext
    {
        public ContactListContext(DbContextOptions<ContactListContext> options) : base(options)
        {
        }
        public DbSet<ContactListItem> ContactListItems { get; set; }
    }
}
