using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MovieAPi.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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
        public virtual DbSet<HttpRequestLog> HttpRequestLogs { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

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

            modelBuilder.Entity<Studio>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Movie>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Tag>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<MovieSchedule>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Order>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(s => s.DeletedAt == null);
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUser = "SYSTEM";
            
            if (_httpContextAccessor.HttpContext != null)
            {
                currentUser = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            }
            
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = currentUser;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = currentUser;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedAt = now;
                        entry.Entity.DeletedBy = currentUser;
                        // override Remove default behaviour to soft delete
                        base.Update(entry.Entity);
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}