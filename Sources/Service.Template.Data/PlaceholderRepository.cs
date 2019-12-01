using Microsoft.EntityFrameworkCore;
using Service.Template.Core;
using System.Collections.Generic;
using System.Linq;

namespace Service.Template.Data
{
    public class PlaceholderRepository : IPlaceholderRepository
    {
        private readonly string connectionString;

        public PlaceholderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Placeholder> Find()
        {
            using (PlaceholderContext context = new PlaceholderContext(this.connectionString))
            {
                DbSet<PlaceholderDto> dtos = context.Placeholders;
                return dtos.Select(o => new Placeholder(o.Name)).ToList();
            }
        }

        public Placeholder Find(string name)
        {
            using (PlaceholderContext context = new PlaceholderContext(this.connectionString))
            {
                PlaceholderDto dto = context.Placeholders.SingleOrDefault(o => o.Name == name);
                return new Placeholder(dto.Name);
            }
        }
    }
}