using Microsoft.EntityFrameworkCore;

namespace Uni_NET_Pz3.DB;

public class SqlLiteDbContext : DbContext
{
    public SqlLiteDbContext(DbContextOptions<SqlLiteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Content> Contents { get; set; } = null!;
}


public class Content
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}