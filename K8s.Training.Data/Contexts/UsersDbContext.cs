using Microsoft.EntityFrameworkCore;
using System.Reflection;
using K8s.Training.Domain.Entities;

namespace K8s.Training.Data.Contexts
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
               .Property(e => e.FirstName)
               .HasMaxLength(50);

            modelBuilder.Entity<User>()
               .Property(e => e.LastName)
               .HasMaxLength(50);

            modelBuilder.Entity<User>()
               .Property(e => e.PhoneNumber)
               .HasMaxLength(50);

            _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}