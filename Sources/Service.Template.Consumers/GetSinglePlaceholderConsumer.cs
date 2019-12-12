using System.Threading.Tasks;
using MassTransit;
using Service.Template.Core;

namespace Service.Template.Consumers
{
    public class GetPlaceholderConsumer : IConsumer<GetPlaceholderCommand>
    {
        private readonly IPlaceholderService service;

        public GetPlaceholderConsumer(IPlaceholderService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GetPlaceholderCommand> context)
        {
            Placeholder placeholder = this.service.Get(context.Message.Name);

            await context.RespondAsync(new GetPlaceholderResponse { Placeholder = placeholder });
        }
    }
}

