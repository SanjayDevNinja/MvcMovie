using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcUserContext : DbContext
    {
        public MvcUserContext(DbContextOptions<MvcUserContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
