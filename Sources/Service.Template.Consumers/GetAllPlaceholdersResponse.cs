using Service.Template.Core;
using System.Collections.Generic;

namespace Service.Template.Consumers
{
    /// <summary>
    /// Ответ на команду получения списка Placeholder.
    /// </summary>
    public class GetAllPlaceholdersResponse
    {
        /// <summary>
        /// Получает или задает список Placeholder.
        /// </summary>
        public List<Placeholder> Placeholders { get; set; }

        /// <summary>
        /// Получает или задает результат обработки команды.
        /// </summary>
        public string Result { get; set; }
    }
}