using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Service.Template.Consumers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Service.Template.WebApi
{
    /// <summary>
    /// Конролер Placeholder.
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
        /// Получить список Placeholder.
        /// </summary>
        /// <returns>Список Placeholder.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                RequestHandle<GetAllPlaceholdersCommand> request = this.getAllPlaceholdersClient.Create(new GetAllPlaceholdersCommand());
                Response<GetAllPlaceholdersResponse> response = await request.GetResponse<GetAllPlaceholdersResponse>();
                System.Collections.Generic.List<Core.Placeholder> placeholdes = response.Message.Placeholders;
                return this.Ok(response.Message.Placeholders);
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
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                RequestHandle<GetPlaceholderCommand> request = this.getPlaceholderClient.Create(new GetPlaceholderCommand { Id = id });
                Response<GetPlaceholderResponse> response = await request.GetResponse<GetPlaceholderResponse>();
                return this.Ok(response.Message.Placeholder);
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