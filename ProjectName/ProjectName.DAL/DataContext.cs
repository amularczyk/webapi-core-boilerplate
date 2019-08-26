using Microsoft.EntityFrameworkCore;
using ProjectName.Core.Models;

namespace ProjectName.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureEntityItem(builder);
        }

        private void ConfigureEntityItem(ModelBuilder builder)
        {
            builder.Entity<Item>()
                .Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}