using Nancy;
using Nancy.TinyIoc;
using Facade.Template.Instance.Gateways;

namespace Facade.Template.Instance
{
    public class Bootstraper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IPlaceholderGateway, PlaceholderGateway>().AsSingleton();

            base.ConfigureApplicationContainer(container);
        }
    }
}