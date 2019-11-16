using Nancy;
using Service.Template.Instance.Gateways;

namespace Service.Template.Instance.Modules
{
    public sealed class PlaceholderModule : NancyModule
    {
        private readonly IPlaceholderGateway placeholderGateway;

        public PlaceholderModule(IPlaceholderGateway placeholderGateway)
        {
            this.placeholderGateway = placeholderGateway;

            this.Get("/placeholders/{placeholderId}", o => { return placeholderGateway.Find(o.placeholderId); });
            this.Get("/placeholders", o => { return placeholderGateway.Find(); });
        }
    }
}