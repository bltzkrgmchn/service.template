using Microsoft.EntityFrameworkCore;
using Service.Template.Core;
using System.Collections.Generic;
using System.Linq;

namespace Service.Template.Data
{
    /// <inheritdoc />
    public class PlaceholderRepository : IPlaceholderRepository
    {
        private readonly string connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PlaceholderRepository"/>.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        public PlaceholderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <inheritdoc />
        public List<Placeholder> Find()
        {
            using (PlaceholderContext context = new PlaceholderContext(this.connectionString))
            {
                DbSet<PlaceholderDto> dtos = context.Placeholders;
                return dtos.Select(o => new Placeholder(o.Id)).ToList();
            }
        }

        /// <inheritdoc />
        public Placeholder Find(string name)
        {
            using (PlaceholderContext context = new PlaceholderContext(this.connectionString))
            {
                PlaceholderDto dto = context.Placeholders.FirstOrDefault(o => o.Id == name);
                return new Placeholder(dto.Id);
            }
        }
    }
}