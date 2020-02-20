using Service.Template.Core;

namespace Service.Template.Consumers
{
    /// <summary>
    /// Ответ на команду получения Placeholder.
    /// </summary>
    public class GetPlaceholderResponse
    {
        /// <summary>
        /// Получает или задает Placeholder.
        /// </summary>
        public Placeholder Placeholder { get; set; }

        /// <summary>
        /// Получает или задает результат обработки команды.
        /// </summary>
        public string Result { get; set; }
    }
}