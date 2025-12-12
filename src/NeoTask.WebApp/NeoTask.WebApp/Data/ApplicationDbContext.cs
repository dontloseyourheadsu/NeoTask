using Microsoft.EntityFrameworkCore;

namespace NeoTask.WebApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
}
