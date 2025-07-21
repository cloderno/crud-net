using crud_net.Models;
using Microsoft.EntityFrameworkCore;

namespace crud_net.Data;

public class ApplicationDBContext: DbContext
{
    // just a constructor
    public ApplicationDBContext(DbContextOptions options) : base(options) 
    {
    }
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}