using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class BowlingContext : DbContext
    {

        public BowlingContext(DbContextOptions<BowlingContext> options) : base(options)
        {
        }

        public DbSet<Bowler> Bowlers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<SubGame> SubGames { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bowler>().ToTable("Bolwer");
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<SubGame>().ToTable("SubGame");
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<TeamMember>().ToTable("TeamMember");

        }
    }
}
