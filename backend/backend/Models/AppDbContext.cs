using backend.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public required DbSet<User> Users { get; set; }
    public required DbSet<TaskItem> Tasks { get; set; }
    public required DbSet<TaskList> TaskLists { get; set; }
    public required DbSet<FamilyCode> FamilyCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskList>(entity =>
        {
            entity.HasKey(e => e.ListId);
            entity.Property(e => e.ListId)
                .UseIdentityAlwaysColumn();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.ParentId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.ChildId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.TaskId);
            entity.Property(e => e.TaskId)
                .UseIdentityAlwaysColumn();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.ParentId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.ChildId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<TaskList>()
                .WithMany()
                .HasForeignKey(t => t.TaskListId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn();

            entity.HasIndex(e => e.Email)
                .IsUnique();
        });
        
        modelBuilder.Entity<FamilyCode>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Iыd)
                .UseIdentityAlwaysColumn();
	
            entity.HasOne(fc => fc.Parent)
                .WithMany(u => u.FamilyCodes)
                .HasForeignKey(fc => fc.ParentId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(fc => fc.Code)
                .IsUnique();
        });
    }
}