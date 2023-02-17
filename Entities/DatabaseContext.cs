using Microsoft.EntityFrameworkCore;

namespace MovieAPi.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DatabaseContext()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieTag> MovieTags { get; set; }
        public virtual DbSet<Studio> Studios { get; set; }
        public virtual DbSet<MovieSchedule> MovieSchedules { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=127.0.0.1;Initial Catalog=MovieApi;User ID=sa;Password=mssql1Ipw;Integrated Security=false;Connect Timeout=10;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieTag>().HasKey(sc =>
                new
                {
                    sc.MovieId, sc.TagId
                });

            modelBuilder.Entity<MovieTag>()
                .HasOne(mt => mt.Movie)
                .WithMany(m => m.MovieTags)
                .HasForeignKey(mt => mt.MovieId);

            modelBuilder.Entity<MovieTag>()
                .HasOne(mt => mt.Tag)
                .WithMany(m => m.MovieTags)
                .HasForeignKey(mt => mt.TagId);
        }
    }
}