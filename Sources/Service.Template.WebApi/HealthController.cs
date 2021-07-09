using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Service.Template.Consumers.Healthchecks;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Service.Template.WebApi
{
    /// <summary>
    /// Контроллер проверки состояния службы.
    /// </summary>
    [ApiController]
    [Route("/health")]
    public class HealthController : ControllerBase
    {
        private readonly IRequestClient<HealthcheckCommand> healthClient;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HealthController"/>.
        /// </summary>
        /// <param name="healthClient">Клиент получения статуса службы.</param>
        public HealthController(IRequestClient<HealthcheckCommand> healthClient)
        {
            this.healthClient = healthClient;
        }

        /// <summary>
        /// Получить статус службы.
        /// </summary>
        /// <returns>Статус службы.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Response<HealthcheckResponse> response = await this.healthClient.GetResponse<HealthcheckResponse>(new HealthcheckCommand());

                return response.Message.Result == "success" ? this.Ok() : new StatusCodeResult((int)HttpStatusCode.BadGateway);
            }
            catch (RequestTimeoutException)
            {
                return new StatusCodeResult((int)HttpStatusCode.GatewayTimeout);
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadGateway);
            }
        }
    }
}