using Microsoft.EntityFrameworkCore;

namespace Expenses_Tracker.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LoginViewModel> LoginViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<LoginViewModel>(entity => {
                entity.HasKey(k => k.id);
            });
        }
    }
}
