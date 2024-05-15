using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext :IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
        
    }

    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentGroup>()
            .HasKey(sg => new { sg.GroupId, sg.StudentId });
        
        base.OnModelCreating(modelBuilder);
    }
}