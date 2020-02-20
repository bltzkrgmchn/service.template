namespace Service.Template.WebApi
{
    /// <summary>
    /// Команда проверки авторизации.
    /// </summary>
    public class AuthorizeCommand
    {
        /// <summary>
        /// Получает или задает токен авторизации.
        /// </summary>
        public string Token { get; set; }
    }
}