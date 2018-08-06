using Microsoft.EntityFrameworkCore;

namespace core2Bowling.Models
{
    public class AccountContext :DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }

        public DbSet<UserIdentity> UserIdentities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserIdentity>().ToTable("UserIdentity");
        }
    }
}
