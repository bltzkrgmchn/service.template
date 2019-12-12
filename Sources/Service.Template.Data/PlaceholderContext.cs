using Microsoft.EntityFrameworkCore;

namespace Service.Template.Data
{
    /// <summary>
    /// Контекст базы данных Placeholder.
    /// </summary>
    public class PlaceholderContext : DbContext
    {
        private readonly string connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PlaceholderContext"/>.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        public PlaceholderContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Получает или задает содержимое базы данных Placeholder.
        /// </summary>
        public DbSet<PlaceholderDto> Placeholders { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }
    }
}