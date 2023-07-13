using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class WebAPIContext : DbContext
    {
        public WebAPIContext(DbContextOptions<WebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Command> Commands { get; set; }


    }
}
