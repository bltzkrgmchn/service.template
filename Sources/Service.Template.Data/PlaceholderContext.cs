using Microsoft.EntityFrameworkCore;

namespace Service.Template.Data
{
    public class PlaceholderContext : DbContext
    {
        private readonly string connectionString;

        public PlaceholderContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<PlaceholderDto> Placeholders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }
    }
}