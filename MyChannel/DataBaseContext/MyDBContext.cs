using Microsoft.EntityFrameworkCore;
using MyChannel.DataBaseContext.DBModels;

namespace MyChannel.DataBaseContext
{
    internal class MyDBContext(DbContextOptions<MyDBContext> options) : DbContext(options)
    {
        public DbSet<UserInfoEntity> UserInfoEntity { get; set; }

        public DbSet<FileIDInfoEntity> FileIDInfoEntity { get; set; }

        public DbSet<ChannelInfoEntity> ChannelInfoEntity { get; set; }

        public DbSet<MessageInfoEntity> MessageInfoEntity { get; set; }

        public DbSet<HashTagInfoEntity> HashTagInfoEntity { get; set; }

        public DbSet<GroupInfoEntity> GroupInfoEntity { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) => base.ConfigureConventions(configurationBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);
    }
}
