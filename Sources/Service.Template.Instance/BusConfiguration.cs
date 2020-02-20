namespace Service.Template.Instance
{
    /// <summary>
    /// Конфигурация шины.
    /// </summary>
    public class BusConfiguration
    {
        /// <summary>
        /// Получает или задает строку подключения.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Получает или задает пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Получает или задает значение, показывающее, флаг подтверждения получения сообщений.
        /// </summary>
        public bool PublisherConfirmation { get; set; }
    }
}