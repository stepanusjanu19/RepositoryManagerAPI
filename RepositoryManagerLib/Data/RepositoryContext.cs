using Microsoft.EntityFrameworkCore;

namespace RepositoryManagerLib.Data
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }
        public DbSet<RepositoryItem> RepositoryItems { get; set; } = null!;
    }
}