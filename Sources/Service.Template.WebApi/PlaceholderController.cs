using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Service.Template.Consumers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Service.Template.WebApi
{
    /// <summary>
    /// Контроллер Placeholder.
    /// </summary>
    [Route("/placeholders")]
    public class PlaceholderController : Controller
    {
        private readonly IRequestClient<GetPlaceholderCommand> getPlaceholderClient;
        private readonly IRequestClient<GetAllPlaceholdersCommand> getAllPlaceholdersClient;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PlaceholderController"/>.
        /// </summary>
        /// <param name="getPlaceholderClient">Клиент получения Placeholder.</param>
        /// <param name="getAllPlaceholdersClient">Клиент получения списка Placeholder.</param>
        public PlaceholderController(IRequestClient<GetPlaceholderCommand> getPlaceholderClient, IRequestClient<GetAllPlaceholdersCommand> getAllPlaceholdersClient)
        {
            this.getPlaceholderClient = getPlaceholderClient;
            this.getAllPlaceholdersClient = getAllPlaceholdersClient;
        }

        /// <summary>
        /// Метод для получения доступным методов.
        /// </summary>
        /// <returns>Список доступных методов.</returns>
        [HttpOptions]
        public IActionResult Options()
        {
            this.Response.Headers.Add("Allow", "GET, OPTIONS");
            return this.Ok();
        }

        /// <summary>
        /// Получить список Placeholder.
        /// </summary>
        /// <returns>Список Placeholder.</returns>
        [HttpGet]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                Response<GetAllPlaceholdersResponse> response = await this.getAllPlaceholdersClient.GetResponse<GetAllPlaceholdersResponse>(new GetAllPlaceholdersCommand());

                if (response.Message.Result == "success")
                {
                    return this.Ok(response.Message.Placeholders);
                }

                return new StatusCodeResult((int)HttpStatusCode.BadGateway);
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

        /// <summary>
        /// Получить Placeholder.
        /// </summary>
        /// <param name="id">Идентификатор Placeholder.</param>
        /// <returns>Placeholder.</returns>
        [HttpGet]
        [Route("{id}")]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                Response<GetPlaceholderResponse> response = await this.getPlaceholderClient.GetResponse<GetPlaceholderResponse>(new GetPlaceholderCommand { Id = id });

                switch (response.Message.Result)
                {
                    case "success":
                        return this.Ok(response.Message.Placeholder);
                    case "not-found":
                        return new StatusCodeResult((int)HttpStatusCode.NotFound);
                    default:
                        return new StatusCodeResult((int)HttpStatusCode.BadGateway);
                }
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