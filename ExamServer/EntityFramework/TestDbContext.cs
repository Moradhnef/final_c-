using Server.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static System.Net.Mime.MediaTypeNames;

namespace Server.EntityFramework
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<User> Users { get; set; }
      
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().HasMany(i => i.Questions);
            modelBuilder.Entity<Question>().HasMany(i => i.Answers);

            modelBuilder.Entity<Test>()
            .Property(s => s.TotalTime)
            .HasConversion(new TimeSpanToTicksConverter());
            modelBuilder.Entity<User>().HasData(
           new User
           {
               Id = 1,
               Username = "Prof",
               Password = "123456",
               IsLector = true
           });
         
        }
     
    }
}
