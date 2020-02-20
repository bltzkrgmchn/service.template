namespace Service.Template.Consumers.Healthchecks
{
    /// <summary>
    /// Ответ на команду получения состояния службы.
    /// </summary>
    public class HealthcheckResponse
    {
        /// <summary>
        /// Получает или задает результат обработки команды.
        /// </summary>
        public string Result { get; set; }
    }
}