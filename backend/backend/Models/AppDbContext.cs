using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using backend.Models; 

namespace backend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd(); 

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(t => t.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(t => t.ChildId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // Конфигурация для User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityAlwaysColumn();
            });
        }
    }
}