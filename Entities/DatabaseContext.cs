using Microsoft.EntityFrameworkCore;

namespace MovieAPi.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        
        public virtual DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=MovieApi;User ID=sa;Password=mssql1Ipw;Integrated Security=false;Connect Timeout=10;TrustServerCertificate=true");
            }
        }
    }
}