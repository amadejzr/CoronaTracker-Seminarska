using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class CoronaContext : IdentityDbContext<Uporabnik>
    {
        public CoronaContext(DbContextOptions<CoronaContext> options) : base(options)
        {
        }

        public DbSet<Uporabnik> Uporabniki { get; set; }
        public DbSet<Odlok> Odloki { get; set; }

        public DbSet<Stik> Stiki { get; set; }
        public DbSet<Prebivalisce> Prebivalisca { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uporabnik>().ToTable("Uporabnik");
            modelBuilder.Entity<Odlok>().ToTable("Odlok");
            modelBuilder.Entity<Stik>().ToTable("Stik");
            modelBuilder.Entity<Prebivalisce>().ToTable("Prebivalisce");
            base.OnModelCreating(modelBuilder);
            
        }
    }
}