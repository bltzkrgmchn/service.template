using System.Collections.Generic;

namespace Service.Template.Core
{
    /// <inheritdoc/>
    public class PlaceholderService : IPlaceholderService
    {
        private readonly IPlaceholderRepository placeholderRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PlaceholderService"/>.
        /// </summary>
        /// <param name="placeholderRepository">Хранилище Placeholder.</param>
        public PlaceholderService(IPlaceholderRepository placeholderRepository)
        {
            this.placeholderRepository = placeholderRepository;
        }

        /// <inheritdoc/>
        public Placeholder Get(string id)
        {
            return this.placeholderRepository.Find(id);
        }

        /// <inheritdoc/>
        public List<Placeholder> Get()
        {
            return this.placeholderRepository.Find();
        }
    }
}