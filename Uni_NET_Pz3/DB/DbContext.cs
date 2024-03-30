using Microsoft.EntityFrameworkCore;

namespace Uni_NET_Pz3.DB;

public class SqlLiteDbContext : DbContext
{
    public DbSet<Content> Contents { get; set; } = null!;
    // TODO: когда добавляю новый объект через круд - он также прокидывается в базу данных. 
    // TODO: когда удаляю он удаляется. 
    // TODO: обновления в такой системе невозможны, но мне на 60. 
    // TODO: получение по айдишникам из БД.
}


public class Content
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}