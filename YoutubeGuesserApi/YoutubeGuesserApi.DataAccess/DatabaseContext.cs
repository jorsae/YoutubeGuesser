using Microsoft.EntityFrameworkCore;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<YoutubeVideo> YoutubeVideos { get; set; }
        public DbSet<Guesses> Guesses { get; set; }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<Profanity> Profanities { get; set; }
        public DbSet<Highscore> Highscores { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

        public DatabaseContext(string connectionString) : base(GetOptions(connectionString))
        {
            Database.EnsureCreated();
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}