using Microsoft.EntityFrameworkCore;

namespace NeoTask.WebApp.Data;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    // Add DbSets here later
}
