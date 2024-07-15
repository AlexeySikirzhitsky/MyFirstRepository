using Library.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.DB
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
