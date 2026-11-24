namespace TaskFlow.Api.Data;

using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Project> Projects { get; set; }
}