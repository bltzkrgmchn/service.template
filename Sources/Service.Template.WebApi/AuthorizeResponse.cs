namespace Service.Template.WebApi
{
    /// <summary>
    /// Ответ на команду проверки токена авторизации.
    /// </summary>
    public class AuthorizeResponse
    {
        /// <summary>
        /// Получает или задает результат проверки токена авторизации.
        /// </summary>
        public string Result { get; set; }
    }
}