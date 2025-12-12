using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NeoTask.WebApp.Data;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // Add DbSets here later
}
