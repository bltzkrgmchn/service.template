using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;

namespace Service.Template.WebApi
{
    /// <summary>
    /// Фильтр проверки токена авторизации.
    /// </summary>
    public class AuthorizationFilter : Attribute, IActionFilter
    {
        private readonly IRequestClient<AuthorizeCommand> authorizationClient;
        private readonly ILogger<AuthorizationFilter> logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationFilter"/>.
        /// </summary>
        /// <param name="authorizationClient">Клиент проверки токена авторизации.</param>
        /// <param name="logger">Абстракция над сервисом журналирования.</param>
        public AuthorizationFilter(IRequestClient<AuthorizeCommand> authorizationClient, ILogger<AuthorizationFilter> logger)
        {
            this.authorizationClient = authorizationClient;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues headerTokens))
            {
                string token = headerTokens.LastOrDefault();
                try
                {
                    AuthorizeCommand command = new AuthorizeCommand { Token = token };
                    Response<AuthorizeResponse> response = this.authorizationClient
                        .GetResponse<AuthorizeResponse>(command)
                        .GetAwaiter()
                        .GetResult();

                    if (response.Message.Result != "success")
                    {
                        this.logger.LogWarning($"Не удалось валидировать токен авторизации '{token}'");
                        context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                    }
                }
                catch (RequestTimeoutException e)
                {
                    this.logger.LogError("Превышено время ожидания ответа от сервиса авторизации.", e);
                    context.Result = new StatusCodeResult((int)HttpStatusCode.GatewayTimeout);
                }
                catch (Exception e)
                {
                    this.logger.LogError("Неизвестная ошибка во время авторизации.", e);
                    context.Result = new StatusCodeResult((int)HttpStatusCode.BadGateway);
                }
            }
            else
            {
                this.logger.LogWarning("Токен авторизации не найден.");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}