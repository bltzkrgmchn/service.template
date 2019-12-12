using System.Collections.Generic;

namespace Service.Template.Core
{
    /// <summary>
    /// Сервисный объект для управления Placeholder.
    /// </summary>
    public interface IPlaceholderService
    {
        /// <summary>
        /// Получить Placeholder.
        /// </summary>
        /// <param name="id">Идентификатор Placeholder.</param>
        /// <returns>Placeholder.</returns>
        Placeholder Get(string id);

        /// <summary>
        /// Получить список Placeholder.
        /// </summary>
        /// <returns>Список Placeholder.</returns>
        List<Placeholder> Get();
    }
}