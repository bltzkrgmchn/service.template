using System.Collections.Generic;

namespace Service.Template.Core
{
    /// <summary>
    /// Хранилище Placeholder.
    /// </summary>
    public interface IPlaceholderRepository
    {
        /// <summary>
        /// Получает список Placeholder.
        /// </summary>
        /// <returns>Список Placeholder.</returns>
        List<Placeholder> Find();

        /// <summary>
        /// Получает Placeholder.
        /// </summary>
        /// <param name="id">Идентификатор Placeholder.</param>
        /// <returns>Placeholder.</returns>
        Placeholder Find(string id);
    }
}