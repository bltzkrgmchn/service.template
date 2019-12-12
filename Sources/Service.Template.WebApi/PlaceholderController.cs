using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Service.Template.Consumers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Service.Template.WebApi
{
    [Route("/placeholders")]
    public class PlaceholderController : Controller
    {
        private readonly IRequestClient<GetPlaceholderCommand> getPlaceholderClient;
        private readonly IRequestClient<GetAllPlaceholdersCommand> getAllPlaceholdersClient;

        public PlaceholderController(IRequestClient<GetPlaceholderCommand> getPlaceholderClient, IRequestClient<GetAllPlaceholdersCommand> getAllPlaceholdersClient)
        {
            this.getPlaceholderClient = getPlaceholderClient;
            this.getAllPlaceholdersClient = getAllPlaceholdersClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                RequestHandle<GetAllPlaceholdersCommand> request = this.getAllPlaceholdersClient.Create(new GetAllPlaceholdersCommand());
                Response<GetAllPlaceholderResponse> response = await request.GetResponse<GetAllPlaceholderResponse>();
                var placeholdes = response.Message.Placeholders;
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

        [HttpGet]
        [Route("{placeholderName}")]
        public async Task<IActionResult> Get(string placeholderName)
        {
            try
            {
                RequestHandle<GetPlaceholderCommand> request = this.getPlaceholderClient.Create(new GetPlaceholderCommand { Name = placeholderName });
                var response = await request.GetResponse<GetPlaceholderResponse>();
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