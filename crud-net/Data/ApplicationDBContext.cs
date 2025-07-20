using crud_net.Models;
using Microsoft.EntityFrameworkCore;

namespace crud_net.Data;

public class ApplicationDBContext: DbContext
{
    public ApplicationDBContext(DbContextOptions options) : base(options) // just a constructor
    {
    }
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}