using Microsoft.EntityFrameworkCore;
using TC.Repository.Entity;

namespace TC.Repository.Context;
public class TestTCDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public TestTCDbContext(DbContextOptions<TestTCDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        for (int i = 1; i <= 5; ++i)
        {
            modelBuilder.Entity<Priority>().HasData(new Priority { Id = i,  Level = i });
        }

        modelBuilder.Entity<ToDoItem>()
            .HasOne(td => td.User)
            .WithMany(u => u.TodoItems)
            .HasForeignKey(td => td.UserId);

        modelBuilder.Entity<ToDoItem>()
            .HasOne(td => td.Priority)
            .WithMany(p => p.TodoItems)
            .HasForeignKey(td => td.PriorityId);

        modelBuilder.Entity<Priority>()
            .HasIndex(p => p.Level)
            .IsUnique();
    }
}

