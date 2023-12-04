using Microsoft.EntityFrameworkCore;
using MyChannel.DataBaseContext.DBModels;

namespace MyChannel.DataBaseContext
{
    internal class MyDBContext(DbContextOptions<DbContext> options) : DbContext(options)
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<ImageInfo> Images { get; set; }

        public DbSet<UserInfo> Users { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
