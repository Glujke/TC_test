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
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoItem>()
            .HasOne(td => td.User)
            .WithMany(u => u.TodoItems)
            .HasForeignKey(td => td.UserId);

        modelBuilder.Entity<ToDoItem>()
            .HasOne(td => td.Priority)
            .WithMany(p => p.TodoItems)
            .HasForeignKey(td => td.PriorityId);
    }
}

