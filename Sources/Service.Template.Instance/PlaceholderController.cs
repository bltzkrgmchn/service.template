using MassTransit;
using Service.Template.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Net;

namespace Service.Template.Instance
{
    [Route("/placeholders")]
    public class PlaceholderController : Controller
    {
        private readonly IRequestClient<GetSinglePlaceholderCommand> getSinglePlaceholderClient;
        private readonly IRequestClient<GetAllPlaceholdersCommand> getAllPlaceholdersClient;

        public PlaceholderController(IRequestClient<GetSinglePlaceholderCommand> getSinglePlaceholderClient, IRequestClient<GetAllPlaceholdersCommand> getAllPlaceholdersClient)
        {
            this.getSinglePlaceholderClient = getSinglePlaceholderClient;
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
        public async Task<IActionResult> GetSingle(string placeholderName)
        {
            try
            {
                RequestHandle<GetSinglePlaceholderCommand> request = this.getSinglePlaceholderClient.Create(new GetSinglePlaceholderCommand { Name = placeholderName });
                var response = await request.GetResponse<GetSinglePlaceholderResponse>();
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