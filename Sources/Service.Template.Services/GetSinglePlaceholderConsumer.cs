using System.Threading.Tasks;
using MassTransit;
using Service.Template.Core;

namespace Service.Template.Services
{
    public class GetSinglePlaceholderConsumer : IConsumer<GetSinglePlaceholderCommand>
    {
        private readonly IPlaceholderService service;

        public GetSinglePlaceholderConsumer(IPlaceholderService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GetSinglePlaceholderCommand> context)
        {
            Placeholder placeholder = this.service.Get(context.Message.Name);

            await context.RespondAsync(new GetSinglePlaceholderResponse { Placeholder = placeholder });
        }
    }
}

