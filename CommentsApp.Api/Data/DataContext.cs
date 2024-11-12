using CommentsApp.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsApp.Api.Data;

public sealed class DataContext : DbContext
{
    public DbSet<User> Users { get; private set; }
    public DbSet<Comment> Comments { get; private set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}