using System.Collections.Generic;

namespace Service.Template.Core
{
    public class PlaceholderService : IPlaceholderService
    {
        private readonly IPlaceholderGateway placeholderGateway;

        public PlaceholderService(IPlaceholderGateway placeholderGateway)
        {
            this.placeholderGateway = placeholderGateway;
        }

        public Placeholder Get(string name)
        {
            return this.placeholderGateway.Find(name);
        }

        public List<Placeholder> Get()
        {
            return this.placeholderGateway.Find();
        }
    }
}
